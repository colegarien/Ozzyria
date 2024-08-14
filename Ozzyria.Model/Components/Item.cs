using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Item : Grecs.Component, ISerializable, IHydrateable
    {
        private string _name = "";
        public string Name
        {
            get => _name; set
            {
                if (!_name.Equals(value))
                {
                    _name = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private string _icon = "";
        public string Icon
        {
            get => _icon; set
            {
                if (!_icon.Equals(value))
                {
                    _icon = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _slot = 0;
        public int Slot
        {
            get => _slot; set
            {
                if (!_slot.Equals(value))
                {
                    _slot = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private string _itemId = "";
        public string ItemId
        {
            get => _itemId; set
            {
                if (!_itemId.Equals(value))
                {
                    _itemId = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private string _equipmentSlot = "";
        public string EquipmentSlot
        {
            get => _equipmentSlot; set
            {
                if (!_equipmentSlot.Equals(value))
                {
                    _equipmentSlot = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private bool _isEquipped = false;
        public bool IsEquipped
        {
            get => _isEquipped; set
            {
                if (!_isEquipped.Equals(value))
                {
                    _isEquipped = value;
                    
                    TriggerChange();
                }
            }
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Name);
            w.Write(Icon);
            w.Write(Slot);
            w.Write(ItemId);
            w.Write(EquipmentSlot);
            w.Write(IsEquipped);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Name = r.ReadString();
            Icon = r.ReadString();
            Slot = r.ReadInt32();
            ItemId = r.ReadString();
            EquipmentSlot = r.ReadString();
            IsEquipped = r.ReadBoolean();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("name"))
            {
                Name = values["name"].Trim('"');
            }
            if (values.HasValueFor("icon"))
            {
                Icon = values["icon"].Trim('"');
            }
            if (values.HasValueFor("slot"))
            {
                Slot = int.Parse(values["slot"]);
            }
            if (values.HasValueFor("itemId"))
            {
                ItemId = values["itemId"].Trim('"');
            }
            if (values.HasValueFor("equipmentSlot"))
            {
                EquipmentSlot = values["equipmentSlot"].Trim('"');
            }
            if (values.HasValueFor("isEquipped"))
            {
                IsEquipped = bool.Parse(values["isEquipped"]);
            }
        }
    }
}
