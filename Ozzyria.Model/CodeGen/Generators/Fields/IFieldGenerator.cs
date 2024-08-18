using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Generators.Fields
{
    public interface IFieldGenerator
    {
        public string GenerateFieldDeclarations(List<FieldDefinition> fields, ValuePacket defaults = null);
    }
}
