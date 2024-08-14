using Grynt.Model.Packages;
namespace Grynt.Generators.Fields
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
