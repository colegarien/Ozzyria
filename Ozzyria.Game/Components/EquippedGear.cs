using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class EquippedGear : Component
    {
        private string _body = "";


        [Savable]
        public string Body
        {
            get => _body; set
            {
                if (_body != value)
                {
                    _body = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
