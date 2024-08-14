using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ozzyria.Model.Types;

namespace Grynt.Model.Definitions
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
