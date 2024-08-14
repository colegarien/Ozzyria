using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Game.Utility;
using System.Linq;
using Movement = Ozzyria.Model.Components.Movement;
using MovementIntent = Ozzyria.Model.Components.MovementIntent;
using AttackIntent = Ozzyria.Model.Components.AttackIntent;
using Ozzyria.Model.Extensions;
using Ozzyria.Model.Types;

namespace Ozzyria.Game.Systems
{
    internal class Slime : TickSystem
    {
        const float MAX_FOLLOW_DISTANCE = 200;

        protected EntityQuery query;
        protected EntityQuery playerQuery;
        public Slime()
        {
            query = new EntityQuery();
            query.And(typeof(SlimeThought));

            playerQuery = new EntityQuery().And(typeof(Movement), typeof(Stats), typeof(PlayerThought));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            var players = context.GetEntities(playerQuery);
            foreach (var entity in entities)
            {
                // Death Check
                if (entity.HasComponent(typeof(Stats)) && entity.GetComponent<Stats>().IsDead())
                {
                    continue;
                }

                var thought = entity.GetComponent<SlimeThought>();
                var movement = entity.GetComponent<Movement>();
                var weapon = entity.GetComponent<Weapon>();

                var closestPlayer = players
                    .OrderBy(p => movement.DistanceTo(p.GetComponent<Movement>()))
                    .FirstOrDefault();
                var playerMovement = closestPlayer?.GetComponent<Movement>();
                var distanceToPlayer = playerMovement == null ? float.PositiveInfinity : movement.DistanceTo(playerMovement);
                if (closestPlayer == null || distanceToPlayer > MAX_FOLLOW_DISTANCE)
                {
                    Think(deltaTime, thought, movement);
                    continue;
                }

                movement.TurnToward(deltaTime, playerMovement);

                // Initiate attack
                if (!entity.HasComponent(typeof(AttackIntent)) && distanceToPlayer <= weapon.AttackRange)
                {
                    var intent = AttackIntent.GetInstance();
                    intent.Frame = 0;
                    intent.DecayFrame = 3;
                    intent.DamageFrame = 1;
                    intent.FrameTimer = 0f;

                    entity.AddComponent(intent);
                }
            }
        }

        private void Think(float deltaTime, SlimeThought thought, Movement movement)
        {
            thought.ThinkDelay.Update(deltaTime);
            if (thought.ThinkDelay.IsReady())
            {
                thought.ThinkDelay.DelayInSeconds = RandomHelper.Random(0f, 1.5f);
                thought.ThinkAction = RandomHelper.Random(0, 5);
            }

            Direction direction = movement.GetLookDirection();
            switch (thought.ThinkAction)
            {
                case 0:
                    if (movement.IsMoving())
                    {
                        // slow down
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = false;
                        intent.MoveRight = false;
                        intent.MoveUp = false;
                        intent.MoveDown = false;
                        movement.Owner.AddComponent(intent);
                    }
                    break;
                case 1:
                    {
                        // rotate direction to the left
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = direction == Direction.Up;
                        intent.MoveRight = direction == Direction.Down;
                        intent.MoveUp = direction == Direction.Right;
                        intent.MoveDown = direction == Direction.Left;
                        movement.Owner.AddComponent(intent);
                    }
                    thought.ThinkAction = 0;
                    break;
                case 2:
                    {
                        // rotate direction to the right
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = direction == Direction.Down;
                        intent.MoveRight = direction == Direction.Up;
                        intent.MoveUp = direction == Direction.Left;
                        intent.MoveDown = direction == Direction.Right;
                        movement.Owner.AddComponent(intent);
                    }
                    thought.ThinkAction = 0;
                    break;
                case 3:
                    {
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = direction == Direction.Left;
                        intent.MoveRight = direction == Direction.Right;
                        intent.MoveUp = direction == Direction.Up;
                        intent.MoveDown = direction == Direction.Down;
                        movement.Owner.AddComponent(intent);
                    }
                    break;
                default:
                    if (movement.IsMoving())
                    {
                        // slow down
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = false;
                        intent.MoveRight = false;
                        intent.MoveUp = false;
                        intent.MoveDown = false;
                        movement.Owner.AddComponent(intent);
                    }
                    break;
            }
        }
    }
}
