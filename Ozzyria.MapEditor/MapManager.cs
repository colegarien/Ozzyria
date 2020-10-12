using Ozzyria.MapEditor.EventSystem;
using System.Runtime.InteropServices.ComTypes;

namespace Ozzyria.MapEditor
{
    class MapManager
    {
        protected static Map _map;

        public static bool MapIsLoaded()
        {
            return _map != null;
        }

        public static void LoadMap(Map map)
        {
            _map = map;

            EventQueue.Queue(new MapLoadedEvent
            {
                TileDimension = _map.TileDimension,
                Width = _map.Width,
                Height = _map.Height,
                NumberOfLayers = _map.layers.Count
            });
        }

        public static void SaveMap()
        {
            if (!MapIsLoaded())
            {
                return;
            }

            if (!System.IO.Directory.Exists("Maps"))
            {
                System.IO.Directory.CreateDirectory("Maps"); // TODO probably wnat to be more tidy about this
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Maps\\test.ozz"))
            {
                file.WriteLine(_map.Width);
                file.WriteLine(_map.Height);
                file.WriteLine(GetNumberOfLayers());
                for (var layer = 0; layer < GetNumberOfLayers(); layer++)
                {
                    for (var x = 0; x < _map.Width; x++)
                    {
                        for (var y = 0; y < _map.Height; y++)
                        {
                            var tileType = GetTileType(layer, x, y);

                            var tx = 0;
                            var ty = 0;
                            if (tileType == TileType.None)
                            {
                                continue;
                            }
                            else if (tileType == TileType.Ground)
                            {
                                tx = 0;
                                ty = 0;
                            }
                            else if (tileType == TileType.Water)
                            {
                                var leftIsGround = GetTileType(layer, x - 1, y) == TileType.Ground;
                                var rightIsGround = GetTileType(layer, x + 1, y) == TileType.Ground;
                                var upIsGround = GetTileType(layer, x, y - 1) == TileType.Ground;
                                var downIsGround = GetTileType(layer, x, y + 1) == TileType.Ground;

                                var upLeftIsGround = GetTileType(layer, x - 1, y - 1) == TileType.Ground;
                                var upRightIsGround = GetTileType(layer, x + 1, y - 1) == TileType.Ground;
                                var downLeftIsGround = GetTileType(layer, x - 1, y + 1) == TileType.Ground;
                                var downRightIsGround = GetTileType(layer, x + 1, y + 1) == TileType.Ground;

                                // TODO genricify 'transition' connecting
                                if (upIsGround && leftIsGround && upLeftIsGround)
                                {
                                    // inner corner on top-left
                                    tx = 1;
                                    ty = 0;
                                }
                                else if (upIsGround && rightIsGround && upRightIsGround)
                                {
                                    // inner corner on top-right
                                    tx = 3;
                                    ty = 0;
                                }
                                else if (downIsGround && leftIsGround && downLeftIsGround)
                                {
                                    // inner corner on bottom-left
                                    tx = 1;
                                    ty = 2;
                                }
                                else if (downIsGround && rightIsGround && downRightIsGround)
                                {
                                    // inner corner on bottom-right
                                    tx = 3;
                                    ty = 2;
                                }
                                else if (upIsGround && (upRightIsGround || upLeftIsGround))
                                {
                                    // left right at top
                                    tx = 2;
                                    ty = 0;
                                }
                                else if (downIsGround && (downLeftIsGround || downRightIsGround))
                                {
                                    // left right at bottom
                                    tx = 2;
                                    ty = 2;
                                }
                                else if (leftIsGround && (upLeftIsGround || downLeftIsGround))
                                {
                                    // up odwn at left
                                    tx = 1;
                                    ty = 1;
                                }
                                else if (rightIsGround && (upRightIsGround || downRightIsGround))
                                {
                                    // up down at right
                                    tx = 3;
                                    ty = 1;
                                }
                                else if (!downIsGround && !rightIsGround && downRightIsGround)
                                {
                                    // outer corner on top-left
                                    tx = 2;
                                    ty = 3;
                                }
                                else if (!downIsGround && !leftIsGround && downLeftIsGround)
                                {
                                    // outer corner on top-right
                                    tx = 3;
                                    ty = 3;
                                }
                                else if (!upIsGround && !leftIsGround && upLeftIsGround)
                                {
                                    // outer corner on bottom-right
                                    tx = 3;
                                    ty = 4;
                                }
                                else if (!upIsGround && !leftIsGround && upRightIsGround)
                                {
                                    // outer corner on bottom-left
                                    tx = 2;
                                    ty = 4;
                                }
                                else
                                {
                                    // no transition
                                    tx = 0;
                                    ty = 1;
                                }
                            }
                            else if (tileType == TileType.Fence)
                            {
                                var leftIsFence = GetTileType(layer, x - 1, y) == TileType.Fence;
                                var rightIsFence = GetTileType(layer, x + 1, y) == TileType.Fence;
                                var upIsFence = GetTileType(layer, x, y - 1) == TileType.Fence;
                                var downIsFence = GetTileType(layer, x, y + 1) == TileType.Fence;
                                
                                // TODO genricify 'wall' connecting
                                if(leftIsFence && !rightIsFence && !upIsFence && !downIsFence)
                                {
                                    // tile to left
                                    tx = 6;
                                    ty = 3;
                                }
                                else if (!leftIsFence && rightIsFence && !upIsFence && !downIsFence)
                                {
                                    // tile to right
                                    tx = 7;
                                    ty = 3;
                                }
                                else if (!leftIsFence && !rightIsFence && upIsFence && !downIsFence)
                                {
                                    // tile to up
                                    tx = 4;
                                    ty = 3;
                                }
                                else if (!leftIsFence && !rightIsFence && !upIsFence && downIsFence)
                                {
                                    // tile to down
                                    tx = 5;
                                    ty = 3;
                                }
                                else if (leftIsFence && rightIsFence && !upIsFence && !downIsFence)
                                {
                                    // tile to left & right
                                    tx = 5;
                                    ty = 0;
                                }
                                else if (leftIsFence && rightIsFence && upIsFence && !downIsFence)
                                {
                                    // tile to left & right & up
                                    tx = 7;
                                    ty = 0;
                                }
                                else if (leftIsFence && rightIsFence && !upIsFence && downIsFence)
                                {
                                    // tile to left & right & down
                                    tx = 6;
                                    ty = 0;
                                }
                                else if (!leftIsFence && !rightIsFence && upIsFence && downIsFence)
                                {
                                    // tile to up & down
                                    tx = 6;
                                    ty = 2;
                                }
                                else if (leftIsFence && !rightIsFence && upIsFence && downIsFence)
                                {
                                    // tile to up & down & left
                                    tx = 7;
                                    ty = 2;
                                }
                                else if (!leftIsFence && rightIsFence && upIsFence && downIsFence)
                                {
                                    // tile to up & down & right
                                    tx = 7;
                                    ty = 1;
                                }
                                else if (leftIsFence && !rightIsFence && upIsFence && !downIsFence)
                                {
                                    // up & left
                                    tx = 5;
                                    ty = 2;
                                }
                                else if (!leftIsFence && rightIsFence && upIsFence && !downIsFence)
                                {
                                    // up & right
                                    tx = 4;
                                    ty = 2;
                                }
                                else if (!leftIsFence && rightIsFence && !upIsFence && downIsFence)
                                {
                                    // down & right
                                    tx = 4;
                                    ty = 1;
                                }
                                else if (leftIsFence && !rightIsFence && !upIsFence && downIsFence)
                                {
                                    // down & left
                                    tx = 5;
                                    ty = 1;
                                }
                                else if (leftIsFence && rightIsFence && upIsFence && downIsFence)
                                {
                                    // all adjacent tile
                                    tx = 4;
                                    ty = 0;
                                }
                                else
                                {
                                    // no adjacent tile
                                    tx = 6;
                                    ty = 1;
                                }
                            }
                            file.WriteLine($"{layer}|{x}|{y}|{tx}|{ty}");
                        }
                    }
                }
                file.WriteLine("END");
            }
        }

        public static void PaintTile(int layer, int x, int y, TileType type)
        {
            if (!MapIsLoaded())
            {
                return;
            }

            _map.SetTileType(layer, x, y, type);
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
