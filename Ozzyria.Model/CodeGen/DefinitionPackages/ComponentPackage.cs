using Ozzyria.Model.CodeGen.Definitions;
using System.Text.Json;

namespace Ozzyria.Model.CodeGen.DefinitionPackages
{
    public class ComponentPackage
    {
        public Dictionary<string, ComponentDefinition> Definitions { get; set; }

        public static ComponentPackage Load(string filePath)
        {
            var package = new ComponentPackage();
            package.Definitions = JsonSerializer.Deserialize<Dictionary<string, ComponentDefinition>>(File.ReadAllText(filePath));

            // map key id's into the definitions
            foreach (var kv in package.Definitions)
            {
                kv.Value.Id = kv.Key;
                if (kv.Value.Fields != null)
                {
                    foreach (var classKv in kv.Value.Fields)
                    {
                        classKv.Value.Id = classKv.Key;
                    }
                }
            }

            return package;
        }

        public ComponentDefinition GetDefinition(string componentId)
        {
            return Definitions.GetValueOrDefault(componentId);
        }
    }
}
