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
            if (OwnerMovement != null)
            {
                return OwnerMovement.X - (Width / 2f);
            }

            return 0;
        }

        public float GetRight()
        {
            if (OwnerMovement != null)
            {
                return OwnerMovement.X + (Width / 2f);
            }

            return Width;
        }

        public float GetTop()
        {
            if (OwnerMovement != null)
            {
                return OwnerMovement.Y - (Height / 2f);
            }

            return 0;
        }

        public float GetBottom()
        {
            if (OwnerMovement != null)
            {
                return OwnerMovement.Y + (Height / 2f);
            }

            return Height;
        }
    }
}
