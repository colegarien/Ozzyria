using System.Text.Json.Serialization;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Definitions
{
    public class ComponentDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_pooled")]
        public bool IsPooled { get; set; }

        [JsonPropertyName("fields")]
        public Dictionary<string, FieldDefinition> Fields { get; set; }

        [JsonPropertyName("defaults")]
        public ValuePacket Defaults { get; set; }
    }
}
