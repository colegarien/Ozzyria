using Ozzyria.Game.Utility;
using System;
using System.Linq;

namespace Ozzyria.Game.Component
{
    public class SlimeThought : Thought
    {
        const float MAX_FOLLOW_DISTANCE = 200;

        public Delay ThinkDelay { get; set; } = new Delay();
        public int ThinkAction { get; set; } = 0;

        public override void Update(float deltaTime, EntityManager entityManager)
        {
            var movement = Owner.GetComponent<Movement>(ComponentType.Movement);
            var combat = Owner.GetComponent<Combat>(ComponentType.Combat);

            var closestPlayer = entityManager.GetEntities()
                .Where(e => e.Id != Owner.Id && e.HasComponent(ComponentType.Movement) && e.HasComponent(ComponentType.Stats) && e.HasComponent(ComponentType.Thought) && e.GetComponent<Thought>(ComponentType.Thought) is PlayerThought)
                .OrderBy(p => movement.DistanceTo(p.GetComponent<Movement>(ComponentType.Movement)))
                .FirstOrDefault();
            if (closestPlayer == null)
            {
                Think(deltaTime, movement);
                combat.Update(deltaTime, false);
                movement.Update(deltaTime);
                return;
            }

            var playerMovement = closestPlayer.GetComponent<Movement>(ComponentType.Movement);

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
