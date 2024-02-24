using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public class Body : Component
    {
        private string _bodyId;

        [Savable]
        public string BodyId
        {
            get => _bodyId; set
            {
                if (_bodyId != value)
                {
                    _bodyId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
