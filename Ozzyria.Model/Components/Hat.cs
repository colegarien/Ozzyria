using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Hat : Grecs.Component, ISerializable, IHydrateable
    {
        private string _hatId = "";
        public string HatId
        {
            get => _hatId; set
            {
                if (!_hatId?.Equals(value) ?? (value != null))
                {
                    _hatId = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "Hat";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(HatId);
        }

        public void Read(System.IO.BinaryReader r)
        {
            HatId = r.ReadString();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("hatId"))
            {
                HatId = values["hatId"].Trim('"');
            }
        }
    }
}
