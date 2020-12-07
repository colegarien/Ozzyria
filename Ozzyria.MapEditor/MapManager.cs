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
                        // reset before recalculating
                        _map.SetEdgeTransitionType(layer, x, y, EdgeTransitionType.None);
                        _map.SetCornerTransitionType(layer, x, y, CornerTransitionType.None);
                        _map.SetPathDirection(layer, x, y, PathDirection.None);

                        var tileType = GetTileType(layer, x, y);
                        if (_tileMetaData.IsTransitionable(tileType))
                        {
                            /* 
                             * TODO OZ-19 : implement this rough pseudo-code:
                             * 
                             * 1. get surround tile types that support transition
                             * 2. narrow the list down further with only tiles that will transition INTO this tile (i.e. based on transition precedence, i.e. grass can tranistion INTO teh water tile, but water tiles to not render transitions INTO the grass tile)
                             * 3. caculate the Transition Types by each of the TileTypes that transtion INTO this tile
                             * 4. Update SaveMap() to add all the transitions based on precendence (i.e. if multiple different tile types transition into this tile then ensure they transition stack right, for example grass transition render below a foret transition etc etc)
                             */


                            // This works via 'bit-mask' math, the enum is very particularlly crafted
                            var leftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x - 1, y));
                            var rightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x + 1, y));
                            var upIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x, y - 1));
                            var downIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x, y + 1));
                            var edgeTransition = (int)EdgeTransitionType.None;
                            if (upIsTransitionable)
                                edgeTransition += (int)EdgeTransitionType.Up;
                            if (downIsTransitionable)
                                edgeTransition += (int)EdgeTransitionType.Down;
                            if (leftIsTransitionable)
                                edgeTransition += (int)EdgeTransitionType.Left;
                            if (rightIsTransitionable)
                                edgeTransition += (int)EdgeTransitionType.Right;
                            _map.SetEdgeTransitionType(layer, x, y, (EdgeTransitionType)edgeTransition);

                            // This works via 'bit-mask' math, the enum is very particularlly crafted 
                            // TODO OZ-19 : don't redundantly add corner transitions aready covered by edges, might add 'EdgeTransition' as a param... or maybe this isnt worth bothering with?
                            var upLeftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x - 1, y - 1));
                            var upRightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x + 1, y - 1));
                            var downLeftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x - 1, y + 1));
                            var downRightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, GetTileType(layer, x + 1, y + 1));
                            var cornerTransition = (int)CornerTransitionType.None;
                            if (upLeftIsTransitionable)
                                cornerTransition += (int)CornerTransitionType.UpLeft;
                            if (upRightIsTransitionable)
                                cornerTransition += (int)CornerTransitionType.UpRight;
                            if (downLeftIsTransitionable)
                                cornerTransition += (int)CornerTransitionType.DownLeft;
                            if (downRightIsTransitionable)
                                cornerTransition += (int)CornerTransitionType.DownRight;
                            _map.SetCornerTransitionType(layer, x, y, (CornerTransitionType)cornerTransition);
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
                        
                        // TODO OZ-19 Instead of stacking tiles on top of eachother re-work this to add a 'decals' thing to tiles
                        var cornerTransition = GetCornerTransitionType(layer, x, y);
                        if (cornerTransition != CornerTransitionType.None)
                        {
                            // TODO OZ-19 Stop hard-coding GROUND, might require storing multiple transition types for each type of material?
                            textureCoordinates = _tileMetaData.GetCornerTransitionTextureCoordinates(TileType.Ground, cornerTransition);
                            layers[layer].Add(new Game.Tile
                            {
                                X = x,
                                Y = y,
                                Z = z,
                                TextureCoordX = textureCoordinates.X,
                                TextureCoordY = textureCoordinates.Y
                            });
                        }
                        var edgeTransition = GetEdgeTransitionType(layer, x, y);
                        if (edgeTransition != EdgeTransitionType.None)
                        {
                            // TODO OZ-19 Stop hard-coding GROUND, might require storing multiple transition types for each type of material?
                            textureCoordinates = _tileMetaData.GetEdgeTransitionTextureCoordinates(TileType.Ground, edgeTransition);
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

        public static EdgeTransitionType GetEdgeTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return EdgeTransitionType.None;
            }

            return _map.GetEdgeTransitionType(layer, x, y);
        }

        public static CornerTransitionType GetCornerTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return CornerTransitionType.None;
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

    }
}
