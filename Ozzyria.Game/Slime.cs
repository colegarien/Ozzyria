using Ozzyria.Game.Utility;
using System;
using System.Linq;

namespace Ozzyria.Game
{
    public class Slime
    {
        const float ACCELERATION = 300f;
        const float MAX_SPEED = 50f;

        const float MAX_FOLLOW_DISTANCE = 200;


        #region movement
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;
        public float MoveDirection { get; set; } = 0f;
        public float LookDirection { get; set; } = 0f;
        #endregion
        #region stats
        public int Health { get; set; } = 30;
        public int MaxHealth { get; set; } = 30;
        #endregion
        #region combat
        public float AttackDelay { get; set; } = 0.5f;
        public float AttackTimer { get; set; } = 0f;
        public bool Attacking { get; set; } = false;
        public float AttackAngle { get; set; } = 0.78f; // forty-five degrees-ish
        public float AttackRange { get; set; } = 20f;
        public int AttackDamage { get; set; } = 5;
        #endregion

        public void Update(float deltaTime, Player[] players)
        {
            var closestPlayer = players.OrderBy(p => Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                SlowDown(deltaTime);
                HandleAttackTimer(deltaTime, false);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.X - X, 2) + Math.Pow(closestPlayer.Y - Y, 2));
            if (distance > MAX_FOLLOW_DISTANCE)
            {
                SlowDown(deltaTime);
                HandleAttackTimer(deltaTime, false);
                return;
            }

            var attack = false;
            if(distance <= AttackRange)
            {
                SlowDown(deltaTime);
                attack = true;
            }
            else
            {
                SpeedUp(deltaTime);
            }
            HandleAttackTimer(deltaTime, attack);

            // Move toward player
            LookDirection = AngleHelper.AngleTo(X, Y, closestPlayer.X, closestPlayer.Y);
            X += Speed * deltaTime * (float)((closestPlayer.X - X) / distance);
            Y += Speed * deltaTime * (float)((closestPlayer.Y - Y) / distance);
        }

        private void HandleAttackTimer(float deltaTime, bool attack)
        {
            if (AttackTimer < AttackDelay)
            {
                // recharge attack timer
                AttackTimer += deltaTime;
            }

            if (AttackTimer >= AttackDelay && attack)
            {
                // has been long enough since last attack
                AttackTimer -= AttackDelay;
                Attacking = true;
            }
            else
            {
                // waiting to attack again, or not currently attacking
                Attacking = false;
            }
        }

        public void Damage(int damage)
        {
            Health -= damage;

            if (IsDead())
            {
                Health = 0;
            }
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

        private void SlowDown(float deltaTime)
        {
            if (Speed == 0)
            {
                return;
            }

            Speed -= ACCELERATION * deltaTime;
            if (Speed < 0.0f)
            {
                Speed = 0.0f;
            }
        }

        private void SpeedUp(float deltaTime)
        {
            if (Speed == MAX_SPEED)
            {
                return;
            }

            Speed += ACCELERATION * deltaTime;
            if (Speed > MAX_SPEED)
            {
                Speed = MAX_SPEED;
            }
        }



    }
}
