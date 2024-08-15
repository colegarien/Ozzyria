using Grynt.Model.Definitions;
using Grynt.Model.Packages;
using Ozzyria.Model.Types;
using System.Collections.Generic;

namespace Grynt.Generators.Decorators
{
    public class HydrateableDecorator : IClassDecorator
    {
        private readonly TypePackage _typePackage;
        public HydrateableDecorator(TypePackage typePackage)
        {
            _typePackage = typePackage;
        }

        public string Actualize(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            var codeDecoration = @"
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            {{HYDRATE}}
        }
";

            var hydrateCode = "";
            foreach (var field in fields)
            {
                var fieldType = _typePackage.GetDefinition(field.TypeId);
                if (fieldType == null)
                    continue;

                hydrateCode += "            " + GenerateFieldHydration(field, fieldType).Trim() + "\r\n";
            }

            return code.Replace(TemplateTag(), codeDecoration.Trim())
                .Replace("{{HYDRATE}}", hydrateCode.Trim());
        }

        private string GenerateFieldHydration(FieldDefinition field, TypeDefinition type, string fieldPrefix = "", string valuesPostfix = "")
        {
            var valuesName = "values" + valuesPostfix;
            var code = @"if ({{VALUES_PARAM}}.HasValueFor(""{{FIELD_ID}}""))
            {
                {{HYDRATE_FIELD}}
            }";

            var hyrdateCode = "";
            switch (type.Type)
            {
                case TypeDefinition.TYPE_ASSUMED:
                    if (type.Id == "string")
                    {
                        hyrdateCode = fieldPrefix + field.Name + " = {{VALUES_PARAM}}[\"{{FIELD_ID}}\"].Trim('\"');";
                    }
                    else
                    {
                        hyrdateCode = fieldPrefix + field.Name + " = " + type.Id + ".Parse({{VALUES_PARAM}}[\"{{FIELD_ID}}\"]);";
                    }
                    break;
                case TypeDefinition.TYPE_ENUM:
                    hyrdateCode = fieldPrefix + field.Name + " = (" + type.Name + ")Enum.Parse(typeof(" + type.Name + "), {{VALUES_PARAM}}[\"{{FIELD_ID}}\"], true);";
                    break;
                case TypeDefinition.TYPE_CLASS:
                    var fieldValuesPostFix = valuesPostfix + "_" + field.Id;
                    var fieldValuesName = "values" + fieldValuesPostFix;
                    hyrdateCode += "                var  " + fieldValuesName + " = " + valuesName + ".Extract(\"" + field.Id + "\");\r\n";
                    foreach (var subFieldByFieldId in type.ClassFields)
                    {
                        var subField = subFieldByFieldId.Value;
                        var subType = _typePackage.GetDefinition(subField.TypeId);
                        if (subType == null || subField == null)
                            continue;

                        var subHydrateBlock = GenerateFieldHydration(subField, subType, fieldPrefix + field.Name + ".", fieldValuesPostFix);
                        if (subHydrateBlock != "")
                        {
                            hyrdateCode += "                " + subHydrateBlock.Trim() + "\r\n";
                        }
                    }
                    break;
            }

            return code.Replace("{{HYDRATE_FIELD}}", hyrdateCode.Trim())
                       .Replace("{{FIELD_ID}}", field.Id)
                       .Replace("{{VALUES_PARAM}}", valuesName);
        }

        public string InterfaceName(string className)
        {
            return "IHydrateable";
        }

        public string TemplateTag()
        {
            return "{{HYRDATEABLE}}";
        }
    }
}
