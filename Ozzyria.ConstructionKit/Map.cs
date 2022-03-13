using Ozzyria.Game;
using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ozzyria.ConstructionKit
{
    class Map
    {
        // TODO OZ-17 what functionaality do I need from a map?
        public string Name { get; set; }
        public string TileSet { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IDictionary<int, List<Tile>> Layers { get; set; }

        public Map(string name, string tileSet, int width, int height, int layers=1)
        {
            Name = name;
            TileSet = tileSet;
            Width = width;
            Height = height;
            Layers = new Dictionary<int, List<Tile>>();

            for(int i = 0; i < layers; i++)
                AddLayer();
        }

        public void AddLayer()
        {
            var newLayer = 0;
            if (Layers.Keys.Count != 0)
                newLayer = Layers.Keys.Max() + 1;

            if (newLayer >= 256)
                return;

            Layers[newLayer] = new List<Tile>();
        }

        public void RemoveLayer(int layer)
        {
            if (!Layers.ContainsKey(layer) || Layers.Keys.Count <= 1)
                return;

            var lastLayer = Layers.Keys.Max();
            Layers.Remove(layer);
            for (var i = layer + 1; i <= lastLayer; i++)
            {
                var currentLayer = Layers[i];
                Layers[i - 1] = currentLayer;
                Layers.Remove(i);
            }
        }
    }

    class MapFactory
    {
        public static IDictionary<string, Map> loadedMaps = new Dictionary<string, Map>();
        public static string lastUsedMap = "";

        public static Map NewMap(string mapName)
        {
            loadedMaps[mapName] = new Map(mapName, "", 32, 32);
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

        public static Map LoadMap(string mapName)
        {
            if (loadedMaps.ContainsKey(mapName))
            {
                return loadedMaps[mapName];
            }

            var persistence = new WorldPersistence();
            var tileMap = persistence.LoadMap(mapName);

            loadedMaps[mapName] = new Map(mapName, tileMap.TileSet, tileMap.Width, tileMap.Height, tileMap.Layers.Keys.Max()+1);
            loadedMaps[mapName].Layers = tileMap.Layers;

            return loadedMaps[mapName];
        }

        public static void SaveMaps()
        {
            var persistence = new WorldPersistence();

            foreach(var mapGroup in loadedMaps)
            {
                var map = mapGroup.Value;
                var tileMap = new TileMap
                {
                    MapName = map.Name,
                    TileSet = map.TileSet,
                    Width = map.Width,
                    Height = map.Height
                };

                foreach(var layerGroup in map.Layers)
                {
                    tileMap.Layers[layerGroup.Key] = new List<Tile>();
                    foreach (var tile in layerGroup.Value)
                    {
                        if (tile.Decals == null)
                            continue;

                        tileMap.Layers[layerGroup.Key].Add(tile);
                    }
                }

                persistence.SaveMap(tileMap.MapName, tileMap);
            }

            // TODO OZ-17 remove saved maps other than the last loaded one
        }
    }
}
