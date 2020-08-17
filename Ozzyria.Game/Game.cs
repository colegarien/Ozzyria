using Ozzyria.Game.Component;
using Ozzyria.Game.Event;
using Ozzyria.Game.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace Ozzyria.Game
{
    public class Game
    {
        public EntityManager entityManager;
        public TileMap tileMap;

        private float eventTimer = 0;

        public List<IEventHandler> eventHandlers;
        public List<IEvent> events;

        public Game()
        {
            entityManager = new EntityManager();
            entityManager.Register(CreateOrb(400, 300, 30));
            entityManager.Register(CreateSlime(500, 400));

            var box = new Entity();
            box.AttachComponent(new Movement() { X = 60, Y = 60, PreviousX = 60, PreviousY = 60 });
            box.AttachComponent(new BoundingCircle() { IsDynamic = false, Radius = 10 });
            //box.AttachComponent(new BoundingBox() { IsDynamic = false, Width = 20, Height = 20});
            entityManager.Register(box);

            // wrap screen in border
            entityManager.Register(CreateWall(400, 20, 900, 10));
            entityManager.Register(CreateWall(400, 510, 900, 10));
            entityManager.Register(CreateWall(20, 300, 10, 700));
            entityManager.Register(CreateWall(780, 300, 10, 700));

            entityManager.Register(CreateWall(150, 300, 400, 10));
            entityManager.Register(CreateWall(200, 300, 10, 300));

            tileMap = new TileMap();

            eventHandlers = new List<IEventHandler>();
            events = new List<IEvent>();
        }

        public void AttachEventHandler(IEventHandler handler)
        {
            eventHandlers.Add(handler);
        }

        public int OnPlayerJoin(int id)
        {
            var player = new Entity { Id = id };
            player.AttachComponent(new Renderable { Sprite = SpriteType.Player });
            player.AttachComponent(new PlayerThought());
            player.AttachComponent(new Movement() { X = 140, Y = 140 });
            player.AttachComponent(new Stats());
            player.AttachComponent(new Combat());
            player.AttachComponent(new Input());
            player.AttachComponent(new BoundingCircle { Radius = 10 });

            return entityManager.Register(player);
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

        protected Entity CreateSlime(float x, float y)
        {
            var slime = new Entity();
            slime.AttachComponent(new Renderable { Sprite = SpriteType.Slime });
            slime.AttachComponent(new SlimeThought());
            slime.AttachComponent(new Movement { MAX_SPEED = 50f, ACCELERATION = 300f, X = x, Y = y });
            slime.AttachComponent(new Stats { Health = 30, MaxHealth = 30 });
            slime.AttachComponent(new Combat());
            slime.AttachComponent(new BoundingCircle { Radius = 10 });

            return slime;
        }

        protected Entity CreateOrb(float x, float y, int value)
        {
            var orb = new Entity();
            orb.AttachComponent(new Renderable { Sprite = SpriteType.Particle });
            orb.AttachComponent(new ExperienceOrbThought());
            orb.AttachComponent(new Movement { ACCELERATION = 200f, MAX_SPEED = 300f, X = x, Y = y });
            orb.AttachComponent(new ExperienceBoost { Experience = value });

            return orb;
        }

        protected Entity CreateWall(float x, float y, int w, int h)
        {
            var wall = new Entity();
            wall.AttachComponent(new Movement() { X = x, Y = y, PreviousX = x, PreviousY = y });
            wall.AttachComponent(new BoundingBox() { IsDynamic = false, Width = w, Height = h });

            return wall;
        }

        public void Update(float deltaTime)
        {
            eventTimer += deltaTime;
            foreach (var entity in entityManager.GetEntities())
            {
                // Death Check
                if (entity.HasComponent(ComponentType.Stats) && entity.GetComponent<Stats>(ComponentType.Stats).IsDead())
                {
                    continue;
                }

                // Handle thoughts
                if (entity.HasComponent(ComponentType.Thought)) {
                    entity.GetComponent<Thought>(ComponentType.Thought).Update(deltaTime, entityManager);

                    // TODO likey move this to collision thang (when it exists)
                    if (entity.HasComponent(ComponentType.ExperienceBoost) && entity.GetComponent<ExperienceBoost>(ComponentType.ExperienceBoost).HasBeenAbsorbed)
                    {
                        entityManager.DeRegister(entity.Id);
                    }
                }

                // Handle Collisions
                if (entity.HasComponent(ComponentType.Collision) && entity.GetComponent<Collision>(ComponentType.Collision).IsDynamic)
                {
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                    var collision = entity.GetComponent<Collision>(ComponentType.Collision);

                    var possibleCollisions = entityManager.GetEntities().Where(e => e.Id != entity.Id && e.HasComponent(ComponentType.Collision));
                    var depthVector = Vector2.Zero;
                    foreach(var collidedEntity in possibleCollisions) {
                        var otherMovement = collidedEntity.GetComponent<Movement>(ComponentType.Movement);
                        var otherCollision = collidedEntity.GetComponent<Collision>(ComponentType.Collision);

                        if(collision is BoundingCircle && otherCollision is BoundingCircle)
                        {
                            var result = Collision.CircleIntersectsCircle((BoundingCircle)collision, (BoundingCircle)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if(collision is BoundingBox && otherCollision is BoundingBox)
                        {
                            var result = Collision.BoxIntersectsBox((BoundingBox)collision, (BoundingBox)otherCollision);
                            if (result.Collided)
                            {
                                depthVector += new Vector2(result.NormalX, result.NormalY) * result.Depth;
                            }
                        }
                        else if(collision is BoundingCircle && otherCollision is BoundingBox)
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

            if (eventTimer > 5)
            {
                eventTimer = 0;
                if (entityManager.GetEntities().Count(e => e.HasComponent(ComponentType.Thought) && e.GetComponent<Thought>(ComponentType.Thought) is SlimeThought) < 3)
                {
                    entityManager.Register(CreateSlime(500, 400));
                }
            }

            ProcessEvents();
        }

        private void ProcessEvents()
        {
            while(events.Count > 0)
            {
                var gameEvent = events[0];

                if(gameEvent is EntityDead)
                {
                    var entity = entityManager.GetEntity(((EntityDead)gameEvent).Id);
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);

                    entityManager.Register(CreateOrb(movement.X, movement.Y, 10));
                    entityManager.DeRegister(entity.Id);

                    if (entity.HasComponent(ComponentType.Thought) && entity.GetComponent<Thought>(ComponentType.Thought) is PlayerThought)
                    {
                        // kick player out
                        OnPlayerLeave(entity.Id);
                    }
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
