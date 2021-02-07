using Ozzyria.Game.Component;
using SFML.System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozzyria.MapEditor
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
    }

    // TODO OZ-18 : Make a Content project to manage all this data? TileSets -> MetaData & Sprites folders in content project
    class TileSetMetaDataFactory
    {
        private IDictionary<string, TileSetMetaData> tileSetMetaDatas;

        private string currentTileSet = "outside_tileset_001";
        private TileSetMetaData currentMetadata;

        public void SetCurrentTileSet(string tileSet)
        {
            InitializeMetaData();
            currentTileSet = tileSet;
            currentMetadata = tileSetMetaDatas[currentTileSet];
        }

        public int[] GetTypes()
        {
            InitializeMetaData();
            return currentMetadata.TileTypes.ToArray();
        }

        public int GetZIndex(int type)
        {
            InitializeMetaData();
            return currentMetadata.BaseTileZ.ContainsKey(type)
                ? currentMetadata.BaseTileZ[type]
                : Renderable.Z_BACKGROUND;
        }

        public Vector2i GetTextureCoordinates(int type, PathDirection direction)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey(type) ? currentMetadata.BaseTileX[type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey(type) ? currentMetadata.BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (IsPathable(type))
            {
                switch (direction)
                {
                    case PathDirection.Left:
                        offsetX = 2;
                        offsetY = 3;
                        break;
                    case PathDirection.Right:
                        offsetX = 3;
                        offsetY = 3;
                        break;
                    case PathDirection.Up:
                        offsetX = 0;
                        offsetY = 3;
                        break;
                    case PathDirection.Down:
                        offsetX = 1;
                        offsetY = 3;
                        break;
                    case PathDirection.LeftRight:
                        offsetX = 1;
                        offsetY = 0;
                        break;
                    case PathDirection.UpT:
                        offsetX = 3;
                        offsetY = 0;
                        break;
                    case PathDirection.DownT:
                        offsetX = 2;
                        offsetY = 0;
                        break;
                    case PathDirection.UpDown:
                        offsetX = 2;
                        offsetY = 2;
                        break;
                    case PathDirection.LeftT:
                        offsetX = 3;
                        offsetY = 2;
                        break;
                    case PathDirection.RightT:
                        offsetX = 3;
                        offsetY = 1;
                        break;
                    case PathDirection.UpLeft:
                        offsetX = 1;
                        offsetY = 2;
                        break;
                    case PathDirection.UpRight:
                        offsetX = 0;
                        offsetY = 2;
                        break;
                    case PathDirection.DownRight:
                        offsetX = 0;
                        offsetY = 1;
                        break;
                    case PathDirection.DownLeft:
                        offsetX = 1;
                        offsetY = 1;
                        break;
                    case PathDirection.All:
                        offsetX = 0;
                        offsetY = 0;
                        break;
                    default:
                        offsetX = 2;
                        offsetY = 1;
                        break;
                }
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public Vector2i GetEdgeTransitionTextureCoordinates(int type, EdgeTransitionType edgeTransition)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey(type) ? currentMetadata.BaseTileX[type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey(type) ? currentMetadata.BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (currentMetadata.TilesThatSupportTransitions.Any(t => t == type))
            {
                offsetX = (int)edgeTransition; // cause the fancy bit-mask
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public Vector2i GetCornerTransitionTextureCoordinates(int type, CornerTransitionType cornerTransition)
        {
            InitializeMetaData();
            int baseTx = currentMetadata.BaseTileX.ContainsKey(type) ? currentMetadata.BaseTileX[type] : 0;
            int baseTy = currentMetadata.BaseTileY.ContainsKey(type) ? currentMetadata.BaseTileY[type] : 0;
            var offsetX = 0;
            var offsetY = 0;

            if (currentMetadata.TilesThatSupportTransitions.Any(t => t == type))
            {
                offsetY = 1;
                offsetX = (int)cornerTransition; // cause the fancy bit-mask
            }

            return new Vector2i
            {
                X = baseTx + offsetX,
                Y = baseTy + offsetY
            };
        }

        public bool CanTransition(int toType, int fromType)
        {
            InitializeMetaData();
            var toIndex = currentMetadata.TilesThatSupportTransitions.IndexOf(toType);
            var fromIndex = currentMetadata.TilesThatSupportTransitions.IndexOf(fromType);
            /* 
             * Is not tranistioning into self
             *  AND both tile types are transitionable
             *  AND tile transitioned INTO is lower precedence 
             */
            return toType != fromType
                && fromIndex != -1 && toIndex != -1
                && toIndex < fromIndex;
        }

        public int[] GetTransitionTypesPrecedenceAscending()
        {
            InitializeMetaData();
            return currentMetadata.TilesThatSupportTransitions.Select(t => t).ToArray();
        }

        public bool IsPathable(int type)
        {
            InitializeMetaData();
            return currentMetadata.TilesThatSupportPathing.Any(t => t == type);
        }


        private void InitializeMetaData()
        {
            if (tileSetMetaDatas != null)
            {
                // if something is already initialized, don't bother re-intializing
                return;
            }

            // TODO consider wrapping this up in json reader/writer with all the custom converters
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DictionaryInt32Converter());
            serializeOptions.Converters.Add(new DictionaryInt32Int32Converter());

            tileSetMetaDatas = JsonSerializer.Deserialize<IDictionary<string, TileSetMetaData>>(File.ReadAllText("tileset_metadata.json"), serializeOptions);
            if (tileSetMetaDatas.ContainsKey(currentTileSet))
            {
                currentMetadata = tileSetMetaDatas[currentTileSet];
            }
        }
    }

    public class DictionaryInt32Converter : JsonConverter<IDictionary<int, string>>
    {
        public override IDictionary<int, string> Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var value = new Dictionary<int, string>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return value;
                }

                string keyString = reader.GetString();

                if (!int.TryParse(keyString, out int keyAsInt32))
                {
                    throw new JsonException($"Unable to convert \"{keyString}\" to System.Int32.");
                }

                reader.Read();

                string itemValue = reader.GetString();

                value.Add(keyAsInt32, itemValue);
            }

            throw new JsonException("Error Occured");
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<int, string> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (KeyValuePair<int, string> item in value)
            {
                writer.WriteString(item.Key.ToString(), item.Value);
            }

            writer.WriteEndObject();
        }
    }
    public class DictionaryInt32Int32Converter : JsonConverter<IDictionary<int, int>>
    {
        public override IDictionary<int, int> Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var value = new Dictionary<int, int>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return value;
                }

                string keyString = reader.GetString();

                if (!int.TryParse(keyString, out int keyAsInt32))
                {
                    throw new JsonException($"Unable to convert \"{keyString}\" to System.Int32.");
                }

                reader.Read();

                int valueAsInt32 = reader.GetInt32();
                value.Add(keyAsInt32, valueAsInt32);
            }

            throw new JsonException("Error Occured");
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<int, int> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (KeyValuePair<int, int> item in value)
            {
                writer.WriteString(item.Key.ToString(), item.Value.ToString());
            }

            writer.WriteEndObject();
        }
    }
}
