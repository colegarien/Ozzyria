using Ozzyria.Model.CodeGen.Definitions;
using System.Text.Json;

namespace Ozzyria.Model.CodeGen.Packages
{
    public class PrefabPackage
    {
        public Dictionary<string, PrefabDefinition> Definitions { get; set; }

        public static PrefabPackage Load(string filePath)
        {
            var package = new PrefabPackage();
            package.Definitions = JsonSerializer.Deserialize<Dictionary<string, PrefabDefinition>>(File.ReadAllText(filePath));

            // map key id's into the definitions
            foreach (var kv in package.Definitions)
            {
                kv.Value.Id = kv.Key;
            }

            return package;
        }

        public PrefabDefinition GetDefinition(string componentId)
        {
            return Definitions.GetValueOrDefault(componentId);
        }
    }
}
