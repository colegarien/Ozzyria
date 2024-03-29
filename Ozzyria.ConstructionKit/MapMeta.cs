﻿using Ozzyria.Game;
using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Ozzyria.ConstructionKit
{
    class MapMetaData
    {
        public string TileSet { get; set; }
        public string EntityTemplate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Layers { get; set; }
    }

    class MapMetaDataFactory
    {
        public static IDictionary<string, MapMetaData> mapMetaDatas;

        public static void AddNewMap(string id)
        {
            mapMetaDatas.Add(id, new MapMetaData
            {
                TileSet = TileSetMetaDataFactory.tileSetMetaDatas.Keys.First(),
                EntityTemplate = id + "_template",
                Width = 32,
                Height = 32,
                Layers = 2
            });
        }

        public static void EnsureInitializedMetaData()
        {
            if (mapMetaDatas != null)
            {
                // if something is already initialized, don't bother re-intializing
                return;
            }

            InitializeMetaData();
        }

        public static void InitializeMetaData()
        {
            mapMetaDatas = JsonSerializer.Deserialize<IDictionary<string, MapMetaData>>(File.ReadAllText(Content.Loader.Root() + "/Maps/map_metadata.json"), JsonOptionsFactory.GetOptions());
        }

        public static void SaveMetaData()
        {
            if (mapMetaDatas == null)
            {
                // if nothing is already initialized, don't bother saving
                return;
            }


            foreach (var mapMetaData in mapMetaDatas)
            {
                var mapName = mapMetaData.Key;
                var metaData = mapMetaData.Value;

                if (MapFactory.MapExists(mapName))
                {
                    var map = MapFactory.LoadMap(mapName);
                    map.TileSet = metaData.TileSet;
                    map.Width = metaData.Width;
                    map.Height = metaData.Height;

                    var newLayers = new Dictionary<int, List<Tile>>();
                    for (var i = 0; i < metaData.Layers; i++)
                    {
                        if (map.Layers.ContainsKey(i))
                        {
                            // remove any tiles that shouldn't exist anymore
                            newLayers[i] = map.Layers[i].Where(t => t.X < map.Width && t.Y < map.Height).ToList();
                        }
                        else
                        {
                            // brand new layer
                            newLayers[i] = new List<Tile>();
                        }
                    }
                    map.Layers = newLayers;
                }
                else
                {
                    MapFactory.NewMap(mapName);
                    MapFactory.loadedMaps[mapName].TileSet = metaData.TileSet;
                    MapFactory.loadedMaps[mapName].Width = metaData.Width;
                    MapFactory.loadedMaps[mapName].Height = metaData.Height;
                    for (var i = 0; i < metaData.Layers; i++)
                        MapFactory.loadedMaps[mapName].Layers[i] = new List<Tile>();
                }
            }
            MapFactory.SaveMaps();

            File.WriteAllText(Content.Loader.Root() + "/Maps/map_metadata.json", JsonSerializer.Serialize(mapMetaDatas, JsonOptionsFactory.GetOptions()));
        }
    }
}
