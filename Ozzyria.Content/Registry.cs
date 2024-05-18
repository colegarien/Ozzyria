using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace Ozzyria.Content
{
    public class Registry
    {
        protected static Registry _instance;

        public Dictionary<uint, string> Resources { get; set; }
        public Dictionary<string, Models.SkeletonOffsets> SkeletonOffsets { get; set; }
        public Dictionary<string, Models.Drawable> Drawables { get; set; }
        public Dictionary<string, Models.Drawable> UIIcons { get; set; }

        public static Registry GetInstance()
        {
            if (_instance == null)
            {
                _instance = JsonSerializer.Deserialize<Registry>(File.ReadAllText(Content.Loader.Root() + "/Resources/resource_registry.json"), Util.JsonOptionsFactory.GetOptions());
            }

            return _instance;
        }
    }
}
