using SFML.Graphics;
using System.Collections.Generic;

namespace Ozzyria.Client.Graphics
{
    public class CompositeGraphic : IGraphic
    {
        public int Layer { get; set; } = 0;
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Width { get; set; } = 0;
        public float Height { get; set; } = 0;
        public int Z { get; set; } = 0;
        public List<Drawable> drawables { get; set; } = new List<Drawable>();

        public void Draw(RenderTarget target)
        {
            foreach (var drawable in drawables)
            {
                target.Draw(drawable);
            }
        }

        public float GetLeft()
        {
            return X;
        }

        public float GetTop()
        {
            return Y;
        }

        public float GetWidth()
        {
            return Width;
        }

        public float GetHeight()
        {
            return Height;
        }

        public int GetLayer()
        {
            return Layer;
        }

        public int GetZOrder()
        {
            return Z;
        }
    }
}
