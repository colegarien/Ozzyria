using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MapEditor
{
    class Map
    {
        public string TileSet { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileDimension { get; set; } = 32;

        public Dictionary<int, Layer> layers;

        public Map(string tileSet, int width, int height)
        {
            TileSet = tileSet;
            Width = width;
            Height = height;

            layers = new Dictionary<int, Layer>();
            AddLayer();
        }

        public void AddLayer()
        {
            var newLayer = 0;
            if (layers.Keys.Count != 0)
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
            for (var i = layer + 1; i <= lastLayer; i++)
            {
                var currentLayer = layers[i];
                layers[i - 1] = currentLayer;
                layers.Remove(i);
            }
        }

        public int GetTileType(int layer, int x, int y)
        {
            if (!layers.ContainsKey(layer))
            {
                return 0;
            }

            return layers[layer].GetTileType(x, y);
        }

        public void SetTileType(int layer, int x, int y, int type)
        {
            if (!layers.ContainsKey(layer))
            {
                return;
            }

            layers[layer].SetTileType(x, y, type);
        }

        public IDictionary<int, EdgeTransitionType> GetEdgeTransitionType(int layer, int x, int y)
        {
            if (!layers.ContainsKey(layer))
            {
                return new Dictionary<int, EdgeTransitionType>();
            }

            return layers[layer].GetEdgeTransitions(x, y);
        }

        public void SetEdgeTransitionType(int layer, int x, int y, IDictionary<int, EdgeTransitionType> transitions)
        {
            if (!layers.ContainsKey(layer))
            {
                return;
            }

            if (transitions is null)
            {
                throw new System.ArgumentNullException(nameof(transitions));
            }

            layers[layer].SetEdgeTransitions(x, y, transitions);
        }

        public IDictionary<int, CornerTransitionType> GetCornerTransitionType(int layer, int x, int y)
        {
            if (!layers.ContainsKey(layer))
            {
                return new Dictionary<int, CornerTransitionType>();
            }

            return layers[layer].GetCornerTransitions(x, y);
        }

        public void SetCornerTransitionType(int layer, int x, int y, IDictionary<int, CornerTransitionType> transitions)
        {
            if (!layers.ContainsKey(layer))
            {
                return;
            }

            layers[layer].SetCornerTransitions(x, y, transitions);
        }

        public PathDirection GetPathDirection(int layer, int x, int y)
        {
            if (!layers.ContainsKey(layer))
            {
                return PathDirection.None;
            }

            return layers[layer].GetPathDirection(x, y);
        }

        public void SetPathDirection(int layer, int x, int y, PathDirection direction)
        {
            if (!layers.ContainsKey(layer))
            {
                return;
            }

            layers[layer].SetPathDirection(x, y, direction);
        }
    }
}
