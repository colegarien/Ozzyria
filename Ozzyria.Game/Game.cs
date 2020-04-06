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

        public List<ExperienceOrb> orbs;
        public List<Slime> slimes;
        private float eventTimer = 0;

        public List<IEventHandler> eventHandlers;
        public List<IEvent> events;

        public Game()
        {
            inputs = new Dictionary<int, Input>();
            players = new Dictionary<int, Player>();

            orbs = new List<ExperienceOrb>();
            orbs.Add(new ExperienceOrb { Movement = new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = 400, Y = 300 } });

            slimes = new List<Slime>();
            slimes.Add(new Slime { Movement = new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = 500, Y = 400 } });

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

        public void Update(float deltaTime)
        {
            eventTimer += deltaTime;
            foreach(var idPlayerPair in players)
            {
                var input = inputs.GetValueOrDefault(idPlayerPair.Key) ?? new Input();
                var player = idPlayerPair.Value;

                if (player.IsDead())
                {
                    continue;
                }

                player.Update(deltaTime, input);
                if (player.Attacking)
                {
                    var slimesInRange = slimes.Where(s => Math.Sqrt(Math.Pow(s.Movement.X - player.Movement.X, 2) + Math.Pow(s.Movement.Y - player.Movement.Y, 2)) <= player.AttackRange);
                    foreach(var target in slimesInRange)
                    {
                        var angleToTarget = AngleHelper.AngleTo(player.Movement.X, player.Movement.Y, target.Movement.X, target.Movement.Y);
                        if (AngleHelper.IsInArc(angleToTarget, player.Movement.LookDirection, player.AttackAngle))
                        {
                            target.Damage(player.AttackDamage);
                            if (target.IsDead())
                            {
                                events.Add(new SlimeDead { Slime = target });
                            }
                        }
                    }
                }
            }

            foreach(var orb in orbs)
            {
                orb.Update(deltaTime, players.Values.ToArray());
            }
            // remove any orbs that have been absorbed
            orbs = orbs.Where(o => !o.HasBeenAbsorbed).ToList();

            foreach (var slime in slimes)
            {
                if (slime.IsDead())
                {
                    continue;
                }

                slime.Update(deltaTime, players.Values.ToArray());
                if (slime.Attacking)
                {
                    var playersInRange = players.Values.Where(p => Math.Sqrt(Math.Pow(p.Movement.X - slime.Movement.X, 2) + Math.Pow(p.Movement.Y - slime.Movement.Y, 2)) <= slime.AttackRange);
                    foreach (var target in playersInRange)
                    {
                        var angleToTarget = AngleHelper.AngleTo(slime.Movement.X, slime.Movement.Y, target.Movement.X, target.Movement.Y);
                        if (AngleHelper.IsInArc(angleToTarget, slime.Movement.LookDirection, slime.AttackAngle))
                        {
                            target.Damage(slime.AttackDamage);
                            if (target.IsDead())
                            {
                                events.Add(new PlayerDead { PlayerId = target.Id });
                            }
                        }
                    }
                }
            }
            // remove any dead slimes
            slimes = slimes.Where(s => !s.IsDead()).ToList();

            if (eventTimer > 5)
            {
                eventTimer = 0;
                if (slimes.Count < 3)
                {
                    slimes.Add(new Slime { Movement = new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = 500, Y = 400 } });
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
                else if(gameEvent is SlimeDead)
                {
                    var slime = ((SlimeDead)gameEvent).Slime;
                    orbs.Add(new ExperienceOrb { Movement = new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = slime.Movement.X, Y = slime.Movement.Y }, Experience = 10 });
                    slimes.Remove(slime);
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
