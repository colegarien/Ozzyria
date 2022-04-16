using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "ExperienceBoost")]
    public class ExperienceBoost : Component
    {
        [Savable]
        public int Experience { get; set; } = 10;
        [Savable]
        public bool HasBeenAbsorbed { get; set; } = false;

        public void AbsorbInto(Stats stats)
        {
            if (HasBeenAbsorbed)
            {
                return;
            }

            stats.AddExperience(Experience);
            HasBeenAbsorbed = true;
        }
    }
}
