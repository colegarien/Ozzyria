using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Armor : Grecs.Component, ISerializable, IHydrateable
    {
        private string _armorId = "";
        public string ArmorId
        {
            get => _armorId; set
            {
                if (!_armorId?.Equals(value) ?? (value != null))
                {
                    _armorId = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "Armor";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(ArmorId);
        }

        public void Read(System.IO.BinaryReader r)
        {
            ArmorId = r.ReadString();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("armorId"))
            {
                ArmorId = values["armorId"].Trim('"');
            }
        }
    }
}
