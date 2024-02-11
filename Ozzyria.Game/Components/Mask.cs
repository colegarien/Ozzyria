using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Mask : Component
    {
        private string _maskId;

        [Savable]
        public string MaskId
        {
            get => _maskId; set
            {
                if (_maskId != value)
                {
                    _maskId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
