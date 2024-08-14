using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Body : Grecs.Component, ISerializable, IHydrateable
    {
        private string _bodyId = "";
        public string BodyId
        {
            get => _bodyId; set
            {
                if (!_bodyId?.Equals(value) ?? (value != null))
                {
                    _bodyId = value;
                    
                    TriggerChange();
                }
            }
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(BodyId);
        }

        public void Read(System.IO.BinaryReader r)
        {
            BodyId = r.ReadString();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("bodyId"))
            {
                BodyId = values["bodyId"].Trim('"');
            }
        }
    }
}
