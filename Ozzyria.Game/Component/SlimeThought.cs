using Ozzyria.Game.Component.Attribute;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Utility;
using System;
using System.Linq;

namespace Ozzyria.Game.Component
{
    [Options(Name = "SlimeThought")]
    public class SlimeThought : Thought
    {
        const float MAX_FOLLOW_DISTANCE = 200;

        public Delay ThinkDelay { get; set; } = new Delay();
        public int ThinkAction { get; set; } = 0;

        public override void Update(float deltaTime, EntityContext context)
        {
            var movement = (Movement)Owner.GetComponent(typeof(Movement));
            var combat = (Combat)Owner.GetComponent(typeof(Combat));

            var closestPlayer = context.GetEntities()
                .Where(e => e.id != Owner.id && e.HasComponent(typeof(Movement)) && e.HasComponent(typeof(Stats)) && e.HasComponent(typeof(PlayerThought)))
                .OrderBy(p => movement.DistanceTo((Movement)p.GetComponent(typeof(Movement))))
                .FirstOrDefault();
            if (closestPlayer == null)
            {
                Think(deltaTime, movement);
                combat.Update(deltaTime, false);
                movement.Update(deltaTime);
                return;
            }

            var playerMovement = (Movement)closestPlayer.GetComponent(typeof(Movement));

            var distance = movement.DistanceTo(playerMovement);
            if (distance > MAX_FOLLOW_DISTANCE)
            {
                Think(deltaTime, movement);
                combat.Update(deltaTime, false);
                movement.Update(deltaTime);
                return;
            }

            var attack = false;
            if (distance <= combat.AttackRange)
            {
                movement.SlowDown(deltaTime);
                attack = true;
            }
            else
            {
                movement.SpeedUp(deltaTime);
            }
            movement.TurnToward(deltaTime, playerMovement.X, playerMovement.Y);

            combat.Update(deltaTime, attack);
            movement.Update(deltaTime);
        }

        private void Think(float deltaTime, Movement movement)
        {
            ThinkDelay.Update(deltaTime);
            if (ThinkDelay.IsReady())
            {
                ThinkDelay.DelayInSeconds = RandomHelper.Random(0f, 1.5f);
                ThinkAction = RandomHelper.Random(0, 5);
            }

            switch (ThinkAction)
            {
                case 0:
                    movement.SlowDown(deltaTime);
                    break;
                case 1:
                    movement.TurnLeft(deltaTime);
                    break;
                case 2:
                    movement.TurnRight(deltaTime);
                    break;
                case 3:
                    movement.SpeedUp(deltaTime);
                    break;
                default:
                    movement.SlowDown(deltaTime);
                    break;
            }
        }
    }
}
