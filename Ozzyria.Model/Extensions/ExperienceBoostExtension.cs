using Ozzyria.Model.Components;

namespace Ozzyria.Model.Extensions
{
    public static class ExperienceBoostExtension
    {
        public static void AbsorbInto(this ExperienceBoost boost, Stats stats)
        {
            if (boost.HasBeenAbsorbed)
            {
                return;
            }

            stats.AddExperience(boost.Experience);
            boost.HasBeenAbsorbed = true;
        }
    }
}
