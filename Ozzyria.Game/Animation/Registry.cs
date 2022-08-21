using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ozzyria.Game.Animation
{
    public class Registry
    {
        public string[] Resources { get; set; }
        public Dictionary<string, FrameSource> FrameSources { get; set; }
        public Dictionary<string, Clip> Clips { get; set; }

        public static Registry GetInstance()
        {
            return JsonSerializer.Deserialize<Registry>(File.ReadAllText(Content.Loader.Root() + "/Entities/resource_registry.json"), JsonOptionsFactory.GetOptions());
        }
    }
}
