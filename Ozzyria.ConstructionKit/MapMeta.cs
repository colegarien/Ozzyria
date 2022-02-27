using Ozzyria.Game;
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
                TileSet = "",
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
            // TODO OZ-17 consider wrapping this up in json reader/writer with all the custom converters
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DictionaryInt32Converter());
            serializeOptions.Converters.Add(new DictionaryInt32Int32Converter());

            mapMetaDatas = JsonSerializer.Deserialize<IDictionary<string, MapMetaData>>(File.ReadAllText(Content.Loader.Root() + "/Maps/map_metadata.json"), serializeOptions);
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

                // TODO OZ-17 this will be a bottle-neck... could be smarter and only save/add the ones changed
                var persistence = new WorldPersistence();
                var mapFile = Content.Loader.Root() + "/Maps/" + mapName + ".ozz";
                if (File.Exists(mapFile))
                {
                    var tileMap = persistence.LoadMap(mapName);
                    tileMap.TileSet = metaData.TileSet;
                    tileMap.Width = metaData.Width;
                    tileMap.Height = metaData.Height;

                    var newLayers = new Dictionary<int, List<Tile>>();
                    for (var i = 0; i < metaData.Layers; i++)
                    {
                        if (tileMap.HasLayer(i))
                        {
                            // remove any tiles that shouldn't exist anymore
                            newLayers[i] = tileMap.Layers[i].Where(t => t.X < tileMap.Width && t.Y < tileMap.Height).ToList();
                        }
                        else
                        {
                            // brand new layer
                            newLayers[i] = new List<Tile>();
                        }
                    }
                    tileMap.Layers = newLayers;

                    persistence.SaveMap(mapName, tileMap);
                }
                else
                {
                    var newMap = new TileMap
                    {
                        MapName = mapName,
                        TileSet = metaData.TileSet,
                        Width = metaData.Width,
                        Height = metaData.Height
                    };

                    for(var i = 0; i < metaData.Layers; i++)
                        newMap.Layers[i] = new List<Tile>();

                    persistence.SaveMap(mapName, newMap);
                }
            }

            // TODO OZ-17 consider wrapping this up in json reader/writer with all the custom converters
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DictionaryInt32Converter());
            serializeOptions.Converters.Add(new DictionaryInt32Int32Converter());

            File.WriteAllText(Content.Loader.Root() + "/Maps/map_metadata.json", JsonSerializer.Serialize(mapMetaDatas, serializeOptions));
        }
    }
}
