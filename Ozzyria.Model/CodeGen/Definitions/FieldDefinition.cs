using System.Text.Json.Serialization;

namespace Ozzyria.Model.CodeGen.Definitions
{
    public class FieldDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string TypeId { get; set; }

        [JsonPropertyName("exclude_from_serialize")]
        public bool ExcludeFromSerialize { get; set; }

    }
}
