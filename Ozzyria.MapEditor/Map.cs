using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
            AddLayer();
            AddLayer();
        }

        public void AddLayer()
        {
            var newLayer = 0;
            if(layers.Keys.Count != 0)
            {
                newLayer = layers.Keys.Max() + 1;
            }
            layers[newLayer] = new Layer(Width, Height);
        }

        public void RemoveLayer(int layer)
        {
            if (!layers.ContainsKey(layer) || layers.Keys.Count <= 1)
            {
                return;
            }

            var lastLayer = layers.Keys.Max();
            layers.Remove(layer);
            for(var i = layer+1; i <= lastLayer; i++)
            {
                var currentLayer = layers[i];
                layers[i - 1] = currentLayer;
                layers.Remove(i);
            }
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
