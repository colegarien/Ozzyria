using Ozzyria.Model.CodeGen.DefinitionPackages;
namespace Ozzyria.Model.CodeGen.Generators.Fields
{
    public class GrecsFieldGenerator : FieldsGenerator
    {
        public GrecsFieldGenerator(TypePackage typePackage): base(typePackage) {
            _callTrigger = "TriggerChange();";
        }

        protected override bool BuildTriggerPreamble()
        {
            // Grecs has TriggerChange baked right in
            return false;
        }


    }
}
