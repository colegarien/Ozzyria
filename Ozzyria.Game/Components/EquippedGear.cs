using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class EquippedGear : Component
    {

        private string _body = "";
        private string _hat = ""; // cowboy_hat || green_hat


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
        [Savable]
        public string Hat
        {
            get => _hat; set
            {
                if (_hat != value)
                {
                    _hat = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
