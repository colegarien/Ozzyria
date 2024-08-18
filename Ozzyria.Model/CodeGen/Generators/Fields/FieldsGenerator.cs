using Ozzyria.Model.CodeGen.Definitions;
using Ozzyria.Model.CodeGen.DefinitionPackages;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.CodeGen.Generators.Fields
{
    public class FieldsGenerator : IFieldGenerator
    {
        protected readonly TypePackage _typePackage;

        protected string _callTrigger = "TriggerChange?.Invoke();";

        public FieldsGenerator(TypePackage typePackage)
        {
            _typePackage = typePackage;
        }

        public string GenerateFieldDeclarations(List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            var buildTriggerPreamble = BuildTriggerPreamble();
            var code = buildTriggerPreamble ? GenerateTriggerCode() : "";
            var fieldPropagationCode = "";
            foreach (var field in fields)
            {
                var type = _typePackage.GetDefinition(field.TypeId);
                if (type == null)
                    return "";

                if(buildTriggerPreamble && type.Type == TypeDefinition.TYPE_CLASS)
                {
                    // add trigger propagation as trigger gets changed into preamble
                    fieldPropagationCode += GenerateTriggerPropagation(field) + "\r\n                ";
                }

                code += GenerateFieldDeclaration(field, type, defaults) + "\r\n        ";
            }
            return code.Replace("{{FIELD_PROPAGATION}}", fieldPropagationCode.Trim());
        }

        protected virtual bool BuildTriggerPreamble()
        {
            return true;
        }


        protected string GenerateTriggerCode()
        {
            return @"private System.Action? _triggerChange;
        public System.Action? TriggerChange { get => _triggerChange; set
            {
                _triggerChange = value;
                {{FIELD_PROPAGATION}}
            }
        }
        
        ";
        }

        protected string GenerateTriggerPropagation(FieldDefinition field)
        {
            return field.Name + ".TriggerChange = TriggerChange;";
        }

        private string GenerateFieldDeclaration(FieldDefinition field, TypeDefinition type, ValuePacket defaults = null)
        {
            var code = @"
        private {{TYPE_NAME}} _{{FIELD_ID}}{{DEFAULTS}};
        public {{TYPE_NAME}} {{FIELD_NAME}}
        {
            get => _{{FIELD_ID}}; set
            {
                if ({{EQUALITY_CHECK}})
                {
                    _{{FIELD_ID}} = value;
                    {{FIELD_TRIGGER_PROPAGATION}}
                    {{TRIGGER_CHANGE_CALL}}
                }
            }
        }
";
            return code.Replace("{{EQUALITY_CHECK}}", type.IsNullable() ? "!_{{FIELD_ID}}?.Equals(value) ?? (value != null)" : "!_{{FIELD_ID}}.Equals(value)")
                       .Replace("{{TYPE_NAME}}", type.Name)       
                       .Replace("{{FIELD_NAME}}", field.Name)
                       .Replace("{{FIELD_ID}}", field.Id)
                       .Replace("{{DEFAULTS}}", GenerateDefaults(field, type, defaults))
                       .Replace("{{FIELD_TRIGGER_PROPAGATION}}", type.Type == TypeDefinition.TYPE_CLASS ? ("if (value != null) { _"+field.Id+".TriggerChange = TriggerChange; }") : "")
                       .Replace("{{TRIGGER_CHANGE_CALL}}", _callTrigger);
        }

        private string GenerateDefaults(FieldDefinition field, TypeDefinition type, ValuePacket defaults = null)
        {
            var hasDefaultValues = defaults != null && defaults.HasValueFor(field.Id);

            var code = " = {{VALUE}}";
            switch (type.Type)
            {
                case TypeDefinition.TYPE_ASSUMED:
                    if (hasDefaultValues)
                    {
                        if (type.Id == "type")
                        {
                            return code.Replace("{{VALUE}}", "typeof(" + defaults[field.Id] + ")");
                        }
                        else
                        {
                            // use raw value
                            return code.Replace("{{VALUE}}", defaults[field.Id]);
                        }
                    }
                    else if(type.Id == "string")
                    {
                        // return the empty string to avoid propagating nulls
                        return code.Replace("{{VALUE}}", "\"\"");
                    }
                    else if(type.Id == "type")
                    {
                        return code.Replace("{{VALUE}}", "typeof(object)");
                    }
                    else
                    {
                        return "";
                    }
                case TypeDefinition.TYPE_ENUM:
                    if (hasDefaultValues)
                    {
                        // qualify value with enum Type
                        return code.Replace("{{VALUE}}", type.Name + "." + defaults[field.Id]);
                    }
                    else
                    {
                        return "";
                    }
                case TypeDefinition.TYPE_CLASS:
                    // build class intializer syntax
                    var initializers = "";
                    if (hasDefaultValues)
                    {
                        var fieldDefaults = defaults.Extract(field.Id);
                        foreach (var subFieldByFieldId in type.ClassFields)
                        {
                            var subField = subFieldByFieldId.Value;
                            var subType = _typePackage.GetDefinition(subField.TypeId);
                            if (subType == null || subField == null || fieldDefaults == null || !fieldDefaults.HasValueFor(subField.Id))
                                continue;

                            var subAssignments = GenerateDefaults(subField, subType, fieldDefaults);
                            if (subAssignments != "")
                            {
                                initializers += subField.Name + subAssignments + ", ";
                            }
                        }
                    }

                    return code.Replace("{{VALUE}}", "new " + type.Name + "{ " + initializers + " }");
            }

            return "";
        }
    }
}
