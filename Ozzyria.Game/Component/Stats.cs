namespace Ozzyria.Game.Component
{
    public class Stats : Component
    {
        public override ComponentType Type() => ComponentType.Stats;
        public int Experience { get; set; } = 0;
        public int MaxExperience { get; set; } = 100;
        public int Health { get; set; } = 100;
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
