using Ozzyria.Model.Components;

namespace Ozzyria.Model.Extensions
{
    public static class StatsExtension
    {
        public static void AddExperience(this Stats stats, int experience)
        {
            stats.Experience += experience;
            if (stats.Experience >= stats.MaxExperience)
            {
                stats.LevelUp();
            }
        }

        public static void Damage(this Stats stats, int damage)
        {
            stats.Health -= damage;

            if (stats.IsDead())
            {
                stats.Health = 0;
            }
        }

        public static bool IsDead(this Stats stats)
        {
            return stats.Health <= 0;
        }

        private static void LevelUp(this Stats stats)
        {
            stats.Experience -= stats.MaxExperience;
            stats.MaxExperience += (int)System.Math.Sqrt(stats.MaxExperience);

            if (stats.Experience < 0)
            {
                stats.Experience = 0;
            }
            else if (stats.Experience >= stats.MaxExperience)
            {
                stats.LevelUp();
            }
        }
    }
}
