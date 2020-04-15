using Ozzyria.Game.Component;
using Ozzyria.Game.Event;
using Ozzyria.Game.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class Game
    {
        public Dictionary<int, Input> inputs;
        public Dictionary<int, Player> players;

        public int maxEntityId = 1;
        public Dictionary<int, Entity> entities;
        private float eventTimer = 0;

        public List<IEventHandler> eventHandlers;
        public List<IEvent> events;

        public Game()
        {
            inputs = new Dictionary<int, Input>();
            players = new Dictionary<int, Player>();

            entities = new Dictionary<int, Entity>();
            var id = maxEntityId++;
            entities[id] = CreateOrb(id, 400, 300, 30);
            id = maxEntityId++;
            entities[id] = CreateSlime(id, 500, 400);

            eventHandlers = new List<IEventHandler>();
            events = new List<IEvent>();
        }

        public void AttachEventHandler(IEventHandler handler)
        {
            eventHandlers.Add(handler);
        }

        public void OnPlayerJoin(int id)
        {
            players[id] = new Player() { Id = id };
            inputs[id] = new Input();
        }

        public void OnPlayerInput(int id, Input input)
        {
            inputs[id] = input;
        }

        public void OnPlayerLeave(int id)
        {
            if (players.ContainsKey(id))
            {
                players.Remove(id);
            }
            if (inputs.ContainsKey(id))
            {
                inputs.Remove(id);
            }
        }

        protected Entity CreateSlime(int id, float x, float y)
        {
            var slime = new Entity { Id = id };
            slime.AttachComponent(new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = x, Y = y });
            slime.AttachComponent(new Stats { Health = 30, MaxHealth = 30 });
            slime.AttachComponent(new Combat());

            slime.AttachComponent(new SlimeThought());

            return slime;
        }

        protected Entity CreateOrb(int id, float x, float y, int value)
        {
            var orb = new Entity { Id = id };
            orb.AttachComponent(new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = x, Y = y });
            orb.AttachComponent(new ExperienceBoost { Experience = value });

            orb.AttachComponent(new ExperienceOrbThought());

            return orb;
        }

        public void Update(float deltaTime)
        {
            eventTimer += deltaTime;
            foreach(var idPlayerPair in players)
            {
                var input = inputs.GetValueOrDefault(idPlayerPair.Key) ?? new Input();
                var player = idPlayerPair.Value;

                if (player.Stats.IsDead())
                {
                    continue;
                }

                player.Update(deltaTime, input);
                if (player.Combat.Attacking)
                {
                    var combatableEntitiesInRange = entities.Values.Where(e => e.HasComponent(typeof(Movement)) && e.HasComponent(typeof(Combat)) && e.HasComponent(typeof(Stats)) && Math.Sqrt(Math.Pow(((Movement)e.Components[typeof(Movement)]).X - player.Movement.X, 2) + Math.Pow(((Movement)e.Components[typeof(Movement)]).Y - player.Movement.Y, 2)) <= player.Combat.AttackRange);
                    foreach(var target in combatableEntitiesInRange)
                    {
                        var angleToTarget = AngleHelper.AngleTo(player.Movement.X, player.Movement.Y, ((Movement)target.Components[typeof(Movement)]).X, ((Movement)target.Components[typeof(Movement)]).Y);
                        if (AngleHelper.IsInArc(angleToTarget, player.Movement.LookDirection, player.Combat.AttackAngle))
                        {
                            ((Stats)target.Components[typeof(Stats)]).Damage(player.Combat.AttackDamage);
                            if (((Stats)target.Components[typeof(Stats)]).IsDead())
                            {
                                events.Add(new EntityDead { Id = target.Id });
                            }
                        }
                    }
                }
            }

            foreach (var entity in entities.Values)
            {
                // Death Check
                if (entity.HasComponent(typeof(Stats)) && ((Stats)entity.Components[typeof(Stats)]).IsDead())
                {
                    continue;
                }

                // TODO oh shit.. this just got complicated!
                if (entity.HasComponent(typeof(SlimeThought)))
                {
                    ((SlimeThought)entity.Components[typeof(SlimeThought)]).Update(deltaTime, players.Values.ToArray(), entities);
                }
                if (entity.HasComponent(typeof(ExperienceOrbThought)))
                {
                    ((ExperienceOrbThought)entity.Components[typeof(ExperienceOrbThought)]).Update(deltaTime, players.Values.ToArray(), entities);
                    if (((ExperienceBoost)entity.Components[typeof(ExperienceBoost)]).HasBeenAbsorbed)
                    {
                        entities.Remove(entity.Id);
                    }
                }

                // Handle Combat
                if (entity.HasComponent(typeof(Combat)) && ((Combat)entity.Components[typeof(Combat)]).Attacking)
                {
                    var movement = (Movement)entity.Components[typeof(Movement)];
                    var combat = (Combat)entity.Components[typeof(Combat)];

                    var playersInRange = players.Values.Where(p => Math.Sqrt(Math.Pow(p.Movement.X - movement.X, 2) + Math.Pow(p.Movement.Y - movement.Y, 2)) <= combat.AttackRange);
                    foreach (var target in playersInRange)
                    {
                        var angleToTarget = AngleHelper.AngleTo(movement.X, movement.Y, target.Movement.X, target.Movement.Y);
                        if (AngleHelper.IsInArc(angleToTarget, movement.LookDirection, combat.AttackAngle))
                        {
                            target.Stats.Damage(combat.AttackDamage);
                            if (target.Stats.IsDead())
                            {
                                events.Add(new PlayerDead { PlayerId = target.Id });
                            }
                        }
                    }
                }
            }

            if (eventTimer > 5)
            {
                eventTimer = 0;
                if (entities.Values.Count(e => e.HasComponent(typeof(SlimeThought))) < 3)
                {
                    var id = maxEntityId++;
                    entities[id] = CreateSlime(id, 500, 400);
                }
            }

            ProcessEvents();
        }

        private void ProcessEvents()
        {
            while(events.Count > 0)
            {
                var gameEvent = events[0];

                if(gameEvent is PlayerDead)
                {
                    var playerId = ((PlayerDead)gameEvent).PlayerId;
                    // kick player out
                    OnPlayerLeave(playerId);
                }
                else if(gameEvent is EntityDead)
                {
                    var entityId = ((EntityDead)gameEvent).Id;
                    var movement = (Movement)entities[entityId].Components[typeof(Movement)];

                    var id = maxEntityId++;
                    entities[id] = CreateOrb(id, movement.X, movement.Y, 10);
                    entities.Remove(entityId);
                }

                foreach(var eventHandler in eventHandlers)
                {
                    if (eventHandler.CanHandle(gameEvent))
                    {
                        eventHandler.Handle(gameEvent);
                    }
                }

                events.RemoveAt(0);
            }
        }

    }
}
