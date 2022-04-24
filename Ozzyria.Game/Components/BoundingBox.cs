using Ozzyria.Game.Components.Attribute;

namespace Ozzyria.Game.Components
{
    [Options(Name = "BoundingBox")]
    public class BoundingBox : Collision
    {
        private int _width = 10;
        private int _height = 10;

        [Savable]
        public int Width { get => _width; set
            {
                if (_width != value)
                {
                    _width = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public int Height { get => _height; set
            {
                if (_height != value)
                {
                    _height = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        public float GetLeft()
        {
            if (Owner.HasComponent(typeof(Movement)))
            {
                return ((Movement)Owner.GetComponent(typeof(Movement))).X - (Width / 2f);
            }

            return 0;
        }

        public float GetRight()
        {
            if (Owner.HasComponent(typeof(Movement)))
            {
                return ((Movement)Owner.GetComponent(typeof(Movement))).X + (Width / 2f);
            }

            return Width;
        }

        public float GetTop()
        {
            if (Owner.HasComponent(typeof(Movement)))
            {
                return ((Movement)Owner.GetComponent(typeof(Movement))).Y - (Height / 2f);
            }

            return 0;
        }

        public float GetBottom()
        {
            if (Owner.HasComponent(typeof(Movement)))
            {
                return ((Movement)Owner.GetComponent(typeof(Movement))).Y + (Height / 2f);
            }

            return Height;
        }
    }
}
