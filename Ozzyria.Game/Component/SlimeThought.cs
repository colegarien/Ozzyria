using Ozzyria.Game.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game.Component
{
    public class SlimeThought : IThought
    {
        const float MAX_FOLLOW_DISTANCE = 200;

        public Delay ThinkDelay { get; set; } = new Delay();
        public int ThinkAction { get; set; } = 0;

        public override void Update(float deltaTime, Player[] players, Dictionary<int, Entity> entities)
        {
            var movement = (Movement)Owner.Components[typeof(Movement)];
            var combat = (Combat)Owner.Components[typeof(Combat)];

            var closestPlayer = players.OrderBy(p => Math.Pow(p.Movement.X - movement.X, 2) + Math.Pow(p.Movement.Y - movement.Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                Think(deltaTime, movement);
                combat.Update(deltaTime, false);
                movement.Update(deltaTime);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.Movement.X - movement.X, 2) + Math.Pow(closestPlayer.Movement.Y - movement.Y, 2));
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
            movement.TurnToward(deltaTime, closestPlayer.Movement.X, closestPlayer.Movement.Y);

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
