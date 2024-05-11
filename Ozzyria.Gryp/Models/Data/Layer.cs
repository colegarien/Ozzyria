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
    }

    internal class Layer
    {
        const int MAX_CAPACITY = 64;

        private Bitmap _compositeRender;
        private bool _needsRendering = true;

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

            _compositeRender = new Bitmap(boundary.Width * 32 + 1, boundary.Height * 32 + 1);

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

        public void SignalNeedsRender()
        {
            _needsRendering = CanRender();
            if (_needsRendering && _parent != null)
            {
                _parent?.SignalNeedsRender();
            }
        }

        public Bitmap GetImage()
        {
            if (_needsRendering)
            {
                _needsRendering = false;
                if (IsSplit())
                {
                    var topLeftImage = _topLeft?.GetImage();
                    var topRightImage = _topRight?.GetImage();
                    var bottomLeftImage = _bottomLeft?.GetImage();
                    var bottomRightImage = _bottomRight?.GetImage();


                    using (var graphics = Graphics.FromImage(_compositeRender))
                    {
                        // render images bottom up
                        if (bottomLeftImage != null)
                        {
                            graphics.DrawImage(bottomLeftImage, new Point((_bottomLeft._boundary.X - _boundary.X) * 32, (_bottomLeft._boundary.Y - _boundary.Y) * 32));
                        }

                        if (bottomRightImage != null)
                        {
                            graphics.DrawImage(bottomRightImage, new Point((_bottomRight._boundary.X - _boundary.X) * 32, (_bottomRight._boundary.Y - _boundary.Y) * 32));
                        }

                        if (topLeftImage != null)
                        {
                            graphics.DrawImage(topLeftImage, new Point((_topLeft._boundary.X - _boundary.X) * 32, (_topLeft._boundary.Y - _boundary.Y) * 32));
                        }

                        if (topRightImage != null)
                        {
                            graphics.DrawImage(topRightImage, new Point((_topRight._boundary.X - _boundary.X) * 32, (_topRight._boundary.Y - _boundary.Y) * 32));
                        }
                    }
                }
                else if (CanRender())
                {
                    using (var graphics = Graphics.FromImage(_compositeRender))
                    {
                        // draw layer relative to itself (everything off of (0, 0) and bottom up)
                        for (var y = _tileData.GetLength(1) - 1; y >= 0; y--)
                        {
                            var tileY = y * 32;
                            for (var x = 0; x < _tileData.GetLength(0); x++)
                            {
                                var tileX = x * 32;
                                _tileData[x, y].Render(graphics, tileX, tileY);
                            }
                        }
                    }
                }
            }

            return _compositeRender;
        }
    }
}
