using Grecs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ozzyria.Game.Persistence
{
    public class WorldPersistence
    {
        private static Dictionary<Type, Type> _baseTypeCache = new Dictionary<Type, Type>();

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
            var name = component?.GetType()?.ToString() ?? null;
            if (name == null)
            {
                writer.Write("");
                return;
            }

            var props = Reflector.GetSavableProperties(component.GetType());

            writer.Write(name);
            writer.Write(props.Length);
            foreach (var p in props)
            {
                writer.Write(p.Name);
                using (MemoryStream m = new MemoryStream())
                {
                    using (var writer2 = new BinaryWriter(m)) {
                        WriteValueOfType(entity, writer2, GetSerializableBaseType(p.PropertyType), Reflector.GetPropertyValue(p, component));
                    }

                    var bytes = m.ToArray();
                    writer.Write(bytes.Length);
                    writer.Write(bytes.ToArray());
                }
            }
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
            var componentType = Reflector.GetTypeForId(reader.ReadString());
            if (componentType == null)
                return null;

            var component = entity.GetComponent(componentType);
            if (component == null)
            {
                component = entity.CreateComponent(componentType);
                entity.AddComponent(component);
            }

            var props = Reflector.GetSavableProperties(component.GetType());
            var numberOfPropsToRead = reader.ReadInt32();
            for(var i = 0; i < numberOfPropsToRead; i++)
            {
                var propertyName = reader.ReadString();
                var packetSize = reader.ReadInt32();
                var property = props.FirstOrDefault(p => p.Name == propertyName);

                if (property == null)
                {
                    // Skip Over Packet
                    reader.ReadBytes(packetSize);
                    continue;
                }


                Reflector.SetPropertyValue(property, component, ReadValueOfType(entity, reader, GetSerializableBaseType(property.PropertyType)));
            }

            return component;
        }

        private static Type GetSerializableBaseType(Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            if (_baseTypeCache.ContainsKey(type))
            {
                return _baseTypeCache[type];
            }

            var baseType = type;
            if (type.IsEnum)
                baseType = typeof(Enum);
            else if (typeof(IComponent).IsAssignableFrom(type))
                baseType = typeof(IComponent);

            _baseTypeCache[type] = baseType;
            return baseType;
        }

        private static void WriteValueOfType(Entity entity, BinaryWriter writer, Type type, object? value) // TODO abstract binary read/write possibly once have dependency injection
        {
            supportedWriteTypes[type](entity, writer, value);
        }

        private static object? ReadValueOfType(Entity entity, BinaryReader reader, Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            return supportedReadTypes[type](entity, reader);
        }

        private static Dictionary<Type, Func<Entity, BinaryReader, object>> supportedReadTypes = new Dictionary<Type, Func<Entity, BinaryReader, object>>
        {
            { typeof(uint), (e, br) => br.ReadUInt32() },
            { typeof(int), (e, br) => br.ReadInt32() },
            { typeof(bool), (e, br) => br.ReadBoolean() },
            { typeof(float), (e, br) => br.ReadSingle() },
            { typeof(string), (e, br) => br.ReadString() },
            { typeof(Enum), (e, br) => br.ReadInt32() },
            { typeof(IComponent), (e, br) => ReadComponent(e, br) },
        };

        private static Dictionary<Type, Action<Entity, BinaryWriter, object?>> supportedWriteTypes = new Dictionary<Type, Action<Entity, BinaryWriter, object?>>
        {
            { typeof(uint), (e, bw, value) => bw.Write((uint)value) },
            { typeof(int), (e, bw, value) => bw.Write((int)value) },
            { typeof(bool), (e, bw, value) => bw.Write((bool)value) },
            { typeof(float), (e, bw, value) => bw.Write((float)value) },
            { typeof(string), (e, bw, value) => bw.Write((string)value) },
            { typeof(Enum), (e, bw, value) => bw.Write((int)value) },
            { typeof(IComponent), (e, bw, value) => WriteComponent(e, bw, (IComponent)value) },
        };

    }
}
