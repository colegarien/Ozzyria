using Ozzyria.Game;
using Ozzyria.Game.Persistence;
using Ozzyria.Game.Utility;
using Ozzyria.MapEditor.EventSystem;
using System.Collections.Generic;

namespace Ozzyria.MapEditor
{
    class MapManager
    {
        protected static Map _map;
        protected static TileSetMetaDataFactory _tileSetMetaData;

        public static bool MapIsLoaded()
        {
            return _map != null;
        }

        public static void LoadMetaData()
        {
            if (_tileSetMetaData == null)
            {
                _tileSetMetaData = new TileSetMetaDataFactory();
            }
        }

        public static void LoadMap(Map map)
        {
            _map = map;
            LoadMetaData();
            _tileSetMetaData.SetCurrentTileSet(map.TileSet);

            EventQueue.Queue(new MapLoadedEvent
            {
                TileDimension = _map.TileDimension,
                Width = _map.Width,
                Height = _map.Height,
                NumberOfLayers = _map.layers.Count
            });
        }

        public static void BakeMap()
        {
            for (var layer = 0; layer < GetNumberOfLayers(); layer++)
            {
                for (var x = 0; x < _map.Width; x++)
                {
                    for (var y = 0; y < _map.Height; y++)
                    {
                        // reset before recalculating
                        var edgeTransitions = new Dictionary<int, EdgeTransitionType>();
                        var cornerTransitions = new Dictionary<int, CornerTransitionType>();
                        _map.SetPathDirection(layer, x, y, PathDirection.None);

                        var tileType = GetTileType(layer, x, y);
                        if (_tileSetMetaData.IsPathable(tileType))
                        {
                            var leftIsPath = GetTileType(layer, x - 1, y) == tileType;
                            var rightIsPath = GetTileType(layer, x + 1, y) == tileType;
                            var upIsPath = GetTileType(layer, x, y - 1) == tileType;
                            var downIsPath = GetTileType(layer, x, y + 1) == tileType;

                            if (leftIsPath && !rightIsPath && !upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Left);
                            }
                            else if (!leftIsPath && rightIsPath && !upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Right);
                            }
                            else if (!leftIsPath && !rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Up);
                            }
                            else if (!leftIsPath && !rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.Down);
                            }
                            else if (leftIsPath && rightIsPath && !upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.LeftRight);
                            }
                            else if (leftIsPath && rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpT);
                            }
                            else if (leftIsPath && rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.DownT);
                            }
                            else if (!leftIsPath && !rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpDown);
                            }
                            else if (leftIsPath && !rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.LeftT);
                            }
                            else if (!leftIsPath && rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.RightT);
                            }
                            else if (leftIsPath && !rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpLeft);
                            }
                            else if (!leftIsPath && rightIsPath && upIsPath && !downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.UpRight);
                            }
                            else if (!leftIsPath && rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.DownRight);
                            }
                            else if (leftIsPath && !rightIsPath && !upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.DownLeft);
                            }
                            else if (leftIsPath && rightIsPath && upIsPath && downIsPath)
                            {
                                _map.SetPathDirection(layer, x, y, PathDirection.All);
                            }
                        }
                        else
                        {
                            // This works via 'bit-mask' math, the enum is very particularlly crafted
                            var leftTileType = GetTileType(layer, x - 1, y);
                            var leftIsTransitionable = _tileSetMetaData.CanTransition(tileType, leftTileType);

                            var rightTileType = GetTileType(layer, x + 1, y);
                            var rightIsTransitionable = _tileSetMetaData.CanTransition(tileType, rightTileType);

                            var upTileType = GetTileType(layer, x, y - 1);
                            var upIsTransitionable = _tileSetMetaData.CanTransition(tileType, upTileType);

                            var downTileType = GetTileType(layer, x, y + 1);
                            var downIsTransitionable = _tileSetMetaData.CanTransition(tileType, downTileType);

                            if (upIsTransitionable)
                            {
                                if (!edgeTransitions.ContainsKey(upTileType))
                                    edgeTransitions[upTileType] = EdgeTransitionType.None;
                                edgeTransitions[upTileType] = (EdgeTransitionType)((int)edgeTransitions[upTileType] + (int)EdgeTransitionType.Up);
                            }
                            if (downIsTransitionable)
                            {
                                if (!edgeTransitions.ContainsKey(downTileType))
                                    edgeTransitions[downTileType] = EdgeTransitionType.None;
                                edgeTransitions[downTileType] = (EdgeTransitionType)((int)edgeTransitions[downTileType] + (int)EdgeTransitionType.Down);
                            }
                            if (leftIsTransitionable)
                            {
                                if (!edgeTransitions.ContainsKey(leftTileType))
                                    edgeTransitions[leftTileType] = EdgeTransitionType.None;
                                edgeTransitions[leftTileType] = (EdgeTransitionType)((int)edgeTransitions[leftTileType] + (int)EdgeTransitionType.Left);
                            }
                            if (rightIsTransitionable)
                            {
                                if (!edgeTransitions.ContainsKey(rightTileType))
                                    edgeTransitions[rightTileType] = EdgeTransitionType.None;
                                edgeTransitions[rightTileType] = (EdgeTransitionType)((int)edgeTransitions[rightTileType] + (int)EdgeTransitionType.Right);
                            }

                            // This works via 'bit-mask' math, the enum is very particularlly crafted 
                            // TODO : Consider adding checks for current edges transition to avoid rednundantly adding corner transitions on top of them 
                            var upLeftTileType = GetTileType(layer, x - 1, y - 1);
                            var upLeftIsTransitionable = _tileSetMetaData.CanTransition(tileType, upLeftTileType);

                            var upRightTileType = GetTileType(layer, x + 1, y - 1);
                            var upRightIsTransitionable = _tileSetMetaData.CanTransition(tileType, upRightTileType);

                            var downLeftTileType = GetTileType(layer, x - 1, y + 1);
                            var downLeftIsTransitionable = _tileSetMetaData.CanTransition(tileType, downLeftTileType);

                            var downRightTileType = GetTileType(layer, x + 1, y + 1);
                            var downRightIsTransitionable = _tileSetMetaData.CanTransition(tileType, downRightTileType);

                            if (upLeftIsTransitionable)
                            {
                                if (!cornerTransitions.ContainsKey(upLeftTileType))
                                    cornerTransitions[upLeftTileType] = CornerTransitionType.None;
                                cornerTransitions[upLeftTileType] = (CornerTransitionType)((int)cornerTransitions[upLeftTileType] + (int)CornerTransitionType.UpLeft);
                            }
                            if (upRightIsTransitionable)
                            {
                                if (!cornerTransitions.ContainsKey(upRightTileType))
                                    cornerTransitions[upRightTileType] = CornerTransitionType.None;
                                cornerTransitions[upRightTileType] = (CornerTransitionType)((int)cornerTransitions[upRightTileType] + (int)CornerTransitionType.UpRight);
                            }
                            if (downLeftIsTransitionable)
                            {
                                if (!cornerTransitions.ContainsKey(downLeftTileType))
                                    cornerTransitions[downLeftTileType] = CornerTransitionType.None;
                                cornerTransitions[downLeftTileType] = (CornerTransitionType)((int)cornerTransitions[downLeftTileType] + (int)CornerTransitionType.DownLeft);
                            }
                            if (downRightIsTransitionable)
                            {
                                if (!cornerTransitions.ContainsKey(downRightTileType))
                                    cornerTransitions[downRightTileType] = CornerTransitionType.None;
                                cornerTransitions[downRightTileType] = (CornerTransitionType)((int)cornerTransitions[downRightTileType] + (int)CornerTransitionType.DownRight);
                            }
                        }

                        _map.SetEdgeTransitionType(layer, x, y, edgeTransitions);
                        _map.SetCornerTransitionType(layer, x, y, cornerTransitions);
                    }
                }
            }
        }

        public static void SaveMap()
        {
            if (!MapIsLoaded())
            {
                return;
            }

            var layers = new Dictionary<int, List<Game.Tile>>();
            for (var layer = 0; layer < GetNumberOfLayers(); layer++)
            {
                layers[layer] = new List<Game.Tile>();
                for (var x = 0; x < _map.Width; x++)
                {
                    for (var y = 0; y < _map.Height; y++)
                    {
                        var tileType = GetTileType(layer, x, y);
                        if (tileType == 0)
                            continue;

                        var textureCoordinates = _tileSetMetaData.GetTextureCoordinates(
                            tileType,
                            GetPathDirection(layer, x, y)
                        );
                        var z = _tileSetMetaData.GetZIndex(tileType);

                        layers[layer].Add(new Game.Tile
                        {
                            X = x,
                            Y = y,
                            Z = z,
                            TextureCoordX = textureCoordinates.X,
                            TextureCoordY = textureCoordinates.Y,
                            Decals = BuildTileDecals(layer, x, y)
                        });
                    }
                }
            }

            var tileMap = new TileMap
            {
                Name = _map.Name,
                TileSet = _map.TileSet,
                Width = _map.Width,
                Height = _map.Height,
                Layers = layers
            };

            var worldLoader = new WorldPersistence();
            worldLoader.SaveMap(_map.Name, tileMap);

            var entityManager = new EntityManager();

            entityManager.Register(EntityFactory.CreateExperienceOrb(400, 300, 30));
            entityManager.Register(EntityFactory.CreateSlimeSpawner(500, 400));
            entityManager.Register(EntityFactory.CreateCircleCollider(60, 60, 10));

            // build colliders for collideable tiles
            for (var layer = 0; layer < GetNumberOfLayers(); layer++)
            {
                layers[layer] = new List<Game.Tile>();

                // build vertical collisions
                for (var x = 0; x < _map.Width; x++)
                {
                    int startType = -1;
                    int startY = -1;
                    int endY = -1;
                    for (var y = 0; y < _map.Height; y++)
                    {
                        var tileType = GetTileType(layer, x, y);
                        if (_tileSetMetaData.IsWallable(tileType))
                        {
                            var direction = GetPathDirection(layer, x, y);
                            if (direction == PathDirection.All
                                || direction == PathDirection.Down
                                || direction == PathDirection.DownLeft
                                || direction == PathDirection.DownRight
                                || direction == PathDirection.DownT
                                || direction == PathDirection.UpT
                                || direction == PathDirection.LeftT
                                || direction == PathDirection.RightT
                                || direction == PathDirection.Up
                                || direction == PathDirection.UpDown
                                || direction == PathDirection.UpLeft
                                || direction == PathDirection.UpRight)
                            {
                                if (tileType != startType && startY != -1 && endY != -1)
                                {
                                    // starting new string of wall types (collision could be a different size)
                                    entityManager.Register(CreateVerticalBoxCollider(startType, x, startY, endY));
                                    startType = -1;
                                    startY = -1;
                                    endY = -1;
                                }

                                if (startY == -1)
                                {
                                    startType = tileType;
                                    startY = y;
                                }
                                endY = y;
                            }
                            else if (startY != -1 && endY != -1)
                            {
                                entityManager.Register(CreateVerticalBoxCollider(startType, x, startY, endY));
                                startType = -1;
                                startY = -1;
                                endY = -1;
                            }
                        }
                        else
                        {
                            if (startY != -1 && endY != -1)
                            {
                                entityManager.Register(CreateVerticalBoxCollider(startType, x, startY, endY));
                            }

                            startType = -1;
                            startY = -1;
                            endY = -1;
                        }
                    }

                    if (startY != -1 && endY != -1)
                    {
                        entityManager.Register(CreateVerticalBoxCollider(startType, x, startY, endY));
                    }
                }

                // build horizontal collisions
                for (var y = 0; y < _map.Height; y++)
                {
                    int startType = -1;
                    int startX = -1;
                    int endX = -1;
                    for (var x = 0; x < _map.Width; x++)
                    {
                        var tileType = GetTileType(layer, x, y);
                        if (tileType == 3 || tileType == 6)
                        {
                            var direction = GetPathDirection(layer, x, y);
                            if (direction == PathDirection.None
                                || direction == PathDirection.All
                                || direction == PathDirection.Left
                                || direction == PathDirection.DownLeft
                                || direction == PathDirection.DownRight
                                || direction == PathDirection.DownT
                                || direction == PathDirection.UpT
                                || direction == PathDirection.LeftT
                                || direction == PathDirection.RightT
                                || direction == PathDirection.Right
                                || direction == PathDirection.LeftRight
                                || direction == PathDirection.UpLeft
                                || direction == PathDirection.UpRight)
                            {
                                if (tileType != startType && startX != -1 && endX != -1)
                                {
                                    // starting new string of wall types (collision could be a different size)
                                    entityManager.Register(CreateHorizontalBoxCollider(startType, y, startX, endX));
                                    startType = -1;
                                    startX = -1;
                                    endX = -1;
                                }

                                if (startX == -1)
                                {
                                    startType = tileType;
                                    startX = x;
                                }
                                endX = x;
                            }
                            else if (startX != -1 && endX != -1)
                            {
                                entityManager.Register(CreateHorizontalBoxCollider(startType, y, startX, endX));
                                startType = -1;
                                startX = -1;
                                endX = -1;
                            }
                        }
                        else
                        {
                            if (startX != -1 && endX != -1)
                            {
                                entityManager.Register(CreateHorizontalBoxCollider(startType, y, startX, endX));
                            }

                            startType = -1;
                            startX = -1;
                            endX = -1;
                        }
                    }

                    if (startX != -1 && endX != -1)
                    {
                        entityManager.Register(CreateHorizontalBoxCollider(startType, y, startX, endX));
                    }
                }
            }
            worldLoader.SaveEntityManager("test_e", entityManager);
        }

        private static Entity CreateVerticalBoxCollider(int tileType, int x, int startY, int endY)
        {
            var tileDimension = GetTileDimension();
            var centerXOffset = _tileSetMetaData.GetWallableCenterXOffset(tileType);
            var centerYOffset = _tileSetMetaData.GetWallableCenterYOffset(tileType);
            var colliderDimension = _tileSetMetaData.GetWallableThickness(tileType);

            var left = x * tileDimension;
            var tileCenterX = left + (tileDimension / 2);
            var centerLeft = tileCenterX + centerXOffset - (colliderDimension / 2);
            var centerRight = centerLeft + colliderDimension;

            var startTop = ((startY * tileDimension) + (tileDimension / 2)) + centerYOffset - (colliderDimension / 2);
            var endBottom = (((endY * tileDimension) + (tileDimension / 2)) + centerYOffset - (colliderDimension / 2)) + colliderDimension;

            return EntityFactory.CreateBoxColliderArea(centerLeft, startTop, centerRight, endBottom);
        }

        private static Entity CreateHorizontalBoxCollider(int tileType, int y, int startX, int endX)
        {
            var tileDimension = GetTileDimension();
            var centerXOffset = _tileSetMetaData.GetWallableCenterXOffset(tileType);
            var centerYOffset = _tileSetMetaData.GetWallableCenterYOffset(tileType);
            var colliderDimension = _tileSetMetaData.GetWallableThickness(tileType);

            var top = y * tileDimension;
            var tileCenterY = top + (tileDimension / 2);
            var centerTop = tileCenterY + centerYOffset - (colliderDimension / 2);
            var centerBottom = centerTop + colliderDimension;

            var startLeft = (((startX * tileDimension) + (tileDimension / 2)) + centerXOffset - (colliderDimension / 2));
            var endRight = (((endX * tileDimension) + (tileDimension / 2)) + centerXOffset - (colliderDimension / 2)) + colliderDimension;

            return EntityFactory.CreateBoxColliderArea(startLeft, centerTop, endRight, centerBottom);
        }

        public static TileDecal[] BuildTileDecals(int layer, int x, int y)
        {
            var decals = new List<TileDecal>();

            var cornerTransitions = GetCornerTransitionType(layer, x, y);
            var edgeTransitions = GetEdgeTransitionType(layer, x, y);

            var transitionTypesByPrecedence = _tileSetMetaData.GetTransitionTypesPrecedenceAscending();
            foreach (var transitionTileType in transitionTypesByPrecedence)
            {
                if (cornerTransitions.ContainsKey(transitionTileType))
                {
                    var cornerTransition = cornerTransitions[transitionTileType];
                    if (cornerTransition != CornerTransitionType.None)
                    {
                        var textureCoordinates = _tileSetMetaData.GetCornerTransitionTextureCoordinates(transitionTileType, cornerTransition);
                        decals.Add(new TileDecal
                        {
                            TextureCoordX = textureCoordinates.X,
                            TextureCoordY = textureCoordinates.Y
                        });
                    }
                }

                if (edgeTransitions.ContainsKey(transitionTileType))
                {
                    var edgeTransition = edgeTransitions[transitionTileType];
                    if (edgeTransition != EdgeTransitionType.None)
                    {
                        var textureCoordinates = _tileSetMetaData.GetEdgeTransitionTextureCoordinates(transitionTileType, edgeTransition);
                        decals.Add(new TileDecal
                        {
                            TextureCoordX = textureCoordinates.X,
                            TextureCoordY = textureCoordinates.Y
                        });
                    }
                }
            }

            return decals.ToArray();
        }

        public static void PaintTile(int layer, int x, int y, int type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
            BakeMap();
        }

        public static void FillTile(int layer, int x, int y, int type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            var currentTile = _map.GetTileType(layer, x, y);
            if (currentTile == type)
            {
                return;
            }

            FillRecursive(layer, x, y, type, currentTile);
            BakeMap();
        }

        private static void FillRecursive(int layer, int x, int y, int type, int toFill)
        {
            var currentTileType = _map.GetTileType(layer, x, y);
            if (x < 0 || x >= GetWidth() || y < 0 || y >= GetHeight() || currentTileType != toFill || currentTileType == type)
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
            FillRecursive(layer, x - 1, y, type, toFill);
            FillRecursive(layer, x + 1, y, type, toFill);
            FillRecursive(layer, x, y - 1, type, toFill);
            FillRecursive(layer, x, y + 1, type, toFill);
        }

        public static void AddLayer()
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.AddLayer();
        }

        public static void RemoveLayer(int layer)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.RemoveLayer(layer);
        }

        public static int GetTileDimension()
        {
            if (!MapIsLoaded())
            {
                return 1;
            }

            return _map.TileDimension;
        }

        public static int GetWidth()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.Width;
        }

        public static int GetHeight()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.Height;
        }

        public static int GetTileType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.GetTileType(layer, x, y);
        }

        public static IDictionary<int, EdgeTransitionType> GetEdgeTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return new Dictionary<int, EdgeTransitionType>();
            }

            return _map.GetEdgeTransitionType(layer, x, y);
        }

        public static IDictionary<int, CornerTransitionType> GetCornerTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return new Dictionary<int, CornerTransitionType>();
            }

            return _map.GetCornerTransitionType(layer, x, y);
        }

        public static PathDirection GetPathDirection(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return PathDirection.None;
            }

            return _map.GetPathDirection(layer, x, y);
        }

        public static int GetNumberOfLayers()
        {
            if (!MapIsLoaded())
            {
                return 0;
            }

            return _map.layers.Count;
        }

        public static int[] GetTileTypes()
        {
            LoadMetaData();
            return _tileSetMetaData.GetTypes();
        }

        public static TileSetMetaDataFactory GetTileSetMetaDataFactory(string tileSet)
        {
            LoadMetaData();
            _tileSetMetaData.SetCurrentTileSet(tileSet);
            return _tileSetMetaData;
        }
    }
}
