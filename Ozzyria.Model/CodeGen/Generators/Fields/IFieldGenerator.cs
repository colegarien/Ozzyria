using System.Collections.Generic;
using Grynt.Model.Definitions;
using Ozzyria.Model.Types;

namespace Grynt.Generators.Fields
{
    public interface IFieldGenerator
    {
        public string GenerateFieldDeclarations(List<FieldDefinition> fields, ValuePacket defaults = null);
    }
}
