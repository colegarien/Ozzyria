﻿using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Generators.Decorators
{
    public class GrecsComponentDecorator : IClassDecorator
    {
        public string Actualize(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            // nothing to manipulate
            return code;
        }

        public string InterfaceName(string className)
        {
            return "Grecs.Component";
        }

        public string TemplateTag()
        {
            // nothing to inject into code, other than an interface
            return "";
        }
    }
}
