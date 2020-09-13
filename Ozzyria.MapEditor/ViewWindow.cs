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

            // ReCenter on map
            xOffset = 0; //((_map.Width * _map.TileDimension) * 0.5f) - (this.width * 0.5f);
            yOffset = 0; //((_map.Height * _map.TileDimension) * 0.5f) - (this.height * 0.5f);
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
            var previousZoomPercent = zoomPercent;
            zoomPercent += delta * zoomSensitivity;
            
            if(zoomPercent < 0.01f)
            {
                zoomPercent = 0.01f;
            }
            else if(zoomPercent > 200f)
            {
                zoomPercent = 200f;
            }

            //var deltaZoom = zoomPercent - previousZoomPercent;
            //var worldX =  (xOrigin * zoomPercent) - (xOrigin * previousZoomPercent);
            //var worldY = (yOrigin * zoomPercent) - (yOrigin * previousZoomPercent);

            //xOffset += xOrigin * deltaZoom;
            //yOffset += yOrigin * deltaZoom;
            //xOffset -= (((xOrigin + (xOffset)) * zoomPercent) - ((xOrigin + (xOffset)) * previousZoomPercent));
            //yOffset -= (((yOrigin + (yOffset)) * zoomPercent) - ((yOrigin + (yOffset)) * previousZoomPercent));
            // TODO zoom on xOrigin + yOrigin (if I remember right this is called 'Affine Transformation')
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
                Position = new SFML.System.Vector2f(-xOffset, -yOffset),
                Scale = new SFML.System.Vector2f(zoomPercent, zoomPercent)
            });
        }
    }
}
