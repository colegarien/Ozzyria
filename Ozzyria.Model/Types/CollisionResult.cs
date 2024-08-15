namespace Ozzyria.Model.Types
{
    public class CollisionResult : ISerializable, IHydrateable
    {
        private System.Action? _triggerChange;
        public System.Action? TriggerChange { get => _triggerChange; set
            {
                _triggerChange = value;
                
            }
        }
        
        
        private bool _collided = false;
        public bool Collided
        {
            get => _collided; set
            {
                if (!_collided.Equals(value))
                {
                    _collided = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private float _depth = 0f;
        public float Depth
        {
            get => _depth; set
            {
                if (!_depth.Equals(value))
                {
                    _depth = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private float _normalX = 0f;
        public float NormalX
        {
            get => _normalX; set
            {
                if (!_normalX.Equals(value))
                {
                    _normalX = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private float _normalY = 0f;
        public float NormalY
        {
            get => _normalY; set
            {
                if (!_normalY.Equals(value))
                {
                    _normalY = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "CollisionResult";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Collided);
            w.Write(Depth);
            w.Write(NormalX);
            w.Write(NormalY);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Collided = r.ReadBoolean();
            Depth = r.ReadSingle();
            NormalX = r.ReadSingle();
            NormalY = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("collided"))
            {
                Collided = bool.Parse(values["collided"]);
            }
            if (values.HasValueFor("depth"))
            {
                Depth = float.Parse(values["depth"]);
            }
            if (values.HasValueFor("normalX"))
            {
                NormalX = float.Parse(values["normalX"]);
            }
            if (values.HasValueFor("normalY"))
            {
                NormalY = float.Parse(values["normalY"]);
            }
        }
    }
}
