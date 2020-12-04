using System.Collections.Generic;

namespace Ozzyria.Game
{
    public class Tile
    {
        public const int DIMENSION = 32;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;
        public int TextureCoordX { get; set; } = 0;
        public int TextureCoordY { get; set; } = 0; // TODO OZ-19 add "Decals" array with x&y&rotation, update CreateTileGraphic to parse this ( first to massive number of decals on every tile to see performance)
    }

    public class TileMap
    {
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;

        public IDictionary<int, List<Tile>> Layers { get; set; } = new Dictionary<int, List<Tile>>();

        public bool HasLayer(int layer)
        {
            return Layers.ContainsKey(layer);
        }

        public IEnumerable<Tile> GetTiles(int layer)
        {
            if (!HasLayer(layer))
            {
                return System.Array.Empty<Tile>();
            }

            return Layers[layer];
        }
    }
}
