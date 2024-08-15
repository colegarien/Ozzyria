namespace Ozzyria.Model.Types
{
    public class CollisionShape : ISerializable, IHydrateable
    {
        private System.Action? _triggerChange;
        public System.Action? TriggerChange { get => _triggerChange; set
            {
                _triggerChange = value;
                BoundingBox.TriggerChange = TriggerChange;
                BoundingCircle.TriggerChange = TriggerChange;
            }
        }
        
        
        private BoundingBox _boundingBox = new BoundingBox{  };
        public BoundingBox BoundingBox
        {
            get => _boundingBox; set
            {
                if (!_boundingBox?.Equals(value) ?? (value != null))
                {
                    _boundingBox = value;
                    if (value != null) { _boundingBox.TriggerChange = TriggerChange; }
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private BoundingCircle _boundingCircle = new BoundingCircle{  };
        public BoundingCircle BoundingCircle
        {
            get => _boundingCircle; set
            {
                if (!_boundingCircle?.Equals(value) ?? (value != null))
                {
                    _boundingCircle = value;
                    if (value != null) { _boundingCircle.TriggerChange = TriggerChange; }
                    TriggerChange?.Invoke();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "collision_shape";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(BoundingBox.Width);
            w.Write(BoundingBox.Height);
            w.Write(BoundingCircle.Radius);
        }

        public void Read(System.IO.BinaryReader r)
        {
            BoundingBox.Width = r.ReadInt32();
            BoundingBox.Height = r.ReadInt32();
            BoundingCircle.Radius = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("boundingBox"))
            {
                var  values_boundingBox = values.Extract("boundingBox");
                if (values_boundingBox.HasValueFor("width"))
            {
                BoundingBox.Width = int.Parse(values_boundingBox["width"]);
            }
                if (values_boundingBox.HasValueFor("height"))
            {
                BoundingBox.Height = int.Parse(values_boundingBox["height"]);
            }
            }
            if (values.HasValueFor("boundingCircle"))
            {
                var  values_boundingCircle = values.Extract("boundingCircle");
                if (values_boundingCircle.HasValueFor("radius"))
            {
                BoundingCircle.Radius = float.Parse(values_boundingCircle["radius"].Trim('f'));
            }
            }
        }
    }
}
