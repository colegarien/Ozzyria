using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{
    [Options(Name = "ExperienceBoost")]
    public class ExperienceBoost : Component
    {
        public override ComponentType Type() => ComponentType.ExperienceBoost;
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
