using SFML.Graphics;
using System.Collections.Generic;

namespace Ozzyria.Client
{
    class Graphic
    {
        public int Layer = 0;
        public float X = 0;
        public float Y = 0;
        public float Width = 0;
        public float Height = 0;
        public int Z = 0;
        public List<Drawable> drawables = new List<Drawable>();

        public virtual void Draw(RenderTarget target)
        {
            foreach (var drawable in drawables)
            {
                target.Draw(drawable);
            }
        }
    }
}
