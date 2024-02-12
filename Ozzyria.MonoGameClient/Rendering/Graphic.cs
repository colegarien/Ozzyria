using Microsoft.Xna.Framework;

namespace Ozzyria.MonoGameClient.Rendering
{
    internal class Graphic
    {
        public bool Hidden { get; set; } = false;
        public long RenderPriority
        {
            get
            {
                return Layer * 1000000000000L + SubLayer * 1000L + SubSubLayer;
            }
        }

        public int Layer { get; set; } = 0;
        public int SubLayer { get; set; } = 0;
        public int SubSubLayer { get; set; } = 0;

        public uint Resource { get; set; }
        public Rectangle Source { get; set; }
        public Rectangle Destination { get; set; }

        public Vector2 Origin { get; set; } = Vector2.Zero;
        public float Angle { get; set; } = 0f;
        public Color Colour { get; set; } = Color.White;

        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }
    }
}
