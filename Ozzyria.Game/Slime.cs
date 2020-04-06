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
            var closestPlayer = players.OrderBy(p => Math.Pow(p.Movement.X - Movement.X, 2) + Math.Pow(p.Movement.Y - Movement.Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                Movement.SlowDown(deltaTime);
                HandleAttackTimer(deltaTime, false);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.Movement.X - Movement.X, 2) + Math.Pow(closestPlayer.Movement.Y - Movement.Y, 2));
            if (distance > MAX_FOLLOW_DISTANCE)
            {
                Movement.SlowDown(deltaTime);
                HandleAttackTimer(deltaTime, false);
                return;
            }

            var attack = false;
            if(distance <= AttackRange)
            {
                Movement.SlowDown(deltaTime);
                attack = true;
            }
            else
            {
                Movement.SpeedUp(deltaTime);
            }
            HandleAttackTimer(deltaTime, attack);

            Movement.TurnToward(deltaTime, closestPlayer.Movement.X, closestPlayer.Movement.Y);
            Movement.Update(deltaTime);
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

    }
}
