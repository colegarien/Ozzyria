using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Mask : Grecs.Component, ISerializable, IHydrateable
    {
        private string _maskId = "";
        public string MaskId
        {
            get => _maskId; set
            {
                if (!_maskId?.Equals(value) ?? (value != null))
                {
                    _maskId = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "mask";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(MaskId);
        }

        public void Read(System.IO.BinaryReader r)
        {
            MaskId = r.ReadString();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("maskId"))
            {
                MaskId = values["maskId"].Trim('"');
            }
        }
    }
}
