namespace Ozzyria.Content.Models.Area
{
    public class PrefabEntry
    {
        public string PrefabId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Model.Types.ValuePacket Attributes { get; set; }
    }

    public class PrefabData
    {
        public PrefabEntry[][] Prefabs { get; set; }
    }
}
