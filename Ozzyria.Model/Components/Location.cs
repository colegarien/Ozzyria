using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Location : Grecs.Component, ISerializable, IHydrateable
    {
        private string _area = "";
        public string Area
        {
            get => _area; set
            {
                if (!_area?.Equals(value) ?? (value != null))
                {
                    _area = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "location";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Area);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Area = r.ReadString();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("area"))
            {
                Area = values["area"].Trim('"');
            }
        }
    }
}
