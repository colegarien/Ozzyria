using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Item : Component
    {
        private string _name = "";
        private string _icon = "";

        private bool _isEquipped = false;


        [Savable]
        public string Name
        {
            get => _name; set
            {
                if (_name != value)
                {
                    _name = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public string Icon
        {
            get => _icon; set
            {
                if (_icon != value)
                {
                    _icon = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public bool IsEquipped
        {
            get => _isEquipped; set
            {
                if (_isEquipped != value)
                {
                    _isEquipped = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
