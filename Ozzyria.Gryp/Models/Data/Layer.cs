using SkiaSharp;

namespace Ozzyria.Gryp.Models.Data
{
    internal class LayerBoundary // TODO switch to Boundary
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public bool contains(int tileX, int tileY)
        {
            return tileX < TileX + TileWidth
                && tileY < TileY + TileHeight
                && TileX <= tileX
                && TileY <= tileY;
        }

        public bool IsInCamera(Camera camera)
        {
            // camera WorldX and ViewX represent the world origin currently
            var boundaryWorldLeft = camera.WorldX + (TileX * 32);
            var boundaryWorldRight = boundaryWorldLeft + (TileWidth * 32);
            var boundaryWorldTop = camera.WorldY + (TileY * 32);
            var boundaryWorldBottom = boundaryWorldTop + (TileHeight * 32);

            // The world moves not the camera
            var cameraWorldLeft = 0;
            var cameraWorldTop = 0;
            var cameraWorldRight = camera.WorldWidth;
            var cameraWorldBottom = camera.WorldHeight;

            // easier to check if NOT in camera and then inverse
            return !((boundaryWorldRight < cameraWorldLeft || boundaryWorldLeft >= cameraWorldRight)
                || (boundaryWorldBottom < cameraWorldTop || boundaryWorldTop >= cameraWorldBottom));
        }
    }

    internal class Layer
    {
        const int MAX_CAPACITY = 64;

        protected LayerBoundary _boundary;

        private Layer? _parent;

        private Layer? _topLeft;
        private Layer? _topRight;
        private Layer? _bottomLeft;
        private Layer? _bottomRight;

        private TileData[,]? _tileData;

        public Layer(LayerBoundary boundary, Layer? parent = null)
        {
            _boundary = boundary;
            _parent = parent;

            // split if big map
            if (boundary.TileWidth * boundary.TileHeight > MAX_CAPACITY)
            {
                var maxHalfWidth = (int)Math.Ceiling(boundary.TileWidth / 2f);
                var maxHalfHeight = (int)Math.Ceiling(boundary.TileHeight / 2f);

                // to help account for odd number widths and thin strips (i.e. 1-by-X and X-by-1 size maps)
                var minHalfWidth = boundary.TileWidth - maxHalfWidth;
                var minHalfHeight = boundary.TileHeight - maxHalfHeight;


                _topLeft = new Layer(new LayerBoundary
                {
                    TileX = _boundary.TileX,
                    TileY = _boundary.TileY,
                    // takes up most potential room
                    TileWidth = maxHalfWidth,
                    TileHeight = maxHalfHeight
                }, this);
                _topRight = new Layer(new LayerBoundary
                {
                    TileX = _boundary.TileX + maxHalfWidth,
                    TileY = _boundary.TileY,
                    // takes up most height, but only left over width
                    TileWidth = minHalfWidth,
                    TileHeight = maxHalfHeight,
                }, this);
                _bottomLeft = new Layer(new LayerBoundary
                {
                    TileX = _boundary.TileX,
                    TileY = _boundary.TileY + maxHalfHeight,
                    // takes up most width, but only left over height
                    TileWidth = maxHalfWidth,
                    TileHeight = minHalfHeight,
                }, this);
                _bottomRight = new Layer(new LayerBoundary
                {
                    TileX = _boundary.TileX + maxHalfWidth,
                    TileY = _boundary.TileY + maxHalfHeight,
                    // takes only left over width and height
                    TileWidth = minHalfWidth,
                    TileHeight = minHalfHeight,
                }, this);
            }
            else
            {
                _tileData = new TileData[_boundary.TileWidth, _boundary.TileHeight];
                for (var y = _tileData.GetLength(1) - 1; y >= 0; y--)
                {
                    for (var x = 0; x < _tileData.GetLength(0); x++)
                    {
                        _tileData[x, y] = new TileData();
                    }
                }
            }
        }

        public bool IsSplit()
        {
            return (_tileData?.Length ?? 0) <= 0 && _topLeft != null && _topRight != null && _bottomLeft != null && _bottomRight != null;
        }

        public bool CanRender()
        {
            return _boundary.TileWidth > 0 && _boundary.TileHeight > 0;
        }

        public void PushTile(TileData tileData, int tileX, int tileY)
        {
            if (!_boundary.contains(tileX, tileY))
            {
                return;
            } else if (IsSplit())
            {
                _bottomLeft?.PushTile(tileData, tileX, tileY);
                _bottomRight?.PushTile(tileData, tileX, tileY);
                _topLeft?.PushTile(tileData, tileX, tileY);
                _topRight?.PushTile(tileData, tileX, tileY);
            }
            else if (_tileData != null)
            {
                _tileData[tileX - _boundary.TileX, tileY - _boundary.TileY] = tileData;
            }
        }

        public void RenderToCanvas(SKCanvas canvas, Camera camera)
        {
            if (canvas == null || !CanRender() || !_boundary.IsInCamera(camera))
            {
                return;
            }



            if (IsSplit())
            {
                _bottomLeft?.RenderToCanvas(canvas, camera);
                _bottomRight?.RenderToCanvas(canvas, camera);
                _topLeft?.RenderToCanvas(canvas, camera);
                _topRight?.RenderToCanvas(canvas, camera);
            }
            else if (_tileData != null)
            {
                // draw layer relative to itself (everything off of (0, 0) and bottom up)
                var dimension = camera.WorldToView(32);
                var boundaryViewX = camera.WorldToView(_boundary.TileX * 32);
                var boundaryViewY = camera.WorldToView(_boundary.TileY * 32);
                for (var y = _tileData.GetLength(1) - 1; y >= 0; y--)
                {
                    var tileY =camera.ViewY + boundaryViewY + camera.WorldToView(y * 32);
                    for (var x = 0; x < _tileData.GetLength(0); x++)
                    {
                        var tileX =camera.ViewX + boundaryViewX + camera.WorldToView(x * 32);
                        _tileData[x, y].Render(canvas, tileX, tileY, dimension, dimension);
                    }
                }
            }
        }

        public SKBitmap GetThumbnail()
        {
            var thumbnailRender = new SKBitmap(new SKImageInfo(32, 32));
            using (SKCanvas canvas = new SKCanvas(thumbnailRender))
            {
                var dummyCamera = new Camera();
                dummyCamera.Scale = 32f / ((_boundary.TileWidth > _boundary.TileHeight ? _boundary.TileWidth : _boundary.TileHeight) * 32);
                dummyCamera.SizeCamera(32, 32);
                dummyCamera.MoveToViewCoordinates(0, 0);

                RenderToCanvas(canvas, dummyCamera);
            }

            return thumbnailRender;
        }
    }
}
