using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ozzyria.ConstructionKit
{
    class TileSetMetaData
    {
        public List<int> TileTypes { get; set; }
        public IDictionary<int, string> TileNames { get; set; }
        public IDictionary<int, int> BaseTileX { get; set; }
        public IDictionary<int, int> BaseTileY { get; set; }
        public IDictionary<int, int> BaseTileZ { get; set; }

        // ordered lowest precedence to highest precedence
        public IList<int> TilesThatSupportTransitions { get; set; }
        public IList<int> TilesThatSupportPathing { get; set; }

        public IList<int> TilesThatSupportWalling { get; set; }
        public IDictionary<int, int> WallingCenterXOffset { get; set; }
        public IDictionary<int, int> WallingCenterYOffset { get; set; }
        public IDictionary<int, int> WallingThickness { get; set; }
    }

    class TileSetMetaDataFactory
    {
        public static IDictionary<string, TileSetMetaData> tileSetMetaDatas;

        public static void AddNewTileSet(string id)
        {
            tileSetMetaDatas[id] = new TileSetMetaData
            {
                TileTypes = new List<int>(),
                TileNames = new Dictionary<int, string>(),
                BaseTileX = new Dictionary<int, int>(),
                BaseTileY = new Dictionary<int, int>(),
                BaseTileZ = new Dictionary<int, int>(),

                TilesThatSupportTransitions = new List<int>(),
                TilesThatSupportPathing = new List<int>(),

                TilesThatSupportWalling = new List<int>(),
                WallingCenterXOffset = new Dictionary<int, int>(),
                WallingCenterYOffset = new Dictionary<int, int>(),
                WallingThickness = new Dictionary<int, int>()
            };
        }

        public static void AddNewTileType(string tileSetId, int type, string name)
        {
            // TODO set default yes / no questions and x / y for tile type
            tileSetMetaDatas[tileSetId].TileTypes.Add(type);
            tileSetMetaDatas[tileSetId].TileNames[type] = name;
        }

        public static void EnsureInitializedMetaData()
        {
            if (tileSetMetaDatas != null)
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

            tileSetMetaDatas = JsonSerializer.Deserialize<IDictionary<string, TileSetMetaData>>(File.ReadAllText(Content.Loader.Root() + "/TileSets/tileset_metadata.json"), serializeOptions);
        }

        public static void SaveMetaData()
        {
            if (tileSetMetaDatas == null)
            {
                // if nothing is already initialized, don't bother saving
                return;
            }


            // TODO OZ-17 consider wrapping this up in json reader/writer with all the custom converters
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DictionaryInt32Converter());
            serializeOptions.Converters.Add(new DictionaryInt32Int32Converter());

            File.WriteAllText(Content.Loader.Root() + "/TileSets/tileset_metadata.json", JsonSerializer.Serialize(tileSetMetaDatas, serializeOptions));
        }
    }
}
