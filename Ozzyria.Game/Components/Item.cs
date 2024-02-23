using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public class Item : Component
    {
        private string _name = "";
        private string _icon = "";
        private int _slot = 0;

        private string _itemId = "";
        private string _equipmentSlot = "";
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
        public int Slot
        {
            get => _slot; set
            {
                if (_slot != value)
                {
                    _slot = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public string ItemId
        {
            get => _itemId; set
            {
                if (_itemId != value)
                {
                    _itemId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public string EquipmentSlot
        {
            get => _equipmentSlot; set
            {
                if (_equipmentSlot != value)
                {
                    _equipmentSlot = value;
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
