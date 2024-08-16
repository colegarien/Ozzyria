using Grynt.Model.Definitions;
using Ozzyria.Model.Types;

namespace Grynt.Generators.Decorators
{
    public interface IClassDecorator
    {
        public string InterfaceName(string className);
        public string TemplateTag();
        public string Actualize(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null);
    }
}
