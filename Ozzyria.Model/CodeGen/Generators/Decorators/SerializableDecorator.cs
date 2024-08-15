using Grynt.Model.Definitions;
using Grynt.Model.Packages;
using Ozzyria.Model.Types;

namespace Grynt.Generators.Decorators
{
    public class SerializableDecorator : IClassDecorator
    {
        private readonly TypePackage _typePackage;
        public SerializableDecorator(TypePackage typePackage)
        {
            _typePackage = typePackage;
        }

        public string Actualize(string code, string classId, List<FieldDefinition> fields, ValuePacket defaults = null)
        {
            var codeDecoration = @"
        public string GetComponentIdentifier() {
            return ""{{CLASS_ID}}"";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            {{WRITER}}
        }

        public void Read(System.IO.BinaryReader r)
        {
            {{READER}}
        }
";

            var writerCode = "";
            var readerCode = "";
            foreach(var field in fields)
            {
                var fieldType = _typePackage.GetDefinition(field.TypeId);
                if (fieldType == null || field.ExcludeFromSerialize)
                    continue;
                
                writerCode += "            " + GenerateFieldWriter(field, fieldType).Trim() + "\r\n";
                readerCode += "            " + GenerateFieldReader(field, fieldType).Trim() + "\r\n";
            }

            return code.Replace(TemplateTag(), codeDecoration.Trim())
                .Replace("{{CLASS_ID}}", classId)
                .Replace("{{WRITER}}", writerCode.Trim())
                .Replace("{{READER}}", readerCode.Trim());
        }

        private string GenerateFieldWriter(FieldDefinition field, TypeDefinition type, string fieldPrefix = "")
        {
            var code = "";
            if (field.ExcludeFromSerialize)
                return code;

            switch (type.Type)
            {
                case TypeDefinition.TYPE_ASSUMED:
                    // use raw value
                    return "w.Write(" + fieldPrefix + field.Name + ");";
                case TypeDefinition.TYPE_ENUM:
                    // enums are serialized as Int32's
                    return "w.Write((int)" + fieldPrefix + field.Name + ");";
                case TypeDefinition.TYPE_CLASS:
                    // build writer block for class
                    var writerBlock = "";

                    foreach (var subFieldByFieldId in type.ClassFields)
                    {
                        var subField = subFieldByFieldId.Value;
                        var subType = _typePackage.GetDefinition(subField.TypeId);
                        if (subType == null || subField == null)
                            continue;

                        var subWriterBlock = GenerateFieldWriter(subField, subType, fieldPrefix + field.Name + ".");
                        if (subWriterBlock != "")
                        {
                            writerBlock += "            " + subWriterBlock.Trim() + "\r\n";
                        }
                    }

                    return writerBlock;
            }

            return "";
        }

        private string GenerateFieldReader(FieldDefinition field, TypeDefinition type, string fieldPrefix = "")
        {
            var code = "";
            if (field.ExcludeFromSerialize)
                return code;

            switch (type.Type)
            {
                case TypeDefinition.TYPE_ASSUMED:
                    var readType = "";
                    switch (type.Id)
                    {
                        case "uint":
                            readType = "UInt32";
                            break;
                        case "int":
                            readType = "Int32";
                            break;
                        case "bool":
                            readType = "Boolean";
                            break;
                        case "float":
                            readType = "Single";
                            break;
                        case "string":
                            readType = "String";
                            break;
                    }
                    if (readType == "")
                        return "";

                    return fieldPrefix + field.Name + " = r.Read"+readType+"();";
                case TypeDefinition.TYPE_ENUM:
                    // enums are serialized as Int32's
                    return fieldPrefix + field.Name + " = ("+type.Name+")r.ReadInt32();";
                case TypeDefinition.TYPE_CLASS:
                    // build reader block for class
                    var readerBlock = "";

                    foreach (var subFieldByFieldId in type.ClassFields)
                    {
                        var subField = subFieldByFieldId.Value;
                        var subType = _typePackage.GetDefinition(subField.TypeId);
                        if (subType == null || subField == null)
                            continue;

                        var subReaderBlock = GenerateFieldReader(subField, subType, fieldPrefix + field.Name + ".");
                        if (subReaderBlock != "")
                        {
                            readerBlock += "            " + subReaderBlock.Trim() + "\r\n";
                        }
                    }

                    return readerBlock;
            }

            return "";
        }

        public string InterfaceName(string className)
        {
            return "ISerializable";
        }

        public string TemplateTag()
        {
            return "{{SERIALIZABLE}}";
        }
    }
}
