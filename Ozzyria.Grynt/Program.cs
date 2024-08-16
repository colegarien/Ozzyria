using Grynt.Generators.Decorators;
using Grynt.Generators.Fields;
using Grynt.Generators;
using Grynt.Model.Packages;
using Grynt.Model.Definitions;
using Ozzyria.Model.CodeGen.Generators;
using Ozzyria.Content;
using Ozzyria.Model.CodeGen.Packages;

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
            string code = "";

            var packages = Packages.GetInstance();
            var typePackage = packages.TypePackage;
            var componentPackage = packages.ComponentPackage;

            System.Console.WriteLine("---- TYPES ----");
            var basicFieldGenerator = new FieldsGenerator(typePackage);
            var typeClassGenerator = new ClassGenerator(targetNamespace, "Types", [
                new FieldsDecorator(basicFieldGenerator),
                new SerializableDecorator(typePackage),
                new HydrateableDecorator(typePackage)
            ]);
            var typeGenerator = new TypeGenerator(targetNamespace, typePackage, typeClassGenerator);
            foreach (var type in typePackage.Definitions.Values)
            {
                code = typeGenerator.Generate(type);
                System.Console.WriteLine(code);
                if (type.Type != TypeDefinition.TYPE_ASSUMED)
                {
                    File.WriteAllText(Path.Combine(modelRoot, "Types", type.Name + ".cs"), code);
                }
            }


            System.Console.WriteLine("---- COMPONENTS ----");
            var componentClassGenerator = new ComponentGenerator(targetNamespace, typePackage);
            foreach (var component in componentPackage.Definitions.Values)
            {
                code = componentClassGenerator.Generate(component);
                System.Console.WriteLine(code);
                File.WriteAllText(Path.Combine(modelRoot, "Components", component.Name + ".cs"), code);
            }

            System.Console.WriteLine("---- UTILS ----");
            var serializerGenerator = new EntitySerializerGenerator(targetNamespace, componentPackage);
            code = serializerGenerator.Generate();
            System.Console.WriteLine(code);
            File.WriteAllText(Path.Combine(modelRoot, "Utility", "EntitySerializer.cs"), code);
        }
    }
}
