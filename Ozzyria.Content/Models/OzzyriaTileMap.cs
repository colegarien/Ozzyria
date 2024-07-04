using Ozzyria.Content.Util;
using System.IO;
using System.Text.Json;

namespace Ozzyria.Content.Models
{
    internal class OzzyriaTileMap
    {
        public string Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string[][][] Layers { get; set; }

        public static OzzyriaTileMap Retrieve(string id)
        {
            return JsonSerializer.Deserialize<OzzyriaTileMap>(File.ReadAllText(GetDirectory() + "/" + id + ".otm"), JsonOptionsFactory.GetOptions());
        }

        public static void Store(OzzyriaTileMap otm)
        {
            File.WriteAllText(GetDirectory() + "/" + otm.Id + ".otm", JsonSerializer.Serialize(otm, JsonOptionsFactory.GetOptions()));
        }

        private static string GetDirectory()
        {
            var baseDirectory = Loader.Root() + "/TileMaps";
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            return baseDirectory;
        }
    }
}
