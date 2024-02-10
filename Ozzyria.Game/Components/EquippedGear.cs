using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class EquippedGear : Component
    {

        private string _body = "";
        private string _hat = ""; // cowboy_hat || green_hat
        private string _armor = ""; // cyan_armor || biker_jacket
        private string _mask = ""; // shades


        [Savable]
        public string Body
        {
            get => _body; set
            {
                if (_body != value)
                {
                    _body = value;
                    Owner?.TriggerComponentChanged(this);
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
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public string Armor
        {
            get => _armor; set
            {
                if (_armor != value)
                {
                    _armor = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public string Mask
        {
            get => _mask; set
            {
                if (_mask != value)
                {
                    _mask = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
