using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

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
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool HasBeenAbsorbed { get => _hasBeenAbsorbed; set
            {
                if (_hasBeenAbsorbed != value)
                {
                    _hasBeenAbsorbed = value;
                    Owner?.TriggerComponentChanged(this);
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
