using System.Collections.Generic;
using System.IO;
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


            // TODO OZ-17 consider wrapping this up in json reader/writer with all the custom converters
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DictionaryInt32Converter());
            serializeOptions.Converters.Add(new DictionaryInt32Int32Converter());

            File.WriteAllText(Content.Loader.Root() + "/Maps/map_metadata.json", JsonSerializer.Serialize(mapMetaDatas, serializeOptions));
        }
    }
}
