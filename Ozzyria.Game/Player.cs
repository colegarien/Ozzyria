using Ozzyria.Game.Component;
using System;

namespace Ozzyria.Game
{
    public class Player
    {
        public int Id { get; set; } = -1;
        public Movement Movement { get; set; } = new Movement();

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
            Movement.Update(deltaTime);
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
                Movement.TurnLeft(deltaTime);
            }
            if (input.TurnRight)
            {
                Movement.TurnRight(deltaTime);
            }

            if (input.MoveUp || input.MoveDown || input.MoveLeft || input.MoveRight)
            {
                Movement.SpeedUp(deltaTime);
            }
            if (!input.MoveDown && !input.MoveUp && !input.MoveRight && !input.MoveLeft)
            {
                Movement.SlowDown(deltaTime);
            }

            if (input.MoveUp && !input.MoveLeft && !input.MoveRight && !input.MoveDown)
            {
                Movement.MoveForward(deltaTime);
            }
            else if (input.MoveDown && !input.MoveLeft && !input.MoveRight && !input.MoveUp)
            {
                Movement.MoveBackward(deltaTime);
            }
            else if (!input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    Movement.MoveRight(deltaTime);
                else if (input.MoveLeft)
                    Movement.MoveLeft(deltaTime);
            }
            else if (input.MoveUp && !input.MoveDown)
            {
                if (input.MoveRight)
                    Movement.MoveForwardRight(deltaTime);
                else if (input.MoveLeft)
                    Movement.MoveForwardLeft(deltaTime);
            }
            else if (input.MoveDown && !input.MoveUp)
            {
                if (input.MoveRight)
                    Movement.MoveBackwardRight(deltaTime);
                else if (input.MoveLeft)
                    Movement.MoveBackwardLeft(deltaTime);
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
