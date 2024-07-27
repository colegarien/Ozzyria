using OpenTK.Audio.OpenAL;
using SkiaSharp;

namespace Ozzyria.Gryp.Models.Data
{
    internal class Layer
    {
        const int MAX_CAPACITY = 64;

        protected TileBoundary _boundary;

        private Layer? _parent;

        private Layer? _topLeft;
        private Layer? _topRight;
        private Layer? _bottomLeft;
        private Layer? _bottomRight;

        private Tile[,]? _tileData;
        private List<WorldBoundary> _walls;
        private List<Entity> _entities;

        protected bool _hasChanged = false;

        public Layer(TileBoundary boundary, Layer? parent = null)
        {
            _boundary = boundary;
            _parent = parent;
            if(_parent == null)
            {
                _walls = new List<WorldBoundary>();
                _entities = new List<Entity>();
            }

            // split if big map
            if (boundary.TileWidth * boundary.TileHeight > MAX_CAPACITY)
            {
                var maxHalfWidth = (int)Math.Ceiling(boundary.TileWidth / 2f);
                var maxHalfHeight = (int)Math.Ceiling(boundary.TileHeight / 2f);

                // to help account for odd number widths and thin strips (i.e. 1-by-X and X-by-1 size maps)
                var minHalfWidth = boundary.TileWidth - maxHalfWidth;
                var minHalfHeight = boundary.TileHeight - maxHalfHeight;


                _topLeft = new Layer(new TileBoundary
                {
                    TileX = _boundary.TileX,
                    TileY = _boundary.TileY,
                    // takes up most potential room
                    TileWidth = maxHalfWidth,
                    TileHeight = maxHalfHeight
                }, this);
                _topRight = new Layer(new TileBoundary
                {
                    TileX = _boundary.TileX + maxHalfWidth,
                    TileY = _boundary.TileY,
                    // takes up most height, but only left over width
                    TileWidth = minHalfWidth,
                    TileHeight = maxHalfHeight,
                }, this);
                _bottomLeft = new Layer(new TileBoundary
                {
                    TileX = _boundary.TileX,
                    TileY = _boundary.TileY + maxHalfHeight,
                    // takes up most width, but only left over height
                    TileWidth = maxHalfWidth,
                    TileHeight = minHalfHeight,
                }, this);
                _bottomRight = new Layer(new TileBoundary
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
                _tileData = new Tile[_boundary.TileWidth, _boundary.TileHeight];
                for (var y = _tileData.GetLength(1) - 1; y >= 0; y--)
                {
                    for (var x = 0; x < _tileData.GetLength(0); x++)
                    {
                        _tileData[x, y] = new Tile();
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

        public IEnumerable<WorldBoundary> GetWalls()
        {
            if (_parent == null)
            {
                return _walls;
            }
            else
            {
                return _parent.GetWalls();
            }
        }

        public void AddWall(WorldBoundary wall)
        {
            if(_parent == null)
            {
                ToggleChanged(true);
                _walls.Add(wall);
            }
            else
            {
                _parent.AddWall(wall);
            }
        }

        public void RemoveWalls(float worldX, float worldY)
        {
            if (_parent == null)
            {
                for(int i = _walls.Count - 1; i >= 0; i--)
                {
                    if (_walls[i].Contains(worldX, worldY))
                    {
                        ToggleChanged(true);
                        _walls.RemoveAt(i);
                    }
                }
            }
            else
            {
                _parent.RemoveWalls(worldX, worldY);
            }
        }

        public IEnumerable<Entity> GetEntities()
        {
            if (_parent == null)
            {
                return _entities;
            }
            else
            {
                return _parent.GetEntities();
            }
        }

        public void AddEntity(Entity entity)
        {
            if (_parent == null)
            {
                ToggleChanged(true);
                _entities.Add(entity);
            }
            else
            {
                _parent.AddEntity(entity);
            }
        }

        public void RemoveEntities(float worldX, float worldY)
        {
            if (_parent == null)
            {
                for (int i = _entities.Count - 1; i >= 0; i--)
                {
                    // remove close by entities
                    var distance = Math.Sqrt(Math.Pow(_entities[i].WorldX - worldX, 2) - Math.Pow(_entities[i].WorldY - worldY, 2));
                    if (distance <= 15)
                    {
                        ToggleChanged(true);
                        _entities.RemoveAt(i);
                    }
                }
            }
            else
            {
                _parent.RemoveEntities(worldX, worldY);
            }
        }

        public void PushTile(Tile tileData, int tileX, int tileY)
        {
            if (!_boundary.Contains(tileX, tileY))
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
                ToggleChanged(true);
                _tileData[tileX - _boundary.TileX, tileY - _boundary.TileY] = tileData;
            }
        }
        public void PaintArea(TileBoundary? region, Tile tileData, int originX, int originY)
        {
            var toFill = new List<Tuple<int, int>>
            {
                Tuple.Create(originX, originY)
            };

            while (toFill.Count > 0)
            {
                var toFillX = toFill[0].Item1;
                var toFillY = toFill[0].Item2;
                if (!_boundary.Contains(toFillX, toFillY) || (region != null && !region.Contains(toFillX, toFillY)))
                {
                    // nothing to fill, keep on keeping on
                    toFill.RemoveAt(0);
                    continue;
                }

                var toFillTile = GetTileData(toFillX, toFillY);
                if (toFillTile == null || toFillTile.Equal(tileData))
                {
                    // nothing to fill, keep on keeping on
                    toFill.RemoveAt(0);
                    continue;
                }

                var fillRight = GetTileData(toFillX + 1, toFillY)?.Equal(toFillTile) ?? false;
                var fillLeft = GetTileData(toFillX - 1, toFillY)?.Equal(toFillTile) ?? false;
                var fillDown = GetTileData(toFillX, toFillY + 1)?.Equal(toFillTile) ?? false;
                var fillUp = GetTileData(toFillX, toFillY - 1)?.Equal(toFillTile) ?? false;

                PushTile(tileData, toFillX, toFillY);
                toFill.RemoveAt(0);

                if (fillRight)
                {
                    toFill.Add(Tuple.Create(toFillX + 1, toFillY));
                }
                if (fillLeft)
                {
                    toFill.Add(Tuple.Create(toFillX - 1, toFillY));
                }
                if (fillDown)
                {
                    toFill.Add(Tuple.Create(toFillX, toFillY + 1));
                }
                if (fillUp)
                {
                    toFill.Add(Tuple.Create(toFillX, toFillY - 1));
                }
            }
        }

        public Tile? GetTileData(int tileX, int tileY)
        {
            if (!_boundary.Contains(tileX, tileY))
            {
                return null;
            }
            else if (IsSplit())
            {
                var bottomLeft = _bottomLeft?.GetTileData(tileX, tileY);
                if (bottomLeft != null)
                {
                    return bottomLeft;
                }

                var bottomRight = _bottomRight?.GetTileData(tileX, tileY);
                if (bottomRight != null)
                {
                    return bottomRight;
                }

                var topLeft = _topLeft?.GetTileData(tileX, tileY);
                if (topLeft != null)
                {
                    return topLeft;
                }

                var topRight = _topRight?.GetTileData(tileX, tileY);
                if (topRight != null)
                {
                    return topRight;
                }
            }
            else if (_tileData != null)
            {
                return _tileData[tileX - _boundary.TileX, tileY - _boundary.TileY];
            }

            return null;
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
                var dimension = Math.Max(1, camera.WorldToView(32));
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

            if(_parent == null)
            {
                foreach(var wall in _walls)
                {
                    var wallX = camera.ViewX + camera.WorldToView(wall.WorldX);
                    var wallY = camera.ViewY + camera.WorldToView(wall.WorldY);
                    var wallWidth = camera.WorldToView(wall.WorldWidth);
                    var wallHeight = camera.WorldToView(wall.WorldHeight);
                    canvas.DrawRect(new SKRect(wallX, wallY, wallX + wallWidth, wallY + wallHeight), Paints.WallEntityPaint);
                }

                foreach (var entity in _entities)
                {
                    var entityX = camera.ViewX + camera.WorldToView(entity.WorldX);
                    var entityY = camera.ViewY + camera.WorldToView(entity.WorldY);
                    var entityHalfWidth = camera.WorldToView(32) / 2f;
                    var entityHalfHeight = camera.WorldToView(32) / 2f;
                    canvas.DrawLine(entityX - entityHalfWidth, entityY, entityX + entityHalfWidth, entityY, Paints.PrefabEntityPaint);
                    canvas.DrawLine(entityX, entityY - entityHalfHeight, entityX, entityY + entityHalfHeight, Paints.PrefabEntityPaint);
                    canvas.DrawCircle(entityX, entityY, entityHalfWidth, Paints.PrefabEntityPaint);
                }
            }
        }

        public SKBitmap GetThumbnail(int size)
        {
            ToggleChanged(false);
            var thumbnailRender = new SKBitmap(new SKImageInfo(size, size));
            using (SKCanvas canvas = new SKCanvas(thumbnailRender))
            {
                var dummyCamera = new Camera();
                dummyCamera.Scale = (float)size / ((_boundary.TileWidth > _boundary.TileHeight ? _boundary.TileWidth : _boundary.TileHeight) * 32);
                dummyCamera.SizeCamera(size, size);
                dummyCamera.MoveToViewCoordinates(0, 0);

                RenderToCanvas(canvas, dummyCamera);
            }

            return thumbnailRender;
        }

        protected void ToggleChanged(bool changed)
        {
            if (_parent != null)
            {
                _parent.ToggleChanged(changed);
            }
            else
            {
                _hasChanged = changed;
            }
        }

        public bool HasChanged()
        {
            if (_parent != null)
            {
                return _parent.HasChanged();
            }
            else
            {
                return _hasChanged;
            }
        }
    }
}
