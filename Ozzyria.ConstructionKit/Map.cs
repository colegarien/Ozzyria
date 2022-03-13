using Ozzyria.Game;
using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ozzyria.ConstructionKit
{
    static class MapExtensions
    {
        public static void AddLayer(this TileMap map)
        {
            var newLayer = 0;
            if (map.Layers.Keys.Count != 0)
                newLayer = map.Layers.Keys.Max() + 1;

            if (newLayer >= 256)
                return;

            map.Layers[newLayer] = new List<Tile>();
        }

        public static void RemoveLayer(this TileMap map, int layer)
        {
            if (!map.Layers.ContainsKey(layer) || map.Layers.Keys.Count <= 1)
                return;

            var lastLayer = map.Layers.Keys.Max();
            map.Layers.Remove(layer);
            for (var i = layer + 1; i <= lastLayer; i++)
            {
                var currentLayer = map.Layers[i];
                map.Layers[i - 1] = currentLayer;
                map.Layers.Remove(i);
            }
        }

        public static void RemoveTile(this TileMap map, int layer, int x, int y)
        {
            if (map.HasLayer(layer))
                map.Layers[layer].RemoveAll(t => t.X == x && t.Y == y);
        }
    }

    class MapFactory
    {
        public static IDictionary<string, TileMap> loadedMaps = new Dictionary<string, TileMap>();
        public static string lastUsedMap = "";

        public static TileMap NewMap(string mapName)
        {
            loadedMaps[mapName] = new TileMap{ Name = mapName, };
            loadedMaps[mapName].AddLayer();

            lastUsedMap = mapName;
            return loadedMaps[mapName];
        }

        public static bool MapExists(string mapName)
        {
            return loadedMaps.ContainsKey(mapName) || File.Exists(Content.Loader.Root() + "/Maps/" + mapName + ".ozz");
        }

        public static void Reinitialize()
        {
            loadedMaps.Clear();
        }

        public static TileMap LoadMap(string mapName)
        {
            if (loadedMaps.ContainsKey(mapName))
            {
                return loadedMaps[mapName];
            }

            var persistence = new WorldPersistence();
            loadedMaps[mapName] = persistence.LoadMap(mapName);

            return loadedMaps[mapName];
        }

        public static void SaveMaps()
        {
            var persistence = new WorldPersistence();

            foreach(var mapGroup in loadedMaps)
            {
                persistence.SaveMap(mapGroup.Key, mapGroup.Value);
            }

            // TODO OZ-17 remove saved maps other than the last loaded one
        }
    }
}
