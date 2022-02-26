using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozzyria.ConstructionKit
{

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


                int valueAsInt32;
                if (reader.TokenType == JsonTokenType.String)
                {
                    string valueString = reader.GetString();
                    if (!int.TryParse(valueString, out valueAsInt32))
                    {
                        throw new JsonException($"Unable to convert \"{valueString}\" to System.Int32.");
                    }
                }
                else if (!reader.TryGetInt32(out valueAsInt32))
                {
                    valueAsInt32 = 0;
                }

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
