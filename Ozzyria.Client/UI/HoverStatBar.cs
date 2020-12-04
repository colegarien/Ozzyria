using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.System;

namespace Ozzyria.Client.UI
{
    class HoverStatBar : Graphic // TODO OZ-13 : make this inheritance less nasty; forcing array of drawables is... frankly, shit
    {
        public HoverStatBar()
        {
            var offset = new Vector2f(0, 14);

            var background = new RectangleShape(new Vector2f(26, 5));
            background.Origin = new Vector2f(background.Size.X / 2 + offset.X, background.Size.Y + offset.Y);
            background.FillColor = Color.Red;

            var overlay = new RectangleShape(background.Size);
            overlay.Origin = background.Origin;
            overlay.FillColor = Color.Green;

            drawables = new System.Collections.Generic.List<Drawable>
            {
                background,
                overlay
            };

            Width = background.Size.X;
            Height = background.Size.Y;
            Z = Renderable.Z_INGAME_UI;
        }

        public void Move(float x, float y)
        {
            ((RectangleShape)drawables[0]).Position = new Vector2f(x, y);
            ((RectangleShape)drawables[1]).Position = new Vector2f(x, y);

            X = ((RectangleShape)drawables[0]).Position.X - ((RectangleShape)drawables[0]).Origin.X;
            Y = ((RectangleShape)drawables[0]).Position.Y - ((RectangleShape)drawables[0]).Origin.Y;
        }

        public void SetMagnitude(int current, int max)
        {
            ((RectangleShape)drawables[1]).Size = new Vector2f(((float)current / (float)max) * ((RectangleShape)drawables[0]).Size.X, ((RectangleShape)drawables[1]).Size.Y);
        }
    }
}
