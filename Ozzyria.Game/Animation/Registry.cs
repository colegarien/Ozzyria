using Ozzyria.Game.Components;
using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ozzyria.Game.Animation
{
    public class Registry
    {
        protected static Registry _instance;

        public Dictionary<uint, string> Resources { get; set; }
        public Dictionary<string, SkeletonOffsets> SkeletonOffsets { get; set; }
        public Dictionary<string, Drawable> Drawables { get; set; }
        public Dictionary<string, Drawable> UIIcons { get; set; }

        public static Registry GetInstance()
        {
            if (_instance == null)
            {
                _instance = JsonSerializer.Deserialize<Registry>(File.ReadAllText(Content.Loader.Root() + "/Entities/resource_registry.json"), JsonOptionsFactory.GetOptions());
            }

            return _instance;
        }
    }
}
