using Ozzyria.Game.Components.Attribute;

namespace Ozzyria.Game.Components
{
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
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public int Height { get => _height; set
            {
                if (_height != value)
                {
                    _height = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public float GetLeft()
        {
            return X - (Width / 2f);
        }

        public float GetRight()
        {
            return X + (Width / 2f);
        }

        public float GetTop()
        {
            return Y - (Height / 2f);
        }

        public float GetBottom()
        {
            return Y + (Height / 2f);
        }
    }
}
