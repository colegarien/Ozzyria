using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Client
{
    class Graphic
    {
        public int Layer = 0;
        public float X = 0;
        public float Y = 0;
        public int Z = 0;
        public List<Drawable> drawables = new List<Drawable>();

        public void Draw(RenderTarget target)
        {
            foreach (var drawable in drawables)
            {
                target.Draw(drawable);
            }
        }
    }
}
