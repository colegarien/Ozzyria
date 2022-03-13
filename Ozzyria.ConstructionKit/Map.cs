using Ozzyria.Game;
using Ozzyria.Game.Component;
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

        public static void PaintTile(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta, int layer, int x, int y, int tileType)
        {
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height
                || layer < 0 || layer >= mapMeta.Layers
                || !tileSetMeta.TileTypes.Any(t => t == tileType))
            {
                return;
            }

            if (tileType == 0)
            {
                map.RemoveTile(layer, x, y);
                return;
            }

            if (!map.HasLayer(layer))
                map.Layers[layer] = new List<Tile>();

            var tileIndex = map.Layers[layer].FindIndex(t => t.X == x && t.Y == y);
            if(tileIndex == -1)
            {
                var newTile = tileSetMeta.CreateTile(tileType);
                newTile.X = x;
                newTile.Y = y;
                map.Layers[layer].Add(newTile);

                return;
            }

            var tile = map.Layers[layer][tileIndex];

            tile.Type = tileType;
            tile.TextureCoordX = tileSetMeta.BaseTileX[tileType];
            tile.TextureCoordY = tileSetMeta.BaseTileY[tileType];
            tile.Z = tileSetMeta.BaseTileZ.ContainsKey(tileType) ? tileSetMeta.BaseTileZ[tileType] : Renderable.Z_BACKGROUND;
            
            // TODO OZ-17 baking map / calcing transition
            tile.Decals = new TileDecal[] { };
            tile.EdgeTransition = new Dictionary<int, EdgeTransitionType>();
            tile.CornerTransition = new Dictionary<int, CornerTransitionType>();
            tile.Direction = PathDirection.None;
        }

        public static void FillTile(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta, int layer, int x, int y, int tileType)
        {
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height
                || layer < 0 || layer >= mapMeta.Layers
                || !tileSetMeta.TileTypes.Any(t => t == tileType))
            {
                return;
            }

            var currentTileType = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y)?.Type ?? 0;
            if (currentTileType == tileType)
            {
                return;
            }

            map.FillRecursive(mapMeta, tileSetMeta, layer, x, y, tileType, currentTileType);
        }

        public static void FillRecursive(this TileMap map, MapMetaData mapMeta, TileSetMetaData tileSetMeta, int layer, int x, int y, int toFillWith, int toReplace)
        {
            var currentTileType = map.Layers[layer].FirstOrDefault(t => t.X == x && t.Y == y)?.Type ?? 0;
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height
                || layer < 0 || layer >= mapMeta.Layers
                || !tileSetMeta.TileTypes.Any(t => t == toFillWith)
                || currentTileType != toReplace
                || currentTileType == toFillWith)
            {
                return;
            }

            map.PaintTile(mapMeta, tileSetMeta, layer, x, y, toFillWith);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x - 1, y, toFillWith, toReplace);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x + 1, y, toFillWith, toReplace);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x, y - 1, toFillWith, toReplace);
            map.FillRecursive(mapMeta, tileSetMeta, layer, x, y + 1, toFillWith, toReplace);
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
