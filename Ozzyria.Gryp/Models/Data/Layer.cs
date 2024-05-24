using SkiaSharp;

namespace Ozzyria.Gryp.Models.Data
{
    internal class LayerBoundary
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool contains(int x, int y)
        {
            return x < X + Width
                && y < Y + Height
                && X <= x
                && Y <= y;
        }

        public bool IsInCamera(Camera camera)
        {
            // camera WorldX and ViewX represent the world origin currently
            var boundaryWorldLeft = camera.WorldX + (X * 32);
            var boundaryWorldRight = boundaryWorldLeft + (Width * 32);
            var boundaryWorldTop = camera.WorldY + (Y * 32);
            var boundaryWorldBottom = boundaryWorldTop + (Height * 32);

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

        private SKBitmap _thumbnailRender;

        protected LayerBoundary _boundary;

        private Layer? _parent;

        private Layer? _topLeft;
        private Layer? _topRight;
        private Layer? _bottomLeft;
        private Layer? _bottomRight;

        private TileData[,] _tileData;

        public Layer(LayerBoundary boundary, Layer? parent = null)
        {
            _boundary = boundary;
            _parent = parent;

            // split if big map
            if (boundary.Width * boundary.Height > MAX_CAPACITY)
            {
                var maxHalfWidth = (int)Math.Ceiling(boundary.Width / 2f);
                var maxHalfHeight = (int)Math.Ceiling(boundary.Height / 2f);

                // to help account for odd number widths and thin strips (i.e. 1-by-X and X-by-1 size maps)
                var minHalfWidth = boundary.Width - maxHalfWidth;
                var minHalfHeight = boundary.Height - maxHalfHeight;


                _topLeft = new Layer(new LayerBoundary
                {
                    X = _boundary.X,
                    Y = _boundary.Y,
                    // takes up most potential room
                    Width = maxHalfWidth,
                    Height = maxHalfHeight
                }, this);
                _topRight = new Layer(new LayerBoundary
                {
                    X = _boundary.X + maxHalfWidth,
                    Y = _boundary.Y,
                    // takes up most height, but only left over width
                    Width = minHalfWidth,
                    Height = maxHalfHeight,
                }, this);
                _bottomLeft = new Layer(new LayerBoundary
                {
                    X = _boundary.X,
                    Y = _boundary.Y + maxHalfHeight,
                    // takes up most width, but only left over height
                    Width = maxHalfWidth,
                    Height = minHalfHeight,
                }, this);
                _bottomRight = new Layer(new LayerBoundary
                {
                    X = _boundary.X + maxHalfWidth,
                    Y = _boundary.Y + maxHalfHeight,
                    // takes only left over width and height
                    Width = minHalfWidth,
                    Height = minHalfHeight,
                }, this);
            }
            else
            {
                _tileData = new TileData[_boundary.Width, _boundary.Height];
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
            return _boundary.Width > 0 && _boundary.Height > 0;
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
            else
            {
                // draw layer relative to itself (everything off of (0, 0) and bottom up)
                var dimension = camera.WorldToView(32);
                var boundaryViewX = camera.WorldToView(_boundary.X * 32);
                var boundaryViewY = camera.WorldToView(_boundary.Y * 32);
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
            if (_thumbnailRender == null)
            {
                _thumbnailRender = new SKBitmap(new SKImageInfo(_boundary.Width * 32 + 1, _boundary.Height * 32 + 1));

                using (SKCanvas canvas = new SKCanvas(_thumbnailRender))
                {
                    var dummyCamera = new Camera();
                    dummyCamera.Scale = 1;
                    dummyCamera.SizeCamera(_thumbnailRender.Width, _thumbnailRender.Height);
                    dummyCamera.MoveToViewCoordinates(0, 0);

                    RenderToCanvas(canvas, dummyCamera);
                }
                _thumbnailRender = _thumbnailRender.Resize(new SKImageInfo(256, 256), SKFilterQuality.High);
            }

            return _thumbnailRender;
        }
    }
}
