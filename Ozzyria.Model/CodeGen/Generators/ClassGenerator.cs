using Ozzyria.Model.CodeGen.Generators.Decorators;
using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.CodeGen.DefinitionPackages;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Generators
{
    public class ClassGenerator
    {
        private string _ns = "";
        private string _destinationNamespace;
        private ComponentPackage _componentPackage;
        private IClassDecorator[] _classDecorators;

        public ClassGenerator(string ns, string destinationNamespace, IClassDecorator[] classDecorators)
        {
            _ns = ns;
            _destinationNamespace = destinationNamespace;
            _classDecorators = classDecorators;
        }

        public string Generate(string className, string classId, List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            var code = @"{{NAMESPACE_PREAMBLE}}
{
    public class {{CLASS_NAME}}{{INTERFACES}}
    {
        {{INTERFACE_TAGS}}
    }
}
";

            string interfaces = "";
            string interfaceTags = "";
            foreach (var classDecorator in _classDecorators)
            {
                var interfaceName = classDecorator.InterfaceName(className);
                if (interfaceName != "")
                {
                    if (interfaces == "")
                        interfaces = " : ";
                    else
                        interfaces += ", ";

                    interfaces += interfaceName;
                }

                var interfaceTag = classDecorator.TemplateTag();
                if (interfaceTag != "")
                {
                    interfaceTags += "        " + interfaceTag + "\r\n";
                }
            }

            code = ApplyNamespace(code)
                    .Replace("{{CLASS_NAME}}", className)
                    .Replace("{{INTERFACES}}", interfaces)
                    .Replace("{{INTERFACE_TAGS}}", interfaceTags.Trim());
            return Decorate(code, classId, fields, defaults);
        }

        private string Decorate(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            foreach (var decorator in _classDecorators)
            {
                code = decorator.Actualize(code, classId, fields, defaults);
            }
            return code;
        }

        private string ApplyNamespace(string code)
        {
            return code
                .Replace("{{NAMESPACE_PREAMBLE}}", NamespacePreamble())
                .Replace("{{NAMESPACE}}", _ns == "" ? "" : (_ns + "."));
        }

        private string NamespacePreamble()
        {
            if (_destinationNamespace == "Types")
            {
                return "namespace {{NAMESPACE}}Types";
            }
            else
            {
                // ensure types are imported
                return @"using {{NAMESPACE}}Types;

namespace {{NAMESPACE}}" + _destinationNamespace;
            }

        }
    }
}
