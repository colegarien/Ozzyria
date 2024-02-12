using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Hat: Component
    {
        private string _hatId;

        [Savable]
        public string HatId
        {
            get => _hatId; set
            {
                if (_hatId != value)
                {
                    _hatId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

    }
}
