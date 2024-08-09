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
        private List<Wall> _walls;
        private List<Entity> _entities;

        protected bool _hasChanged = false;

        public Layer(TileBoundary boundary, Layer? parent = null)
        {
            _boundary = boundary;
            _parent = parent;
            if(_parent == null)
            {
                _walls = new List<Wall>();
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

        public IEnumerable<Wall> GetWalls()
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

        public Wall? GetWall(string internalId)
        {
            if(_parent == null)
            {
                for(int i = 0; i < _walls.Count; i++)
                {
                    if (_walls[i].InternalId == internalId)
                    {
                        return _walls[i];
                    }
                }

                return null;
            }
            else
            {
                return _parent.GetWall(internalId);
            }
        }

        public Wall AddWall(Wall wall)
        {
            if(_parent == null)
            {
                ToggleChanged(true);
                if(wall.InternalId == "")
                    wall.InternalId = System.Guid.NewGuid().ToString();
                ChangeHistory.TrackChange(new AddWallChange
                {
                    InternalId = wall.InternalId
                });
                _walls.Add(wall);

                return wall;
            }
            else
            {
                return _parent.AddWall(wall);
            }
        }

        public Wall? SelectWall(float worldX, float worldY, Wall? currentlySelectedWall)
        {
            if (_parent == null)
            {
                Wall? firstWall = null;
                Wall? nextWall = null;
                bool passedCurrentlySelected = false;
                foreach (var wall in _walls
                    .Where(w => w.Boundary.Contains(worldX, worldY))
                    .OrderBy(w => w.Boundary.WorldY + (w.Boundary.WorldHeight / 2))
                    .ThenBy(w => w.Boundary.WorldX + (w.Boundary.WorldWidth / 2))
                    .ThenBy(w => w.InternalId))
                {
                    if (firstWall == null)
                    {
                        // list is ordered by highest to least priority
                        firstWall = wall;
                    }

                    if (currentlySelectedWall != null && currentlySelectedWall.InternalId == wall.InternalId)
                    {
                        // found the currently selected wall
                        passedCurrentlySelected = true;
                        continue;
                    }

                    if(passedCurrentlySelected)
                    {
                        // found the wall immediately after the currently selected one
                        nextWall = wall;
                        break;
                    }
                }

                if (nextWall != null)
                {
                    return nextWall;
                }

                if (firstWall != null)
                {
                    return firstWall;
                }

                return null;
            }
            else
            {
                return _parent.SelectWall(worldX, worldY, currentlySelectedWall);
            }
        }

        public void RemoveWall(string internalId)
        {
            if (_parent == null)
            {
                for (int i = _walls.Count - 1; i >= 0; i--)
                {
                    if (_walls[i].InternalId == internalId)
                    {
                        ToggleChanged(true);
                        ChangeHistory.TrackChange(new RemoveWallChange
                        {
                            Wall = _walls[i]
                        });
                        _walls.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                _parent.RemoveWall(internalId);
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

        public Entity? GetEntity(string internalId)
        {
            if (_parent == null)
            {
                for (int i = 0; i < _entities.Count; i++)
                {
                    if (_entities[i].InternalId == internalId)
                    {
                        return _entities[i];
                    }
                }

                return null;
            }
            else
            {
                return _parent.GetEntity(internalId);
            }
        }

        public Entity AddEntity(Entity entity)
        {
            if (_parent == null)
            {
                ToggleChanged(true);
                if (entity.InternalId == "")
                    entity.InternalId = System.Guid.NewGuid().ToString();
                ChangeHistory.TrackChange(new AddEnityChange
                {
                    InternalId = entity.InternalId
                });
                _entities.Add(entity);

                return entity;
            }
            else
            {
                return _parent.AddEntity(entity);
            }
        }

        public Entity? SelectEntity(float worldX, float worldY, Entity? currentlySelectedEntity)
        {
            if(_parent == null)
            {
                Entity? firstEntity = null;
                Entity? nextEntity = null;
                bool passedCurrentlySelected = false;
                foreach (var entity in _entities
                    .Where(e => Math.Sqrt(Math.Pow(e.WorldX - worldX, 2) + Math.Pow(e.WorldY - worldY, 2)) <= 16)
                    .OrderBy(e => e.WorldY)
                    .ThenBy(e => e.WorldX)
                    .ThenBy(e => e.InternalId))
                {
                    if (firstEntity == null)
                    {
                        // list is ordered by highest to least priority
                        firstEntity = entity;
                    }

                    if (currentlySelectedEntity != null && currentlySelectedEntity.InternalId == entity.InternalId)
                    {
                        // found the currently selected wall
                        passedCurrentlySelected = true;
                        continue;
                    }

                    if (passedCurrentlySelected)
                    {
                        // found the wall immediately after the currently selected one
                        nextEntity = entity;
                        break;
                    }
                }

                if(nextEntity != null)
                {
                    return nextEntity;
                }

                if(firstEntity != null)
                {
                    return firstEntity;
                }

                return null;
            }
            else
            {
                return _parent.SelectEntity(worldX, worldY, currentlySelectedEntity);
            }
        }

        public void RemoveEntity(string internalId)
        {
            if (_parent == null)
            {
                for (int i = _entities.Count - 1; i >= 0; i--)
                {
                    if (_entities[i].InternalId == internalId)
                    {
                        ToggleChanged(true);
                        ChangeHistory.TrackChange(new RemoveEntityChange
                        {
                            Entity = _entities[i]
                        });
                        _entities.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                _parent.RemoveEntity(internalId);
            }
        }

        public Tile? PushTile(Tile tileData, int tileX, int tileY)
        {
            if (!_boundary.Contains(tileX, tileY))
            {
                return null;
            }
            else if (IsSplit())
            {
                Tile? result = null;
                result = _bottomLeft?.PushTile(tileData, tileX, tileY);
                if (result != null)
                    return result;

                result = _bottomRight?.PushTile(tileData, tileX, tileY);
                if (result != null)
                    return result;

                result = _topLeft?.PushTile(tileData, tileX, tileY);
                if (result != null)
                    return result;

                result = _topRight?.PushTile(tileData, tileX, tileY);
                if (result != null)
                    return result;
            }
            else if (_tileData != null)
            {
                var tileXIndex = tileX - _boundary.TileX;
                var tileYIndex = tileY - _boundary.TileY;
                if (!_tileData[tileXIndex, tileYIndex].Same(tileData))
                {
                    ChangeHistory.TrackChange(new TileChange
                    {
                        TileX = tileX,
                        TileY = tileY,
                        DrawableIds = _tileData[tileXIndex, tileYIndex].DrawableIds
                    });

                    ToggleChanged(true);
                    _tileData[tileXIndex, tileYIndex] = tileData;
                }
                return _tileData[tileXIndex, tileYIndex];
            }

            return null;
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
                    var wallX = camera.ViewX + camera.WorldToView(wall.Boundary.WorldX);
                    var wallY = camera.ViewY + camera.WorldToView(wall.Boundary.WorldY);
                    var wallWidth = camera.WorldToView(wall.Boundary.WorldWidth);
                    var wallHeight = camera.WorldToView(wall.Boundary.WorldHeight);
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
