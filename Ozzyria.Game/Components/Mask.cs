using Ozzyria.Game.Components.Attribute;
using Grecs;

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
                    TriggerChange();
                }
            }
        }
    }
}
