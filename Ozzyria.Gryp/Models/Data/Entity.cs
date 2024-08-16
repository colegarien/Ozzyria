using Ozzyria.Model.Types;

namespace Ozzyria.Gryp.Models.Data
{
    internal class Entity
    {
        public string InternalId { get; set; } = "";

        public string PrefabId { get; set; }
        public float WorldX { get; set; }
        public float WorldY { get; set; }
        public ValuePacket Attributes = new ValuePacket();
    }
}
