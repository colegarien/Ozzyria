using SFML.Graphics;
using SFML.System;
using System;

namespace Ozzyria.Client.UI
{
    class OverlayProgressBar : Graphic // TODO OZ-13 : should this actually be a 'Graphic' or should graphic be reserved for in-game stuff only?
    {
        private const int NUM_SEGMENTS = 10;

        public Color background;
        public Color foreground;

        public OverlayProgressBar(float x, float y, Color backgroundColor, Color foregroundColor)
        {
            X = x;
            Y = y;
            Width = NUM_SEGMENTS * 20 + NUM_SEGMENTS * 22; // pieces width + padding between pieces
            Height = 10;

            background = backgroundColor;
            foreground = foregroundColor;
            for (var segment = 0; segment < NUM_SEGMENTS; segment++)
            {
                drawables.Insert(segment, new RectangleShape()
                {
                    Position = new Vector2f(x + (22 * segment), y),
                    Size = new Vector2f(20, Height),
                });
            }
        }

        public void SetMagnitude(int current, int max)
        {
            var fillToSegment = Math.Round((float)(current) / (float)(max) * NUM_SEGMENTS);
            for (var segment = 0; segment < NUM_SEGMENTS; segment++)
            {
                var fillSegment = segment < fillToSegment;
                ((RectangleShape)drawables[segment]).FillColor = fillSegment ? foreground : background;
            }
        }

    }
}
