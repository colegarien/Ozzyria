namespace Ozzyria.Model.Types
{
    public class BoundingBox : ISerializable, IHydrateable
    {
        private System.Action? _triggerChange;
        public System.Action? TriggerChange { get => _triggerChange; set
            {
                _triggerChange = value;
                
            }
        }
        
        
        private int _width = 0;
        public int Width
        {
            get => _width; set
            {
                if (!_width.Equals(value))
                {
                    _width = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private int _height = 0;
        public int Height
        {
            get => _height; set
            {
                if (!_height.Equals(value))
                {
                    _height = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "BoundingBox";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Width);
            w.Write(Height);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Width = r.ReadInt32();
            Height = r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("width"))
            {
                Width = int.Parse(values["width"]);
            }
            if (values.HasValueFor("height"))
            {
                Height = int.Parse(values["height"]);
            }
        }
    }
}
