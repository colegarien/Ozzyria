using SFML.Graphics;
using SFML.System;

namespace Ozzyria.MapEditor
{
    class ViewWindow
    {
        private const float hScrollSensitivity = 5f;
        private const float vScrollSensitivity = 5f;
        private const float zoomSensitivity = 0.01f;

        private int windowX;
        private int windowY;
        private uint windowWidth;
        private uint windowHeight;

        private float xOffset = 0f;
        private float yOffset = 0f;
        private float zoomPercent = 1f;

        private Map _map; // current map being viewed
        private RenderTexture _renderBuffer; // for rendering window contents
        private RenderTexture _screenBuffer; // for rendering window to screen (mostly for proper cropping!)

        private float cursorScreenX = 0;
        private float cursorScreenY = 0;

        public ViewWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight)
        {
            _map = new Map(10, 10); // TODO have like a 'un/load map' and make _map nullable
            _renderBuffer = new RenderTexture((uint)(_map.Width * _map.TileDimension), (uint)(_map.Width * _map.TileDimension));

            ResizeWindow(x, y, width, height, screenWidth, screenHeight);
        }

        public void ResizeWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight)
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

            // Center on map
            CenterView();
        }

        private void CenterView()
        {
            if(_map == null)
            {
                xOffset = 0;
                yOffset = 0;
                zoomPercent = 1f;
            }

            xOffset = ((_map.Width * _map.TileDimension) * 0.5f) - (this.windowX + this.windowWidth * 0.5f);
            yOffset = ((_map.Height * _map.TileDimension) * 0.5f) - (this.windowY + this.windowHeight * 0.5f);
            zoomPercent = 1f;
        }

        public bool IsInWindow(int x, int y)
        {
            return x >= windowX && x < windowX + windowWidth
                && y >= windowY && y < windowY + windowHeight;
        }

        public void OnPan(float deltaX, float deltaY)
        {
            xOffset -= deltaX / zoomPercent;
            yOffset -= deltaY / zoomPercent;
        }

        public void OnPaint(int x, int y, TileType type)
        {
            if(_map == null)
            {
                return;
            }

            var tileDimension = _map.TileDimension;
            _map.SetTileType(0, (int)(ScreenToWorldX(x) / tileDimension), (int)(ScreenToWorldY(y) / tileDimension), type);
        }

        public void OnHorizontalScroll(float delta) {
            xOffset += (delta / zoomPercent) * hScrollSensitivity;
        }

        public void OnVerticalScroll(float delta)
        {
            yOffset -= (delta / zoomPercent) * vScrollSensitivity;
        }

        public void OnZoom(int xOrigin, int yOrigin, float delta)
        {
            var previousWorldXOrigin = ScreenToWorldX(xOrigin);
            var previousWorldYOrigin = ScreenToWorldY(yOrigin);

            var scale = (delta > 0)
                ? zoomSensitivity
                : -zoomSensitivity;
            zoomPercent *= 1 + scale;

            if(zoomPercent < 0.01f)
            {
                zoomPercent = 0.01f;
            }
            else if(zoomPercent > 200f)
            {
                zoomPercent = 200f;
            }

            var currentWorldXOrigin = ScreenToWorldX(xOrigin);
            var currentWorldYOrigin = ScreenToWorldY(yOrigin);

            xOffset += previousWorldXOrigin - currentWorldXOrigin;
            yOffset += previousWorldYOrigin - currentWorldYOrigin;
        }

        public void OnMouseMove(int x, int y)
        {
            cursorScreenX = x;
            cursorScreenY = y;
        }

        private float ScreenToWorldX(float screenX)
        {
            return (screenX / zoomPercent) + xOffset;
        }
        private float ScreenToWorldY(float screenY)
        {
            return (screenY / zoomPercent) + yOffset;
        }

        private float WorldToScreenX(float worldX)
        {
            return (worldX - xOffset) * zoomPercent;
        }
        private float WorldToScreenY(float worldY)
        {
            return (worldY - yOffset) * zoomPercent;
        }

        public void OnRender(RenderTarget surface)
        {
            _renderBuffer.Clear();
            if (_map == null)
            {
                return;
            }

            var tileDimension = _map.TileDimension;

            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var tileShape = new RectangleShape(new Vector2f(tileDimension, tileDimension));
                    tileShape.Position = new Vector2f((x * tileDimension), (y * tileDimension));
                    switch(_map.GetTileType(0, x, y))
                    {
                        case TileType.Ground:
                            tileShape.FillColor = Color.Green;
                            break;
                        case TileType.Water:
                            tileShape.FillColor = Color.Blue;
                            break;
                        case TileType.Fence:
                            tileShape.FillColor = Color.Red;
                            break;
                        default:
                            tileShape.FillColor = Color.Black;
                            break;
                    }

                    _renderBuffer.Draw(tileShape);
                }
            }

            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var overlayBorder = new RectangleShape(new Vector2f(tileDimension, tileDimension));
                    overlayBorder.Position = new Vector2f((x * tileDimension), (y * tileDimension));
                    overlayBorder.FillColor = Color.Transparent;
                    overlayBorder.OutlineThickness = 2;
                    overlayBorder.OutlineColor = new Color(140, 140, 140);

                    _renderBuffer.Draw(overlayBorder);
                }
            }

            var cursorShape = new RectangleShape(new Vector2f(tileDimension - 2, tileDimension - 2));
            cursorShape.Position = new Vector2f(((int)(ScreenToWorldX(cursorScreenX)/tileDimension)*tileDimension) + 1, ((int)(ScreenToWorldY(cursorScreenY)/tileDimension) * tileDimension) + 1);
            cursorShape.FillColor = Color.Transparent;
            cursorShape.OutlineThickness = 1;
            cursorShape.OutlineColor = Color.Cyan;
            _renderBuffer.Draw(cursorShape);

            _renderBuffer.Display();
            _screenBuffer.Clear();
            // draw map
            _screenBuffer.Draw(new Sprite(_renderBuffer.Texture)
            {
                Position = new Vector2f(WorldToScreenX(0), WorldToScreenY(0)),
                Scale = new Vector2f(zoomPercent, zoomPercent)
            });

            // draw border around window
            _screenBuffer.Draw(new RectangleShape
            {
                Position = new Vector2f(windowX+2, windowY+2),
                Size = new Vector2f(windowWidth-4, windowHeight-4),
                FillColor = Color.Transparent,
                OutlineThickness = 2,
                OutlineColor = Color.Yellow
            });
            _screenBuffer.Display();

            surface.Draw(new Sprite(_screenBuffer.Texture)
            {
                Position = new Vector2f(windowX, windowY),
                Scale = new Vector2f((float)windowWidth / (float)_screenBuffer.Size.X, (float)windowHeight / (float)_screenBuffer.Size.Y)
            });
        }
    }
}
