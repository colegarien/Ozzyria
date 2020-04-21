namespace Ozzyria.Game.Component
{
    public class BoundingBox : Collision
    {
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        public float GetLeft()
        {
            if (Owner.HasComponent(ComponentType.Movement))
            {
                return Owner.GetComponent<Movement>(ComponentType.Movement).X - (Width / 2f);
            }

            return 0;
        }

        public float GetRight()
        {
            if (Owner.HasComponent(ComponentType.Movement))
            {
                return Owner.GetComponent<Movement>(ComponentType.Movement).X + (Width / 2f);
            }

            return Width;
        }

        public float GetTop()
        {
            if (Owner.HasComponent(ComponentType.Movement))
            {
                return Owner.GetComponent<Movement>(ComponentType.Movement).Y - (Height / 2f);
            }

            return 0;
        }

        public float GetBottom()
        {
            if (Owner.HasComponent(ComponentType.Movement))
            {
                return Owner.GetComponent<Movement>(ComponentType.Movement).Y + (Height / 2f);
            }

            return Height;
        }
    }
}
