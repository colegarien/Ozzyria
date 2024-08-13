using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public class ExperienceBoost : Component
    {
        private int _experience = 10;
        private bool _hasBeenAbsorbed = false;

        [Savable]
        public int Experience { get => _experience; set
            {
                if (_experience != value)
                {
                    _experience = value;
                    TriggerChange();
                }
            }
        }
        [Savable]
        public bool HasBeenAbsorbed { get => _hasBeenAbsorbed; set
            {
                if (_hasBeenAbsorbed != value)
                {
                    _hasBeenAbsorbed = value;
                    TriggerChange();
                }
            }
        }
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
