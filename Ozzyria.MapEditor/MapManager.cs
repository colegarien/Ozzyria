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
        protected static TileMetaData _tileMetaData;

        public static bool MapIsLoaded()
        {
            return _map != null;
        }

        public static void LoadMap(Map map)
        {
            _map = map;
            _tileMetaData = new TileMetaData(); // TODO OZ-18 load from file

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
                        // TODO OZ-19 : instead of all this white noise, simpify with Edge and Corner Transistion (simple math since it's binary encoded!!!)

                        // reset before recalculating
                        _map.SetTransitionType(layer, x, y, TransitionType.None);
                        _map.SetPathDirection(layer, x, y, PathDirection.None);

                        var tileType = GetTileType(layer, x, y);
                        if (_tileMetaData.IsTransitionable(tileType))
                        {
                            var leftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x - 1, y));
                            var rightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x + 1, y));
                            var upIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x, y - 1));
                            var downIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x, y + 1));

                            var upLeftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x - 1, y - 1));
                            var upRightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x + 1, y - 1));
                            var downLeftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x - 1, y + 1));
                            var downRightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x + 1, y + 1));

                            if (upIsTransitionable && leftIsTransitionable && upLeftIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpLeft);
                            }
                            else if (upIsTransitionable && rightIsTransitionable && upRightIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpRight);
                            }
                            else if (downIsTransitionable && leftIsTransitionable && downLeftIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownLeft);
                            }
                            else if (downIsTransitionable && rightIsTransitionable && downRightIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownRight);
                            }
                            else if (upIsTransitionable && (upRightIsTransitionable || upLeftIsTransitionable))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Up);
                            }
                            else if (downIsTransitionable && (downLeftIsTransitionable || downRightIsTransitionable))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Down);
                            }
                            else if (leftIsTransitionable && (upLeftIsTransitionable || downLeftIsTransitionable))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Left);
                            }
                            else if (rightIsTransitionable && (upRightIsTransitionable || downRightIsTransitionable))
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.Right);
                            }
                            else if (!downIsTransitionable && !rightIsTransitionable && downRightIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownRightDiagonal);
                            }
                            else if (!downIsTransitionable && !leftIsTransitionable && downLeftIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.DownLeftDiagonal);
                            }
                            else if (!upIsTransitionable && !leftIsTransitionable && upLeftIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpLeftDiagonal);
                            }
                            else if (!upIsTransitionable && !leftIsTransitionable && upRightIsTransitionable)
                            {
                                _map.SetTransitionType(layer, x, y, TransitionType.UpRightDiagonal);
                            }
                        }
                        else if (_tileMetaData.IsPathable(tileType))
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
                        if (tileType == TileType.None)
                            continue;

                        var textureCoordinates = _tileMetaData.GetTextureCoordinates(
                            tileType,
                            GetTransitionType(layer, x, y),
                            GetPathDirection(layer, x, y)
                        );
                        var z = _tileMetaData.GetZIndex(tileType);

                        layers[layer].Add(new Game.Tile
                        {
                            X = x,
                            Y = y,
                            Z = z,
                            TextureCoordX = textureCoordinates.X,
                            TextureCoordY = textureCoordinates.Y
                        });
                    }
                }
            }

            var tileMap = new TileMap
            {
                Width = _map.Width,
                Height = _map.Height,
                Layers = layers
            };

            var worldLoader = new WorldPersistence();
            worldLoader.SaveMap("test_m", tileMap);

            var entityManager = new EntityManager();

            entityManager.Register(EntityFactory.CreateExperienceOrb(400, 300, 30));
            entityManager.Register(EntityFactory.CreateSlimeSpawner(500, 400));
            entityManager.Register(EntityFactory.CreateCircleCollider(60, 60, 10));

            // wrap screen in border
            entityManager.Register(EntityFactory.CreateBoxCollider(400, 20, 900, 10));
            entityManager.Register(EntityFactory.CreateBoxCollider(400, 510, 900, 10));
            entityManager.Register(EntityFactory.CreateBoxCollider(20, 300, 10, 700));
            entityManager.Register(EntityFactory.CreateBoxCollider(780, 300, 10, 700));

            entityManager.Register(EntityFactory.CreateBoxCollider(150, 300, 400, 10));
            entityManager.Register(EntityFactory.CreateBoxCollider(200, 300, 10, 300));

            worldLoader.SaveEntityManager("test_e", entityManager);
        }

        public static void PaintTile(int layer, int x, int y, TileType type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
            BakeMap();
        }

        public static void FillTile(int layer, int x, int y, TileType type)
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

        private static void FillRecursive(int layer, int x, int y, TileType type, TileType toFill)
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

        public static TileType GetTileType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return TileType.None;
            }

            return _map.GetTileType(layer, x, y);
        }

        public static TransitionType GetTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return TransitionType.None;
            }

            return _map.GetTransitionType(layer, x, y);
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

    }
}
