using SFML.Graphics;

namespace Ozzyria.MapEditor
{
    class ViewWindow
    {
        private const float hScrollSensitivity = 5f;
        private const float vScrollSensitivity = 5f;
        private const float zoomSensitivity = 0.01f;

        private int width;
        private int height;
        private float xOffset = 0f;
        private float yOffset = 0f;
        private float zoomPercent = 1f;

        private Map _map;
        private RenderTexture _renderBuffer;

        public ViewWindow(int width, int height)
        {
            this.width = width;
            this.height = height;

            _map = new Map
            {
                Width = 10,
                Height = 10
            }; // TODO have like a 'un/load map' and make _map nullable
            _renderBuffer = new RenderTexture((uint)(_map.Width * _map.TileDimension), (uint)(_map.Width * _map.TileDimension));

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

            xOffset = ((_map.Width * _map.TileDimension) * 0.5f) - (this.width * 0.5f);
            yOffset = ((_map.Height * _map.TileDimension) * 0.5f) - (this.height * 0.5f);
            zoomPercent = 1f;
        }

        public void OnHorizontalScroll(float delta) {
            xOffset += delta * hScrollSensitivity;
        }

        public void OnVerticalScroll(float delta)
        {
            yOffset -= delta * vScrollSensitivity;
        }

        public void OnZoom(int xOrigin, int yOrigin, float delta)
        {
            var previousWorldXOrigin = ScreenToWorldX(xOrigin);
            var previousWorldYOrigin = ScreenToWorldY(yOrigin);

            zoomPercent += delta * zoomSensitivity;
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

        public void OnRender(RenderWindow window)
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
                    var tileShape = new RectangleShape(new SFML.System.Vector2f(tileDimension, tileDimension));
                    tileShape.Position = new SFML.System.Vector2f((x * tileDimension), (y * tileDimension));

                    _renderBuffer.Draw(tileShape);
                }
            }

            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    var overlayBorder = new RectangleShape(new SFML.System.Vector2f(tileDimension, tileDimension));
                    overlayBorder.Position = new SFML.System.Vector2f((x * tileDimension), (y * tileDimension));
                    overlayBorder.FillColor = Color.Transparent;
                    overlayBorder.OutlineThickness = 2;
                    overlayBorder.OutlineColor = Color.Black;

                    _renderBuffer.Draw(overlayBorder);
                }
            }

            _renderBuffer.Display();
            window.Draw(new Sprite(_renderBuffer.Texture)
            {
                Position = new SFML.System.Vector2f(WorldToScreenX(0), WorldToScreenY(0)),
                Scale = new SFML.System.Vector2f(zoomPercent, zoomPercent)
            });
        }
    }
}
