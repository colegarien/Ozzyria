using Ozzyria.Model.Types;

namespace Ozzyria.Gryp.Models.Data
{
    internal class Entity
    {
        public Dictionary<string, List<string>> AttributeLinks = new Dictionary<string, List<string>>
        {
            {"X", new List<string>{ "movement::x", "movement::previousX" } },
            {"Y", new List<string>{ "movement::y", "movement::previousY" } },
            {"Layer", new List<string>{ "movement::layer" } },
        };

        public string InternalId { get; set; } = "";

        public string PrefabId { get; set; }
        public float WorldX { get; set; }
        public float WorldY { get; set; }
        public ValuePacket Attributes = new ValuePacket();
    }
}
