using System.Text.Json.Serialization;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Definitions
{
    public class PrefabDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("components")]
        public List<string> Components { get; set; }

        [JsonPropertyName("defaults")]
        public ValuePacket Defaults { get; set; }

        [JsonPropertyName("exposed")]
        public List<string> Exposed { get; set; }
    }
}
