using SFML.Graphics;
using SFML.System;
using System;

namespace Ozzyria.Client.Graphics.UI
{
    class OverlayProgressBar : IGraphic
    {
        private const int NUM_SEGMENTS = 10;
        private const int SEGMENT_WIDTH = 20;
        private const int SEGMENT_PADDING = 2;
        private const int SEGMENT_HEIGHT = 20;
        private RectangleShape[] segments = new RectangleShape[NUM_SEGMENTS];

        private Color background;
        private Color foreground;

        public OverlayProgressBar(float x, float y, Color backgroundColor, Color foregroundColor)
        {
            background = backgroundColor;
            foreground = foregroundColor;
            for (var segment = 0; segment < NUM_SEGMENTS; segment++)
            {
                segments[segment] = new RectangleShape()
                {
                    Position = new Vector2f(x + ((SEGMENT_WIDTH + SEGMENT_PADDING) * segment), y),
                    Size = new Vector2f(20, SEGMENT_HEIGHT),
                };
            }
        }

        public void SetMagnitude(int current, int max)
        {
            var fillToSegment = Math.Round((float)(current) / (float)(max) * NUM_SEGMENTS);
            for (var segment = 0; segment < NUM_SEGMENTS; segment++)
            {
                var fillSegment = segment < fillToSegment;
                segments[segment].FillColor = fillSegment ? foreground : background;
            }
        }

        public void Draw(RenderTarget target)
        {
            foreach (var segment in segments)
            {
                target.Draw(segment);
            }
        }

        public float GetLeft()
        {
            return segments[0].Position.X - segments[0].Origin.X;
        }

        public float GetTop()
        {
            return segments[0].Position.Y - segments[0].Origin.Y;
        }

        public float GetWidth()
        {
            return (SEGMENT_WIDTH + SEGMENT_PADDING) * NUM_SEGMENTS;
        }

        public float GetHeight()
        {
            return SEGMENT_HEIGHT;
        }

        public int GetLayer()
        {
            return 99999; // overlay that sucka
        }

        public int GetZOrder()
        {
            return 0;
        }

    }
}
