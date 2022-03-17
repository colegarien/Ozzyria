using Ozzyria.Game;
using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.System;

namespace Ozzyria.Client.Graphics.DebugShape
{
    class DebugRenderArea : IGraphic
    {
        private int layer;
        private RectangleShape debugShape;

        public DebugRenderArea(IGraphic graphic)
        {
            layer = graphic.GetLayer();

            debugShape = new RectangleShape(new Vector2f(graphic.GetWidth(), graphic.GetHeight()));
            debugShape.Position = new Vector2f(graphic.GetLeft(), graphic.GetTop());
            debugShape.FillColor = Color.Transparent;
            debugShape.OutlineColor = Color.Blue;
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
            return debugShape.Size.X;
        }

        public float GetHeight()
        {
            return debugShape.Size.Y;
        }

        public int GetLayer()
        {
            return layer;
        }

        public int GetZOrder()
        {
            return (int)ZLayer.Debug;
        }
    }
}
