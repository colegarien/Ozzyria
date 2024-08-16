using Grynt.Model.Packages;
namespace Ozzyria.Model.CodeGen.Generators
{
    public class EntitySerializerGenerator
    {

        private string _ns = "";
        private ComponentPackage _componentPackage;

        public EntitySerializerGenerator(string ns, ComponentPackage componentPackage)
        {
            _ns = ns;
            _componentPackage = componentPackage;
        }

        private string ApplyNamespace(string code)
        {
            return code.Replace("{{NAMESPACE}}", _ns == "" ? "" : (_ns + "."));
        }

        public string Generate()
        {
            var code = @"using Grecs;
using Ozzyria.Model.Components;
using Ozzyria.Model.Types;

namespace {{NAMESPACE}}Utility
{
    public class EntitySerializer
    {
        public static Dictionary<string, Type> ComponentIdToTypeMap = new Dictionary<string, Type>
        {
            {{ID_TO_TYPE_MAPPING}}
        };

        public static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.id);

            var components = entity.GetComponents();
            writer.Write(components.Length);
            foreach (var component in entity.GetComponents())
            {
                WriteComponent(entity, writer, component);
            }
        }

        public static void WriteDetachedEntity(BinaryWriter writer, Entity entity)
        {
            var components = entity.GetComponents();
            writer.Write(components.Length);
            foreach (var component in entity.GetComponents())
            {
                WriteComponent(entity, writer, component);
            }
        }


        private static void WriteComponent(Entity entity, BinaryWriter writer, IComponent component)
        {
            if (!(component is ISerializable))
                return;


            var serializeable = component as ISerializable;
            writer.Write(serializeable.GetComponentIdentifier());
            serializeable.Write(writer);
        }

        public static Entity ReadEntity(EntityContext context, BinaryReader reader)
        {
            var id = reader.ReadUInt32();
            var entity = context.CreateEntity(id);
            // remove components that don't get updated

            var numberOfComponents = reader.ReadInt32();
            var i = 0;
            var componentsRead = new IComponent[numberOfComponents];
            while (numberOfComponents != i && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                componentsRead[i] = ReadComponent(entity, reader);
                i++;
            }

            foreach (var component in entity.GetComponents())
            {
                if (!componentsRead.Contains(component))
                {
                    entity.RemoveComponent(component);
                }
            }

            return entity;
        }

        public static Entity ReadDetachedEntity(BinaryReader reader)
        {
            var entity = new Entity();

            var numberOfComponents = reader.ReadInt32();
            var componentsRead = 0;
            while (numberOfComponents != componentsRead && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                ReadComponent(entity, reader);
                componentsRead++;
            }

            return entity;
        }

        private static IComponent ReadComponent(Entity entity, BinaryReader reader)
        {
            var componentIdentifier = reader.ReadString();
            if (!ComponentIdToTypeMap.ContainsKey(componentIdentifier)) {
                return null;
            }

            var componentType = ComponentIdToTypeMap[componentIdentifier];
            var component = entity.GetComponent(componentType);
            if (component == null)
            {
                component = entity.CreateComponent(componentType);
                entity.AddComponent(component);
            }

            if (component is ISerializable)
            {
                ((ISerializable)component).Read(reader);
            }

            return component;
        }


    }
}
";

            var idToTypeMappings = "";
            foreach (var component in _componentPackage.Definitions.Values)
            {
                idToTypeMappings += "{\""+component.Id+"\", typeof("+component.Name+")},\r\n            ";
            }

            return ApplyNamespace(code)
                .Replace("{{ID_TO_TYPE_MAPPING}}", idToTypeMappings.Trim());
        }
    }
}
