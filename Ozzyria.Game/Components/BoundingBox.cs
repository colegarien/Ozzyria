using Ozzyria.Game.Components.Attribute;

namespace Ozzyria.Game.Components
{
    [Options(Name = "BoundingBox")]
    public class BoundingBox : Collision
    {
        [Savable]
        public int Width { get; set; } = 10;
        [Savable]
        public int Height { get; set; } = 10;

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
