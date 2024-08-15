namespace Ozzyria.Model.Types
{
    public class BoundingCircle : ISerializable, IHydrateable
    {
        private System.Action? _triggerChange;
        public System.Action? TriggerChange { get => _triggerChange; set
            {
                _triggerChange = value;
                
            }
        }
        
        
        private float _radius = 0f;
        public float Radius
        {
            get => _radius; set
            {
                if (!_radius.Equals(value))
                {
                    _radius = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "BoundingCircle";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Radius);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Radius = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("radius"))
            {
                Radius = float.Parse(values["radius"]);
            }
        }
    }
}
