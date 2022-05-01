using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Stats : Component
    {
        private int _experience = 0;
        private int _maxExperience = 100;
        private int _health = 100;
        private int _maxHealth = 100;

        [Savable]
        public int Experience { get => _experience; set
            {
                if (_experience != value)
                {
                    _experience = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public int MaxExperience { get => _maxExperience; set
            {
                if (_maxExperience != value)
                {
                    _maxExperience = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public int Health { get => _health; set
            {
                if (_health != value)
                {
                    _health = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public int MaxHealth { get => _maxHealth; set
            {
                if (_maxHealth != value)
                {
                    _maxHealth = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
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
            MaxExperience += (int)System.Math.Sqrt(MaxExperience);

            if (Experience < 0)
            {
                Experience = 0;
            }
            else if (Experience >= MaxExperience)
            {
                LevelUp();
            }
        }
    }
}
