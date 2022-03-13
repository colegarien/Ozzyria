using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozzyria.Game.Persistence
{
    public class JsonOptionsFactory
    {
        public static JsonSerializerOptions GetOptions()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DictionaryInt32ListConverter<EdgeTransitionType>());
            serializeOptions.Converters.Add(new DictionaryInt32ListConverter<CornerTransitionType>());
            serializeOptions.Converters.Add(new DictionaryInt32ListConverter<List<Tile>>());
            serializeOptions.Converters.Add(new DictionaryInt32ListConverter<int>());
            serializeOptions.Converters.Add(new DictionaryInt32ListConverter<string>());

            return serializeOptions;
        }
    }

    // OZ-17 : make a factory so no more need for "T" explicitly - https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-core-3-1
    public class DictionaryInt32ListConverter<T> : JsonConverter<IDictionary<int, T>>
    {
        public override IDictionary<int, T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var value = new Dictionary<int, T>();

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

                var itemValue = JsonSerializer.Deserialize<T>(ref reader, options);
                value.Add(keyAsInt32, itemValue);
            }

            throw new JsonException("Error Occured");
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<int, T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<int, T> item in value)
            {
                writer.WritePropertyName(item.Key.ToString());
                JsonSerializer.Serialize(writer, item.Value, options);

            }
            writer.WriteEndObject();
        }
    }

}
