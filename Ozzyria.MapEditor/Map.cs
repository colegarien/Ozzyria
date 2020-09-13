using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzyria.MapEditor
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileDimension { get; set; } = 32;

        public Dictionary<int, Layer> layers;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;

            layers = new Dictionary<int, Layer>();
            layers[0] = new Layer(width, height);
        }

        public TileType GetTileType(int layer, int x, int y)
        {
            if (!layers.ContainsKey(layer))
            {
                return TileType.None;
            }

            return layers[layer].GetTileType(x, y);
        }

        public void SetTileType(int layer, int x, int y, TileType type)
        {
            if (!layers.ContainsKey(layer))
            {
                return;
            }

            layers[layer].SetTileType(x, y, type);
        }
    }
}
