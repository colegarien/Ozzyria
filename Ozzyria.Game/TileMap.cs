using System.Collections.Generic;
using System.Linq;

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
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;

        public IDictionary<int, List<Tile>> Layers { get; set; } = new Dictionary<int, List<Tile>>();

        public bool HasLayer(int layer)
        {
            return Layers.ContainsKey(layer);
        }

        public IEnumerable<Tile> GetTilesInArea(int layer, int stripY, float minX, float minY, float maxX, float maxY)
        {
            if (!HasLayer(layer))
            {
                return System.Array.Empty<Tile>();
            }

            return Layers[layer].Where(t => t.Y == stripY && t.X * Tile.DIMENSION >= minX && t.X * Tile.DIMENSION <= maxX && t.Y * Tile.DIMENSION >= minY && t.Y * Tile.DIMENSION <= maxY);
        }
    }
}
