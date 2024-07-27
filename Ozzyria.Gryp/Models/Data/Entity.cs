namespace Ozzyria.Gryp.Models.Data
{
    internal class Entity
    {
        public string PrefabId { get; set; }
        public float WorldX { get; set; }
        public float WorldY { get; set; }
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
    }
}
