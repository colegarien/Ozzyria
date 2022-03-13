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

        public EntityManager LoadEntityManager(string resource)
        {
            var entityManager = new EntityManager();
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Content.Loader.Root() + "/Maps/" + resource + ".ozz")))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    entityManager.Register(ReadEntity(reader));
                }
            }

            return entityManager;
        }

        public void SaveEntityManager(string resource, EntityManager entityManager)
        {
            var baseMapsDirectory = Content.Loader.Root() + "/Maps";
            if (!Directory.Exists(baseMapsDirectory))
            {
                Directory.CreateDirectory(baseMapsDirectory);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(baseMapsDirectory + "/" + resource + ".ozz", FileMode.Create)))
            {
                foreach (var entity in entityManager.GetEntities())
                {
                    WriteEntity(writer, entity);
                }
            }
        }


        public static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.Id);
            foreach (var component in entity.GetAllComponents()) // TODO consider the fact that adding/removing property can cause old save to break, think of how to communication what parts of the components were actually saved | could possible just write property name first?
            {
                writer.Write(false); // signal not done reading entity
                WriteComponent(writer, component);
            }
            writer.Write(true); // signal end-of-entity
        }

        private static void WriteComponent(BinaryWriter writer, Component.Component component)
        {
            var options = Reflector.GetOptionsAttribute(component?.GetType());
            if (options == null)
            {
                writer.Write("");
                return;
            }

            writer.Write(options.Name);
            var props = Reflector.GetSavableProperties(component.GetType());
            foreach (var p in props)
            {
                WriteValueOfType(writer, GetSerializableBaseType(p.PropertyType), Reflector.GetPropertyValue(p, component));
            }
        }

        public static Entity ReadEntity(BinaryReader reader)
        {
            var entity = new Entity
            {
                Id = reader.ReadInt32(),
            };
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var isEndOfEntity = reader.ReadBoolean();
                if (isEndOfEntity)
                    break;

                entity.AttachComponent(ReadComponent(reader));
            }

            return entity;
        }

        private static Component.Component ReadComponent(BinaryReader reader)
        {
            var component = Reflector.CreateInstance(reader.ReadString());
            if (component == null)
                return null;

            var props = Reflector.GetSavableProperties(component.GetType());
            foreach (var p in props)
            {
                Reflector.SetPropertyValue(p, component, ReadValueOfType(reader, GetSerializableBaseType(p.PropertyType)));
            }
            return (Component.Component)component;
        }

        private static Type GetSerializableBaseType(Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            if (type.IsEnum)
                return typeof(Enum);
            else if (type.BaseType == typeof(Component.Component))
                return type.BaseType;
            else
                return type;
        }

        private static void WriteValueOfType(BinaryWriter writer, Type type, object? value) // TODO abstract binary read/write possibly once have dependency injection
        {
            supportedWriteTypes[type](writer, value);
        }

        private static object? ReadValueOfType(BinaryReader reader, Type type) // TODO abstract binary read/write possibly once have dependency injection
        {
            return supportedReadTypes[type](reader);
        }

        private static Dictionary<Type, Func<BinaryReader, object>> supportedReadTypes = new Dictionary<Type, Func<BinaryReader, object>>
        {
            { typeof(int), br => br.ReadInt32() },
            { typeof(bool), br => br.ReadBoolean() },
            { typeof(float), br => br.ReadSingle() },
            { typeof(string), br => br.ReadString() },
            { typeof(Enum), br => br.ReadInt32() },
            { typeof(Component.Component), br => ReadComponent(br) },
        };

        private static Dictionary<Type, Action<BinaryWriter, object?>> supportedWriteTypes = new Dictionary<Type, Action<BinaryWriter, object?>>
        {
            { typeof(int), (bw, value) => bw.Write((int)value) },
            { typeof(bool), (bw, value) => bw.Write((bool)value) },
            { typeof(float), (bw, value) => bw.Write((float)value) },
            { typeof(string), (bw, value) => bw.Write((string)value) },
            { typeof(Enum), (bw, value) => bw.Write((int)value) },
            { typeof(Component.Component), (bw, value) => WriteComponent(bw, (Component.Component)value) },
        };

    }
}
