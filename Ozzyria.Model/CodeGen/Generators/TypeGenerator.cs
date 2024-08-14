using Grynt.Model.Definitions;
using Grynt.Model.Packages;
using System;
using System.Linq;

namespace Grynt.Generators
{
    public class TypeGenerator
    {
        private string _ns = "";
        private TypePackage _typePackage;
        private ClassGenerator _classGenerator;

        public TypeGenerator(string ns, TypePackage typePackage, ClassGenerator classGenerator)
        {
            _ns = ns;
            _typePackage = typePackage;
            _classGenerator = classGenerator;
        }

        public string Generate(TypeDefinition type) {
            var code = "";
            switch (type.Type)
            {
                case TypeDefinition.TYPE_ENUM:
                    code = GenerateEnum(type);
                    break;
                case TypeDefinition.TYPE_CLASS:
                    code = _classGenerator.Generate(type.Name, type.ClassFields.Values.ToList(), type.ClassDefaults);
                    break;
            }
            return code;
        }

        private string ApplyNamespace(string code)
        {
            return code.Replace("{{NAMESPACE}}", _ns == "" ? "" : (_ns + "."));
        }

        private string GenerateEnum(TypeDefinition type)
        {

            var code = @"namespace {{NAMESPACE}}Types
{
    public enum {{ENUM_NAME}}
    {
        {{ENUM_VALUES}}
    }
}
";

            return ApplyNamespace(code)
                .Replace("{{ENUM_NAME}}", type.Name)
                .Replace("{{ENUM_VALUES}}", String.Join(", \r\n        ", type.EnumValues).Trim());
        }
    }
}
