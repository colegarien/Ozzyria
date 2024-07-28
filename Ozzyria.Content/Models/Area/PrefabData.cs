using System.Collections.Generic;

namespace Ozzyria.Content.Models.Area
{
    public class PrefabEntry
    {
        public string PrefabId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }

    public class PrefabData
    {
        public PrefabEntry[][] Prefabs { get; set; }
    }
}
