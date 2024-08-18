using Ozzyria.Model.CodeGen.Definitions;
using System.Text.Json;

namespace Ozzyria.Model.CodeGen.DefinitionPackages
{
    public class TypePackage
    {
        public Dictionary<string, TypeDefinition> Definitions { get; set; }

        public static TypePackage Load(string filePath)
        {
            var package = new TypePackage();
            package.Definitions = JsonSerializer.Deserialize<Dictionary<string, TypeDefinition>>(File.ReadAllText(filePath));

            // map key id's into the definitions
            foreach(var kv in package.Definitions)
            {
                kv.Value.Id = kv.Key;
                if(kv.Value.ClassFields != null)
                {
                    foreach(var classKv in kv.Value.ClassFields)
                    {
                        classKv.Value.Id = classKv.Key;
                    }
                }
            }

            // sprinkle in assumed types
            package.Definitions["int"] = new TypeDefinition
            {
                Id = "int",
                Name = "int",
                Type = TypeDefinition.TYPE_ASSUMED,
            };
            package.Definitions["float"] = new TypeDefinition
            {
                Id = "float",
                Name = "float",
                Type = TypeDefinition.TYPE_ASSUMED,
            };
            package.Definitions["string"] = new TypeDefinition
            {
                Id = "string",
                Name = "string",
                Type = TypeDefinition.TYPE_ASSUMED,
            };
            package.Definitions["bool"] = new TypeDefinition
            {
                Id = "bool",
                Name = "bool",
                Type = TypeDefinition.TYPE_ASSUMED,
            };
            package.Definitions["type"] = new TypeDefinition
            {
                Id = "type",
                Name = "Type",
                Type = TypeDefinition.TYPE_ASSUMED,
            };

            return package;
        }

        public TypeDefinition GetDefinition(string typeId)
        {
            return Definitions.GetValueOrDefault(typeId);
        }
    }
}
