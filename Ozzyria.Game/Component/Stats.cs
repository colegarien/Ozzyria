using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{
    [Options(Name = "Stats")]
    public class Stats : Component
    {
        [Savable]
        public int Experience { get; set; } = 0;
        [Savable]
        public int MaxExperience { get; set; } = 100;
        [Savable]
        public int Health { get; set; } = 100;
        [Savable]
        public int MaxHealth { get; set; } = 100;


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
