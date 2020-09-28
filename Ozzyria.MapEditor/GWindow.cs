using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;
using SFML.System;

namespace Ozzyria.MapEditor
{
    abstract class GWindow : IObserver
    {
        protected int windowX;
        protected int windowY;
        protected uint windowWidth;
        protected uint windowHeight;

        protected int margin;
        protected int padding;

        protected RenderTexture _screenBuffer; // for rendering window to screen (mostly for proper cropping!)

        public GWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight, int margin, int padding)
        {
            this.margin = margin;
            this.padding = padding; // TODO use padding
            OnResize(x, y, width, height, screenWidth, screenHeight);
        }

        protected int GetLeft()
        {
            return windowX + margin;
        }

        protected int GetTop()
        {
            return windowY + margin;
        }

        protected int GetWidth()
        {
            return (int)windowWidth - (margin * 2);
        }
        protected int GetHeight()
        {
            return (int)windowHeight - (margin * 2);
        }

        protected int GetRight()
        {
            return (int)(GetLeft() + GetWidth());
        }

        protected int GetCenterX()
        {
            return (int)(GetLeft() + GetWidth() * 0.5f);
        }

        protected int GetBottom()
        {
            return (int)(GetTop() + GetHeight());
        }

        protected int GetCenterY()
        {
            return (int)(GetTop() + GetHeight() * 0.5f);
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
            _screenBuffer.SetView(new View(new FloatRect(GetLeft(), GetTop(), GetWidth(), GetHeight())));
        }

        public bool IsInWindow(int x, int y)
        {
            return x >= GetLeft() && x < GetRight()
                && y >= GetTop() && y < GetBottom();
        }

        public virtual bool CanHandle(IEvent e)
        {
            if (e is WindowSpecificEvent)
            {
                var w = (WindowSpecificEvent)e;
                return IsInWindow(w.OriginX, w.OriginY);
            }

            return e is MouseMoveEvent;
        }

        public virtual void Notify(IEvent e)
        {
            if (e is HorizontalScrollEvent)
            {
                OnHorizontalScroll((HorizontalScrollEvent)e);
            }
            else if (e is VerticalScrollEvent)
            {
                OnVerticalScroll((VerticalScrollEvent)e);
            }
            else if (e is MouseMoveEvent)
            {
                OnMouseMove((MouseMoveEvent)e);
            }
            else if (e is MouseDownEvent)
            {
                OnMouseDown((MouseDownEvent)e);
            }
        }

        public abstract void OnMouseDown(MouseDownEvent e);
        public abstract void OnMouseMove(MouseMoveEvent e);
        public abstract void OnHorizontalScroll(HorizontalScrollEvent e);
        public abstract void OnVerticalScroll(VerticalScrollEvent e);


        public void OnRender(RenderTarget surface)
        {
            _screenBuffer.Clear();
            RenderWindowContents(_screenBuffer);

            // draw border around window
            _screenBuffer.Draw(new RectangleShape
            {
                Position = new Vector2f(GetLeft() + 2, GetTop() + 2),
                Size = new Vector2f(GetWidth() - 4, GetHeight() - 4),
                FillColor = Color.Transparent,
                OutlineThickness = 2,
                OutlineColor = Color.Yellow
            });
            _screenBuffer.Display();

            // draw window to surface
            surface.Draw(new Sprite(_screenBuffer.Texture)
            {
                Position = new Vector2f(GetLeft(), GetTop()),
                Scale = new Vector2f((float)GetWidth() / (float)_screenBuffer.Size.X, (float)GetHeight() / (float)_screenBuffer.Size.Y)
            });
        }

        protected abstract void RenderWindowContents(RenderTarget buffer);
    }
}
