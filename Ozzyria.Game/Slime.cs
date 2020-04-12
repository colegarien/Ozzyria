using Ozzyria.Game.Component;
using Ozzyria.Game.Utility;
using System;
using System.Linq;

namespace Ozzyria.Game
{
    public class Slime
    {
        const float MAX_FOLLOW_DISTANCE = 200;

        public Movement Movement { get; set; } = new Movement { MAX_SPEED = 50f, ACCELERATION = 300f };
        public Stats Stats { get; set; } = new Stats { Health = 30, MaxHealth = 30 };
        public Combat Combat { get; set; } = new Combat();

        #region ai
        public Delay ThinkDelay { get; set; } = new Delay();
        public int ThinkAction { get; set; } = 0;
        #endregion

        public void Update(float deltaTime, Player[] players)
        {
            var closestPlayer = players.OrderBy(p => Math.Pow(p.Movement.X - Movement.X, 2) + Math.Pow(p.Movement.Y - Movement.Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                Think(deltaTime);
                Combat.Update(deltaTime, false);
                Movement.Update(deltaTime);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.Movement.X - Movement.X, 2) + Math.Pow(closestPlayer.Movement.Y - Movement.Y, 2));
            if (distance > MAX_FOLLOW_DISTANCE)
            {
                Think(deltaTime);
                Combat.Update(deltaTime, false);
                Movement.Update(deltaTime);
                return;
            }

            var attack = false;
            if(distance <= Combat.AttackRange)
            {
                Movement.SlowDown(deltaTime);
                attack = true;
            }
            else
            {
                Movement.SpeedUp(deltaTime);
            }
            Movement.TurnToward(deltaTime, closestPlayer.Movement.X, closestPlayer.Movement.Y);

            Combat.Update(deltaTime, attack);
            Movement.Update(deltaTime);
        }

        private void Think(float deltaTime)
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
                    Movement.SlowDown(deltaTime);
                    break;
                case 1:
                    Movement.TurnLeft(deltaTime);
                    break;
                case 2:
                    Movement.TurnRight(deltaTime);
                    break;
                case 3:
                    Movement.SpeedUp(deltaTime);
                    break;
                default:
                    Movement.SlowDown(deltaTime);
                    break;
            }
        }
    }
}
