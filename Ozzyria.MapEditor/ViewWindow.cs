using SFML.Graphics;
using SFML.System;

namespace Ozzyria.MapEditor
{
    class ViewWindow : GWindow
    {
        private const float hScrollSensitivity = 5f;
        private const float vScrollSensitivity = 5f;
        private const float zoomSensitivity = 0.01f;

        private float xOffset = 0f;
        private float yOffset = 0f;
        public float zoomPercent = 1f;

        private Map _map; // current map being viewed
        public int Layer { get; set; } = 0;
        private RenderTexture _renderBuffer; // for rendering window contents

        private float cursorScreenX = 0;
        private float cursorScreenY = 0;

        public ViewWindow(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight) : base(x, y, width, height, screenWidth, screenHeight)
        {
        }

        public void LoadMap(Map map)
        {
            if(_map != null)
            {
                _map = null;
                _renderBuffer.Dispose();
            }

            _map = map;
            _renderBuffer = new RenderTexture((uint)(_map.Width * _map.TileDimension), (uint)(_map.Height * _map.TileDimension));
            CenterView();
        }

        public override void OnResize(int x, int y, uint width, uint height, uint screenWidth, uint screenHeight)
        {
            base.OnResize(x, y, width, height, screenWidth, screenHeight);

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
                return;
            }

            // center in window based
            zoomPercent = 1f;
            xOffset = (((_map.Width * _map.TileDimension) * 0.5f) - (this.windowX + this.windowWidth * 0.5f));
            yOffset = (((_map.Height * _map.TileDimension) * 0.5f) - (this.windowY + this.windowHeight * 0.5f));

            // biggest dimension should take of 88% of the screen (cause it look nice)
            var newZoom = (0.88f * (float)this.windowWidth) / (_map.TileDimension * _map.Width);
            if (windowHeight < windowWidth)
            {
                newZoom = (0.88f * (float)this.windowHeight) / (_map.TileDimension * _map.Height);
            }

            ZoomTo((int)(this.windowX + this.windowWidth * 0.5f), (int)(this.windowY + this.windowHeight * 0.5f), newZoom);
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
            _map.SetTileType(Layer, (int)(ScreenToWorldX(x) / tileDimension), (int)(ScreenToWorldY(y) / tileDimension), type);
        }

        public override void OnHorizontalScroll(float delta) {
            xOffset += (delta / zoomPercent) * hScrollSensitivity;
        }

        public override void OnVerticalScroll(float delta)
        {
            yOffset -= (delta / zoomPercent) * vScrollSensitivity;
        }

        public override void OnMouseMove(int x, int y)
        {
            cursorScreenX = x;
            cursorScreenY = y;
        }

        public void OnZoom(int xOrigin, int yOrigin, float delta)
        {
            var scale = (delta > 0)
                ? zoomSensitivity
                : -zoomSensitivity;
            var targetZoomPercent = zoomPercent * (1 + scale);

            ZoomTo(xOrigin, yOrigin, targetZoomPercent);
        }

        private void ZoomTo(int xOrigin, int yOrigin, float targetZoomPercent)
        {
            var previousWorldXOrigin = ScreenToWorldX(xOrigin);
            var previousWorldYOrigin = ScreenToWorldY(yOrigin);

            zoomPercent = targetZoomPercent;
            if (zoomPercent < 0.05f)
            {
                zoomPercent = 0.05f;
            }
            else if (zoomPercent > 10f)
            {
                zoomPercent = 10f;
            }

            var currentWorldXOrigin = ScreenToWorldX(xOrigin);
            var currentWorldYOrigin = ScreenToWorldY(yOrigin);

            xOffset += previousWorldXOrigin - currentWorldXOrigin;
            yOffset += previousWorldYOrigin - currentWorldYOrigin;
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

        protected override void RenderWindowContents(RenderTarget buffer)
        {
            if (_map == null)
            {
                return;
            }

            _renderBuffer.Clear();
            var tileDimension = _map.TileDimension;

            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var tileShape = new RectangleShape(new Vector2f(tileDimension, tileDimension));
                    tileShape.Position = new Vector2f((x * tileDimension), (y * tileDimension));
                    switch (_map.GetTileType(Layer, x, y))
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
            cursorShape.Position = new Vector2f(((int)(ScreenToWorldX(cursorScreenX) / tileDimension) * tileDimension) + 1, ((int)(ScreenToWorldY(cursorScreenY) / tileDimension) * tileDimension) + 1);
            cursorShape.FillColor = Color.Transparent;
            cursorShape.OutlineThickness = 1;
            cursorShape.OutlineColor = Color.Cyan;
            _renderBuffer.Draw(cursorShape);

            _renderBuffer.Display();

            // draw map
            buffer.Draw(new Sprite(_renderBuffer.Texture)
            {
                Position = new Vector2f(WorldToScreenX(0), WorldToScreenY(0)),
                Scale = new Vector2f(zoomPercent, zoomPercent)
            });
        }
    }
}
