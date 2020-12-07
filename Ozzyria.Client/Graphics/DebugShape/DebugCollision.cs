using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.System;

namespace Ozzyria.Client.Graphics.DebugShape
{
    class DebugCollision : IGraphic
    {
        private int layer = 0;
        private Shape debugShape;

        public DebugCollision(Movement movement, Collision collision)
        {
            layer = movement.Layer;
            if (collision is BoundingCircle)
            {
                var radius = ((BoundingCircle)collision).Radius;
                debugShape = new CircleShape(radius);
                debugShape.Position = new Vector2f(movement.X - radius, movement.Y - radius);
            }
            else
            {
                var width = ((BoundingBox)collision).Width;
                var height = ((BoundingBox)collision).Height;
                debugShape = new RectangleShape(new Vector2f(width, height));
                debugShape.Position = new Vector2f(movement.X - (width / 2f), movement.Y - (height / 2f));
            }

            debugShape.FillColor = Color.Transparent;
            debugShape.OutlineColor = Color.Magenta;
            debugShape.OutlineThickness = 1;
        }

        public void Draw(RenderTarget target)
        {
            target.Draw(debugShape);
        }


        public float GetLeft()
        {
            return debugShape.Position.X;
        }

        public float GetTop()
        {
            return debugShape.Position.Y;
        }

        public float GetWidth()
        {
            if (debugShape is RectangleShape)
            {
                return ((RectangleShape)debugShape).Size.X;
            }

            return ((CircleShape)debugShape).Radius * 2f;
        }

        public float GetHeight()
        {
            if (debugShape is RectangleShape)
            {
                return ((RectangleShape)debugShape).Size.Y;
            }

            return ((CircleShape)debugShape).Radius * 2f;
        }

        public int GetLayer()
        {
            return layer;
        }

        public int GetZOrder()
        {
            return Renderable.Z_DEBUG;
        }
    }
}
