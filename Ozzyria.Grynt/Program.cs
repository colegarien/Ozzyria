using Grynt.Generators.Decorators;
using Grynt.Generators.Fields;
using Grynt.Generators;
using Grynt.Model.Packages;
using Grynt.Model.Definitions;

namespace Ozzyria.Grynt
{
    internal class Program
    {
        public static string OzzyriaModelRoot()
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Ozzyria.Model");
        }

        static void Main(string[] args)
        {
            var targetNamespace = "Ozzyria.Model";
            var modelRoot = OzzyriaModelRoot();

            System.Console.WriteLine("---- TYPES ----");

            var typePackage = TypePackage.Load(Path.Combine(modelRoot, "CodeGen", "Files", "type_defs.json"));
            var basicFieldGenerator = new FieldsGenerator(typePackage);
            var typeClassGenerator = new ClassGenerator(targetNamespace, "Types", [
                new FieldsDecorator(basicFieldGenerator),
                new SerializableDecorator(typePackage),
                new HydrateableDecorator(typePackage)
            ]);
            var typeGenerator = new TypeGenerator(targetNamespace, typePackage, typeClassGenerator);
            foreach (var type in typePackage.Definitions.Values)
            {
                var code = typeGenerator.Generate(type);
                System.Console.WriteLine(code);
                if (type.Type != TypeDefinition.TYPE_ASSUMED)
                {
                    File.WriteAllText(Path.Combine(modelRoot, "Types", type.Name + ".cs"), code);
                }
            }


            System.Console.WriteLine("---- COMPONENTS ----");

            var componentPackage = ComponentPackage.Load(Path.Combine(modelRoot, "CodeGen", "Files", "component_defs.json"));
            var componentClassGenerator = new ComponentGenerator(targetNamespace, typePackage);
            foreach (var component in componentPackage.Definitions.Values)
            {
                var code = componentClassGenerator.Generate(component);
                System.Console.WriteLine(code);
                File.WriteAllText(Path.Combine(modelRoot, "Components", component.Name + ".cs"), code);
            }
        }
    }
}
