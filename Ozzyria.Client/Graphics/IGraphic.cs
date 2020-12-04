using SFML.Graphics;

namespace Ozzyria.Client.Graphics
{
    interface IGraphic
    {
        public float GetLeft();
        public float GetTop();
        public float GetWidth();
        public float GetHeight();

        public int GetLayer();
        public int GetZOrder();

        public void Draw(RenderTarget target);
    }
}
