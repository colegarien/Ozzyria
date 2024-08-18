using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Generators.Decorators
{
    public interface IClassDecorator
    {
        public string InterfaceName(string className);
        public string TemplateTag();
        public string Actualize(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null);
    }
}
