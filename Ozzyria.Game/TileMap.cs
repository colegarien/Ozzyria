using System.Collections.Generic;

namespace Ozzyria.Game
{
    public class Tile
    {
        public const int DIMENSION = 32;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int TextureCoordX { get; set; } = 0;
        public int TextureCoordY { get; set; } = 0;
    }

    public class TileMap
    {
        // TODO make these not public
        public int width = 32;
        public int height = 32;

        public IDictionary<int, List<Tile>> layers;

        public TileMap()
        {
            layers = new Dictionary<int, List<Tile>>();
            layers[0] = new List<Tile>();
            layers[1] = new List<Tile>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var backgroundTile = new Tile { X = x, Y = y };

                    var makeWall = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                    if (makeWall)
                    {
                        backgroundTile.TextureCoordX = 0;
                        backgroundTile.TextureCoordY = 1;

                        layers[1].Add(new Tile
                        {
                            X = x,
                            Y = y,
                            TextureCoordX = WallX(x, y, width, height),
                            TextureCoordY = WallY(x, y, width, height)
                        });
                    }

                    layers[0].Add(backgroundTile);
                }
            }
        }

        // TODO remove these once actually have map files
        private int WallX(int x, int y, int width, int height)
        {
            if (x == 0 && y == 0)
            {
                // top-left
                return 4;
            }
            else if(x == 0 && y == height - 1){
                // bottom-left
                return 4;
            }
            else if (x == width - 1 && y == 0)
            {
                // top-right
                return 5;
            }
            else if (x == width - 1 && y == height - 1)
            {
                // bottom-right
                return 5;
            }
            else if(x == 0 || x == width - 1)
            {
                // up-down edge
                return 6;
            }
            else if(y == 0 || y == height - 1){
                // left-right edge
                return 5;
            }

            return -1;
        }
        private int WallY(int x, int y, int width, int height)
        {
            if (x == 0 && y == 0)
            {
                // top-left
                return 1;
            }
            else if (x == 0 && y == height - 1)
            {
                // bottom-left
                return 2;
            }
            else if (x == width - 1 && y == 0)
            {
                // top-right
                return 1;
            }
            else if (x == width - 1 && y == height - 1)
            {
                // bottom-right
                return 2;
            }
            else if (x == 0 || x == width - 1)
            {
                // up-down edge
                return 2;
            }
            else if (y == 0 || y == height - 1)
            {
                // left-right edge
                return 0;
            }

            return -1;
        }
    }
}
