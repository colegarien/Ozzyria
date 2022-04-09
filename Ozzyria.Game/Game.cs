using Ozzyria.Game.Component;
using Ozzyria.Game.Event;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Ozzyria.Game
{
    public class Game
    {
        public EntityManager entityManager;
        public TileMap tileMap;

        public List<IEventHandler> eventHandlers;
        public List<IEvent> events;

        public Game()
        {
            var worldLoader = new WorldPersistence();
            tileMap = worldLoader.LoadMap("test_m");
            entityManager = worldLoader.LoadEntityManager("test_e");

            var slimeSpawn = new Entity();
            slimeSpawn.AttachComponent(new SlimeSpawner());
            entityManager.Register(slimeSpawn);

            eventHandlers = new List<IEventHandler>();
            events = new List<IEvent>();
        }

        public void AttachEventHandler(IEventHandler handler)
        {
            eventHandlers.Add(handler);
        }

        public int OnPlayerJoin(int id)
        {
            return entityManager.Register(EntityFactory.CreatePlayer(id));
        }

        public void OnPlayerInput(int id, Input input)
        {
            // TODO this is a little weird
            entityManager.GetEntity(id).AttachComponent(input);
        }

        public void OnPlayerLeave(int id)
        {
            // Do something special for players
            entityManager.DeRegister(id);
        }

        public void Update(float deltaTime)
        {
            foreach (var entity in entityManager.GetEntities())
            {
                // Death Check
                if (entity.HasComponent(ComponentType.Stats) && entity.GetComponent<Stats>(ComponentType.Stats).IsDead())
                {
                    continue;
                }

                // Handle thoughts
                if (entity.HasComponent(ComponentType.Thought))
                {
                    entity.GetComponent<Thought>(ComponentType.Thought).Update(deltaTime, entityManager);

                    // TODO OZ-14 : likey move this to collision thang (when it exists)
                    if (entity.HasComponent(ComponentType.ExperienceBoost) && entity.GetComponent<ExperienceBoost>(ComponentType.ExperienceBoost).HasBeenAbsorbed)
                    {
                        entityManager.DeRegister(entity.Id);
                    }
                }

                // TODO OZ-24 : consider the "Layer" in movement the entities are on when doing collision and such

                // Handle Collisions
                if (entity.HasComponent(ComponentType.Collision) && entity.GetComponent<Collision>(ComponentType.Collision).IsDynamic)
                {
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                    var collision = entity.GetComponent<Collision>(ComponentType.Collision);

                    var possibleCollisions = entityManager.GetEntities().Where(e => e.Id != entity.Id && e.HasComponent(ComponentType.Collision));
                    var depthVector = Vector2.Zero;
                    foreach (var collidedEntity in possibleCollisions)
                    {
                        var otherMovement = collidedEntity.GetComponent<Movement>(ComponentType.Movement);
                        var otherCollision = collidedEntity.GetComponent<Collision>(ComponentType.Collision);

                        if (collision is BoundingCircle && otherCollision is BoundingCircle)
                        {
                            var result = Collision.CircleIntersectsCircle((BoundingCircle)collision, (BoundingCircle)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is BoundingBox && otherCollision is BoundingBox)
                        {
                            var result = Collision.BoxIntersectsBox((BoundingBox)collision, (BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is BoundingCircle && otherCollision is BoundingBox)
                        {
                            var result = Collision.CircleIntersectsBox((BoundingCircle)collision, (BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if (collision is BoundingBox && otherCollision is BoundingCircle)
                        {
                            var result = Collision.BoxIntersectsCircle((BoundingBox)collision, (BoundingCircle)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                    }
                    movement.X += depthVector.X;
                    movement.Y += depthVector.Y;
                }

                // Handle Combat
                if (entity.HasComponent(ComponentType.Combat) && entity.GetComponent<Combat>(ComponentType.Combat).Attacking)
                {
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                    var combat = entity.GetComponent<Combat>(ComponentType.Combat);

                    var entitiesInRange = entityManager.GetEntities().Where(e => entity.Id != e.Id && e.HasComponent(ComponentType.Movement) && e.HasComponent(ComponentType.Combat) && e.HasComponent(ComponentType.Stats) && e.GetComponent<Movement>(ComponentType.Movement).DistanceTo(movement) <= combat.AttackRange);
                    foreach (var target in entitiesInRange)
                    {
                        var targetMovement = target.GetComponent<Movement>(ComponentType.Movement);
                        var targetStats = target.GetComponent<Stats>(ComponentType.Stats);

                        var angleToTarget = AngleHelper.AngleTo(movement.X, movement.Y, targetMovement.X, targetMovement.Y);
                        if (AngleHelper.IsInArc(angleToTarget, movement.LookDirection, combat.AttackAngle))
                        {
                            targetStats.Damage(combat.AttackDamage);
                            if (targetStats.IsDead())
                            {
                                events.Add(new EntityDead { Id = target.Id });
                            }
                        }
                    }
                }
            }

            ProcessEvents();
        }

        private void ProcessEvents()
        {
            while (events.Count > 0)
            {
                var gameEvent = events[0];

                if (gameEvent is EntityDead)
                {
                    var entity = entityManager.GetEntity(((EntityDead)gameEvent).Id);
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);

                    entityManager.Register(EntityFactory.CreateExperienceOrb(movement.X, movement.Y, 10));
                    entityManager.DeRegister(entity.Id);

                    if (entity.HasComponent(ComponentType.Thought) && entity.GetComponent<Thought>(ComponentType.Thought) is PlayerThought)
                    {
                        // kick player out
                        OnPlayerLeave(entity.Id);// TODO OZ-14 : create Graveyard component and add a AssignedGraveyard to players, then just revive them there
                    }
                }

                foreach (var eventHandler in eventHandlers)
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
