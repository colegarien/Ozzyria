using Grynt.Model.Definitions;
using Ozzyria.Model.Types;
using System.Collections.Generic;

namespace Grynt.Generators.Decorators
{
    public interface IClassDecorator
    {
        public string InterfaceName(string className);
        public string TemplateTag();
        public string Actualize(string code, List<FieldDefinition> fields, ValuePacket defaults = null);
    }
}
