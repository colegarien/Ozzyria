﻿using Ozzyria.Game;
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
                        var edgeTransitions = new Dictionary<TileType, EdgeTransitionType>();
                        var cornerTransitions = new Dictionary<TileType, CornerTransitionType>();
                        _map.SetPathDirection(layer, x, y, PathDirection.None);

                        var tileType = GetTileType(layer, x, y);
                        if (_tileMetaData.IsTransitionable(tileType))
                        {
                            // This works via 'bit-mask' math, the enum is very particularlly crafted
                            var leftTileType = GetTileType(layer, x - 1, y);
                            var leftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, leftTileType);

                            var rightTileType = GetTileType(layer, x + 1, y);
                            var rightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, rightTileType);

                            var upTileType = GetTileType(layer, x, y - 1);
                            var upIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, upTileType);

                            var downTileType = GetTileType(layer, x, y + 1);
                            var downIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, downTileType);

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
                            // TODO OZ-19 : don't redundantly add corner transitions aready covered by edges, might add 'EdgeTransition' as a param... or maybe this isnt worth bothering with?
                            var upLeftTileType = GetTileType(layer, x - 1, y - 1);
                            var upLeftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, upLeftTileType);

                            var upRightTileType = GetTileType(layer, x + 1, y - 1);
                            var upRightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, upRightTileType);

                            var downLeftTileType = GetTileType(layer, x - 1, y + 1);
                            var downLeftIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, downLeftTileType);

                            var downRightTileType = GetTileType(layer, x + 1, y + 1);
                            var downRightIsTransitionable = _tileMetaData.IsSupportedTransition(tileType, downRightTileType);

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

            // TODO OZ-19 :  Update SaveMap() to add all the transitions based on precendence (i.e. if multiple different tile types transition into this tile then ensure they transition stack right, for example grass transition render below a foret transition etc etc)
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
                        var cornerTransitions = GetCornerTransitionType(layer, x, y);
                        foreach (var kv in cornerTransitions)
                        {
                            var transitionTileType = kv.Key;
                            var cornerTransition = kv.Value;
                            if (cornerTransition != CornerTransitionType.None)
                            {
                                textureCoordinates = _tileMetaData.GetCornerTransitionTextureCoordinates(transitionTileType, cornerTransition);
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

                        var edgeTransitions = GetEdgeTransitionType(layer, x, y);
                        foreach (var kv in edgeTransitions)
                        {
                            var transitionTileType = kv.Key;
                            var edgeTransition = kv.Value;
                            if (edgeTransition != EdgeTransitionType.None)
                            {
                                textureCoordinates = _tileMetaData.GetEdgeTransitionTextureCoordinates(transitionTileType, edgeTransition);
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

        public static IDictionary<TileType, EdgeTransitionType> GetEdgeTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return new Dictionary<TileType, EdgeTransitionType>();
            }

            return _map.GetEdgeTransitionType(layer, x, y);
        }

        public static IDictionary<TileType, CornerTransitionType> GetCornerTransitionType(int layer, int x, int y)
        {
            if (!MapIsLoaded())
            {
                return new Dictionary<TileType, CornerTransitionType>();
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
