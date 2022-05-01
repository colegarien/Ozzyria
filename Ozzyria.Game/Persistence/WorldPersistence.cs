using Ozzyria.Game.ECS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ozzyria.Game.Persistence
{
    public class WorldPersistence
    {

        public TileMap LoadMap(string mapName)
        {
            return JsonSerializer.Deserialize<TileMap>(File.ReadAllText(Content.Loader.Root() + "/Maps/" + mapName + ".ozz"), JsonOptionsFactory.GetOptions());
        }

        public void SaveMap(string resource, TileMap map)
        {
            var baseMapsDirectory = Content.Loader.Root() + "/Maps";
            if (!Directory.Exists(baseMapsDirectory))
            {
                Directory.CreateDirectory(baseMapsDirectory);
            }

            File.WriteAllText(baseMapsDirectory + "/" + resource + ".ozz", JsonSerializer.Serialize(map, JsonOptionsFactory.GetOptions()));
        }

        public void LoadContext(EntityContext context, string resource)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Content.Loader.Root() + "/Maps/" + resource + ".ozz")))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ReadEntity(context, reader);
                }
            }
        }

        public void SaveContext(string resource, EntityContext context)
        {
            var baseMapsDirectory = Content.Loader.Root() + "/Maps";
            if (!Directory.Exists(baseMapsDirectory))
            {
                Directory.CreateDirectory(baseMapsDirectory);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(baseMapsDirectory + "/" + resource + ".ozz", FileMode.Create)))
            {
                foreach (var entity in context.GetEntities())
                {
                    WriteEntity(writer, entity);
                }
            }
        }


        public static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.id);
            foreach (var component in entity.GetComponents()) // TODO OZ-29 consider the fact that adding/removing property can cause old save to break, think of how to communication what parts of the components were actually saved | could possible just write property name first?
            {
                writer.Write(false); // signal not done reading entity
                WriteComponent(entity, writer, component);
            }
            writer.Write(true); // signal end-of-entity
        }

        private static void WriteComponent(Entity entity, BinaryWriter writer, IComponent component)
        {
            var name = component?.GetType()?.ToString() ?? null;
            if (name == null)
            {
                writer.Write("");
                return;
            }

            writer.Write(name);
            var props = Reflector.GetSavableProperties(component.GetType());
            foreach (var p in props)
            {
                WriteValueOfType(entity, writer, GetSerializableBaseType(p.PropertyType), Reflector.GetPropertyValue(p, component));
            }
        }

        public static Entity ReadEntity(EntityContext context, BinaryReader reader)
        {
            var id = reader.ReadUInt32();
            var entity = context.CreateEntity(id);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var isEndOfEntity = reader.ReadBoolean();
                if (isEndOfEntity)
                    break;

                ReadComponent(entity, reader);
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
            foreach (var p in props)
            {
                Reflector.SetPropertyValue(p, component, ReadValueOfType(entity, reader, GetSerializableBaseType(p.PropertyType)));
            }
            return component;
        }

        private static Type GetSerializableBaseType(Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            if (type.IsEnum)
                return typeof(Enum);
            else if (type.BaseType == typeof(IComponent) || type.BaseType == typeof(Component))
                return typeof(IComponent);
            else
                return type;
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
            { typeof(int), (e, br) => br.ReadInt32() },
            { typeof(bool), (e, br) => br.ReadBoolean() },
            { typeof(float), (e, br) => br.ReadSingle() },
            { typeof(string), (e, br) => br.ReadString() },
            { typeof(Enum), (e, br) => br.ReadInt32() },
            { typeof(IComponent), (e, br) => ReadComponent(e, br) },
        };

        private static Dictionary<Type, Action<Entity, BinaryWriter, object?>> supportedWriteTypes = new Dictionary<Type, Action<Entity, BinaryWriter, object?>>
        {
            { typeof(int), (e, bw, value) => bw.Write((int)value) },
            { typeof(bool), (e, bw, value) => bw.Write((bool)value) },
            { typeof(float), (e, bw, value) => bw.Write((float)value) },
            { typeof(string), (e, bw, value) => bw.Write((string)value) },
            { typeof(Enum), (e, bw, value) => bw.Write((int)value) },
            { typeof(IComponent), (e, bw, value) => WriteComponent(e, bw, (IComponent)value) },
        };

    }
}
