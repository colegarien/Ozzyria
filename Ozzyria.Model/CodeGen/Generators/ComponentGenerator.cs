using Ozzyria.Model.CodeGen.Generators.Decorators;
using Ozzyria.Model.CodeGen.Generators.Fields;
using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.CodeGen.DefinitionPackages;

namespace Ozzyria.Model.CodeGen.Generators
{
    public class ComponentGenerator
    {
        private readonly ClassGenerator _componentClassGenerator;
        private readonly ClassGenerator _pooledComponentClassGenerator;

        public ComponentGenerator(string ns, TypePackage typePackage)
        {
            var grecsFieldGenerator = new GrecsFieldGenerator(typePackage);
            _componentClassGenerator = new ClassGenerator(ns, "Components",new IClassDecorator[] {
                new GrecsComponentDecorator(),
                new FieldsDecorator(grecsFieldGenerator),
                new SerializableDecorator(typePackage),
                new HydrateableDecorator(typePackage)
            });
            _pooledComponentClassGenerator = new ClassGenerator(ns, "Components", new IClassDecorator[] {
                new GrecsPooledComponentDecorator(),
                new FieldsDecorator(grecsFieldGenerator),
                new SerializableDecorator(typePackage),
                new HydrateableDecorator(typePackage)
            });
        }

        public string Generate(ComponentDefinition componentDefinition)
        {
            if (componentDefinition.IsPooled)
            {
                return _pooledComponentClassGenerator.Generate(componentDefinition.Name, componentDefinition.Id, componentDefinition.Fields.Values.ToList(), componentDefinition.Defaults);
            }

            return _componentClassGenerator.Generate(componentDefinition.Name, componentDefinition.Id, componentDefinition.Fields.Values.ToList(), componentDefinition.Defaults);
        }

    }
}
