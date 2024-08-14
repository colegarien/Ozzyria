using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ozzyria.Model.Types;

namespace Grynt.Model.Definitions
{
    public class TypeDefinition
    {
        public const string TYPE_ASSUMED = "assumed";
        public const string TYPE_ENUM = "enum";
        public const string TYPE_CLASS = "class";

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("values")]
        public List<string> EnumValues { get; set; }

        [JsonPropertyName("fields")]
        public Dictionary<string, FieldDefinition> ClassFields { get; set; }

        [JsonPropertyName("defaults")]
        public ValuePacket ClassDefaults { get; set; }

        public bool IsNullable()
        {
            return Id == "string" || Type == TYPE_CLASS;
        }
    }
}
