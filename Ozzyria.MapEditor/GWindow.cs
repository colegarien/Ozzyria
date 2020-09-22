using SFML.Graphics;
using SFML.System;

namespace Ozzyria.MapEditor
{
    abstract class GWindow
    {
        protected int windowX;
        protected int windowY;
        protected uint windowWidth;
        protected uint windowHeight;

        protected RenderTexture _screenBuffer; // for rendering window to screen (mostly for proper cropping!)

        public GWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight)
        {
            OnResize(x, y, width, height, screenWidth, screenHeight);
        }

        public virtual void OnResize(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight)
        {
            windowX = x;
            windowY = y;
            windowWidth = width;
            windowHeight = height;

            if (_screenBuffer != null)
            {
                _screenBuffer.Dispose();
            }

            _screenBuffer = new RenderTexture(screenWidth, screenHeight);
            _screenBuffer.SetView(new View(new FloatRect(windowX, windowY, windowWidth, windowHeight)));
        }

        public bool IsInWindow(int x, int y)
        {
            return x >= windowX && x < windowX + windowWidth
                && y >= windowY && y < windowY + windowHeight;
        }


        public abstract void OnMouseMove(int x, int y);
        public abstract void OnHorizontalScroll(float delta);
        public abstract void OnVerticalScroll(float delta);


        public void OnRender(RenderTarget surface)
        {
            _screenBuffer.Clear();
            RenderWindowContents(_screenBuffer);

            // draw border around window
            _screenBuffer.Draw(new RectangleShape
            {
                Position = new Vector2f(windowX + 2, windowY + 2),
                Size = new Vector2f(windowWidth - 4, windowHeight - 4),
                FillColor = Color.Transparent,
                OutlineThickness = 2,
                OutlineColor = Color.Yellow
            });
            _screenBuffer.Display();

            // draw window to surface
            surface.Draw(new Sprite(_screenBuffer.Texture)
            {
                Position = new Vector2f(windowX, windowY),
                Scale = new Vector2f((float)windowWidth / (float)_screenBuffer.Size.X, (float)windowHeight / (float)_screenBuffer.Size.Y)
            });
        }

        protected abstract void RenderWindowContents(RenderTarget buffer);
    }
}
