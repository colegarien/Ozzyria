using Ozzyria.Model.CodeGen.Generators.Fields;
using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Generators.Decorators
{
    public class FieldsDecorator : IClassDecorator
    {
        private readonly IFieldGenerator _fieldGenerator;
        public FieldsDecorator(IFieldGenerator fieldGenerator)
        {
            _fieldGenerator = fieldGenerator;
        }

        public string Actualize(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            return code.Replace(TemplateTag(), _fieldGenerator.GenerateFieldDeclarations(fields, defaults).Trim());
        }

        public string InterfaceName(string className)
        {
            // no additional interface needed
            return "";
        }

        public string TemplateTag()
        {
            return "{{FIELDS}}";
        }
    }
}
