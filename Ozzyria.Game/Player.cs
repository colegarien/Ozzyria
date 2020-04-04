using Ozzyria.Game.Utility;
using System;

namespace Ozzyria.Game
{
    public class Player
    {
        const float ACCELERATION = 200f;
        const float MAX_SPEED = 100f;
        const float TURN_SPEED = 5f;

        public int Id { get; set; } = -1;
        #region movement
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;
        public float MoveDirection { get; set; } = 0f;
        public float LookDirection { get; set; } = 0f;
        #endregion
        #region stats
        public int Experience { get; set; } = 0;
        public int MaxExperience { get; set; } = 100;
        public int Health { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        #endregion
        #region combat
        public float AttackDelay { get; set; } = 0.5f;
        public float AttackTimer { get; set; } = 0f;
        public bool Attacking { get; set; } = false;
        public float AttackAngle { get; set; } = 0.78f; // forty-five degrees-ish
        public float AttackRange { get; set; } = 20f;
        public int AttackDamage { get; set; } = 5;
        #endregion

        public void Update(float deltaTime, Input input)
        {
            HandleInput(deltaTime, input);

            X += Speed * deltaTime * (float)Math.Sin(LookDirection + MoveDirection);
            Y += Speed * deltaTime * (float)Math.Cos(LookDirection + MoveDirection);
        }

        public void AddExperience(int experience)
        {
            Experience += experience;
            if (Experience >= MaxExperience)
            {
                LevelUp();
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

        private void LevelUp()
        {
            Experience -= MaxExperience;
            MaxExperience += (int)Math.Sqrt(MaxExperience);

            if(Experience < 0)
            {
                Experience = 0;
            }
            else if(Experience >= MaxExperience)
            {
                LevelUp();
            }
        }

        private void HandleInput(float deltaTime, Input input)
        {
            if (input.TurnLeft)
            {
                LookDirection = AngleHelper.Clamp(LookDirection + (TURN_SPEED * deltaTime));
            }
            if (input.TurnRight)
            {
                LookDirection = AngleHelper.Clamp(LookDirection - (TURN_SPEED * deltaTime));
            }

            if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
            {
                Speed += ACCELERATION * deltaTime;
                if (Speed > MAX_SPEED)
                {
                    Speed = MAX_SPEED;
                }
            }
            if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
            {
                if (Speed > 0)
                {
                    Speed -= ACCELERATION * deltaTime;
                    if (Speed < 0.0f)
                    {
                        Speed = 0.0f;
                    }
                }
                else if (Speed < 0)
                {
                    Speed += ACCELERATION * deltaTime;
                    if (Speed > 0.0f)
                    {
                        Speed = 0.0f;
                    }
                }
            }

            if (input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
            {
                MoveDirection = 0;
            }
            else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
            {
                MoveDirection = AngleHelper.Pi;
            }
            else if (!input.MoveUp && !input.MoveDown)
            {
                var sideways = AngleHelper.PiOverTwo;
                if (input.MoveRight)
                    MoveDirection = -sideways;
                else if (input.MoveLeft)
                    MoveDirection = sideways;
            }
            else if (input.MoveUp && !input.MoveDown)
            {
                var forwardFortyFive = AngleHelper.PiOverFour;
                if (input.MoveRight)
                    MoveDirection = -forwardFortyFive;
                else if (input.MoveLeft)
                    MoveDirection = forwardFortyFive;
            }
            else if (input.MoveDown && !input.MoveUp)
            {
                var backwardFortyFive = AngleHelper.ThreePiOverFour;
                if (input.MoveRight)
                    MoveDirection = -backwardFortyFive;
                else if (input.MoveLeft)
                    MoveDirection = backwardFortyFive;
            }

            HandleAttackTimer(deltaTime, input);
        }

        private void HandleAttackTimer(float deltaTime, Input input)
        {
            if (AttackTimer < AttackDelay)
            {
                // recharge attack timer
                AttackTimer += deltaTime;
            }

            if (AttackTimer >= AttackDelay && input.Attack)
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
    }
}
