using Ozzyria.Game;
using Ozzyria.Game.Components;
using SFML.Graphics;
using SFML.System;

namespace Ozzyria.Client.Graphics.UI
{
    class HoverStatBar : IGraphic
    {
        private int layer = 0;
        private RectangleShape background;
        private RectangleShape overlay;

        public HoverStatBar(int layer, float x, float y, int current, int max)
        {
            this.layer = layer;
            var offset = new Vector2f(0, 14);

            background = new RectangleShape(new Vector2f(26, 5));
            background.Origin = new Vector2f(background.Size.X / 2 + offset.X, background.Size.Y + offset.Y);
            background.FillColor = Color.Red;

            overlay = new RectangleShape(background.Size);
            overlay.Origin = background.Origin;
            overlay.FillColor = Color.Green;

            Move(x, y);
            SetMagnitude(current, max);
        }

        public void Move(float x, float y)
        {
            background.Position = new Vector2f(x, y);
            overlay.Position = new Vector2f(x, y);
        }

        public void SetMagnitude(int current, int max)
        {
            overlay.Size = new Vector2f(((float)current / (float)max) * background.Size.X, overlay.Size.Y);
        }

        public void Draw(RenderTarget target)
        {
            target.Draw(background);
            target.Draw(overlay);
        }


        public float GetLeft()
        {
            return background.Position.X - background.Origin.X;
        }

        public float GetTop()
        {
            return background.Position.Y - background.Origin.Y;
        }

        public float GetWidth()
        {
            return background.Size.X;
        }

        public float GetHeight()
        {
            return background.Size.Y;
        }

        public int GetLayer()
        {
            return layer;
        }

        public int GetZOrder()
        {
            return (int)ZLayer.InGameUi;
        }
    }
}
