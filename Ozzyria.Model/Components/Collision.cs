using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Collision : Grecs.Component, ISerializable, IHydrateable
    {
        private bool _isDynamic = true;
        public bool IsDynamic
        {
            get => _isDynamic; set
            {
                if (!_isDynamic.Equals(value))
                {
                    _isDynamic = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "collision";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(IsDynamic);
        }

        public void Read(System.IO.BinaryReader r)
        {
            IsDynamic = r.ReadBoolean();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("isDynamic"))
            {
                IsDynamic = bool.Parse(values["isDynamic"]);
            }
        }
    }
}
