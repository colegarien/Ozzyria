using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public enum BodyType
    {
        Human,
        Slime,
    }

    public class Body : Component
    {
        private BodyType _bodyType;

        [Savable]
        public BodyType BodyType
        {
            get => _bodyType; set
            {
                if (_bodyType != value)
                {
                    _bodyType = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
