using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Component.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ozzyria.Networking.Model
{
    public enum ServerMessage
    {
        JoinResult = 0,
        JoinReject = 1,
        EntityUpdate = 2,
    }

    class ServerPacket
    {
        public ServerMessage Type { get; set; }
        public byte[] Data { get; set; }
    }

    class ServerPacketFactory
    {
        public static ServerPacket Parse(byte[] packet)
        {
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return new ServerPacket
                    {
                        Type = (ServerMessage)reader.ReadInt32(),
                        Data = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position))
                    };
                }
            }
        }


        public static byte[] Join(int clientId)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    var messageType = clientId <= -1 ? ServerMessage.JoinReject : ServerMessage.JoinResult;
                    writer.Write((int)messageType);
                    writer.Write(clientId);
                }
                return m.ToArray();
            }
        }

        public static int ParseJoin(byte[] packet)
        {
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    var messageType = (ServerMessage)reader.ReadInt32();
                    if (messageType != ServerMessage.JoinResult)
                    {
                        return -1;
                    }

                    return reader.ReadInt32();
                }
            }
        }

        private static void WriteComponent(BinaryWriter writer, Component component)
        {
            var options = (OptionsAttribute)component.GetType().GetCustomAttributes(typeof(OptionsAttribute), false).FirstOrDefault();
            if (options == null)
            {
                writer.Write(""); // TODO this is a hackity hack because currently writing Components that are missing OptionsAttribute
                return;
            }

            writer.Write(options.Name);

            var props = component.GetType().GetProperties()
                .Where(prop => System.Attribute.IsDefined(prop, typeof(SavableAttribute)))
                .OrderBy(p => p.Name);
            foreach (var p in props)
            {
                var type = p.PropertyType.IsEnum ? typeof(Enum) :
                    (p.PropertyType.BaseType == typeof(Component) ? typeof(Component) : p.PropertyType);
                supportedWriteTypes[type](writer, p.GetValue(component));
            }
        }

        private static Component ReadComponent(BinaryReader reader)
        {
            var componentType = reader.ReadString();
            if (!componentTypes.ContainsKey(componentType))
                return null;

            var component = Activator.CreateInstance(componentTypes[componentType]);
            var props = component.GetType().GetProperties()
                .Where(prop => System.Attribute.IsDefined(prop, typeof(SavableAttribute)))
                .OrderBy(p => p.Name);

            foreach (var p in props)
            {
                var type = p.PropertyType.IsEnum ? typeof(Enum) :
                    (p.PropertyType.BaseType == typeof(Component) ? typeof(Component) : p.PropertyType);

                p.SetValue(component, supportedReadTypes[type](reader), null);
            }
            return (Component)component;
        }

        private static Dictionary<string, Type> componentTypes = new Dictionary<string, Type>{
            {"BoundingBox", typeof(BoundingBox) },
            {"BoundingCircle", typeof(BoundingCircle) },
            {"Combat", typeof(Combat) },
            {"Delay", typeof(Delay) },
            {"ExperienceBoost", typeof(ExperienceBoost) },
            {"Input", typeof(Input) },
            {"Movement", typeof(Movement) },
            {"Renderable", typeof(Renderable) },
            {"Stats", typeof(Stats) },
        }; // TODO build this on instantiation / boot of program to avoid all the reflection slowness

        private static Dictionary<Type, Func<BinaryReader, object>> supportedReadTypes = new Dictionary<Type, Func<BinaryReader, object>>
        {
            { typeof(int), br => br.ReadInt32() },
            { typeof(bool), br => br.ReadBoolean() },
            { typeof(float), br => br.ReadSingle() },
            { typeof(Enum), br => br.ReadInt32() },
            { typeof(Component), br => ReadComponent(br) },
        };

        private static Dictionary<Type, Action<BinaryWriter, object?>> supportedWriteTypes = new Dictionary<Type, Action<BinaryWriter, object?>>
        {
            { typeof(int), (bw, value) => bw.Write((int)value) },
            { typeof(bool), (bw, value) => bw.Write((bool)value) },
            { typeof(float), (bw, value) => bw.Write((float)value) },
            { typeof(Enum), (bw, value) => bw.Write((int)value) },
            { typeof(Component), (bw, value) => WriteComponent(bw, (Component)value) },
        };

        private static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.Id);
            foreach (var component in entity.GetAllComponents())
            {
                writer.Write((int)component.Type());
                WriteComponent(writer, component);
            }
            writer.Write((int)ComponentType.None); // signal end-of-entity with empty component
        }

        private static Entity ReadEntity(BinaryReader reader)
        {
            var entity = new Entity
            {
                Id = reader.ReadInt32(),
            };
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var componentType = (ComponentType)reader.ReadInt32();
                if (componentType == ComponentType.None)
                    break; // None type signals end of entity
                else
                    entity.AttachComponent(ReadComponent(reader));
            }

            return entity;
        }

        public static byte[] EntityUpdates(Entity[] entities)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.EntityUpdate);
                    foreach (var entity in entities)
                    {
                        WriteEntity(writer, entity);
                    }
                }

                return m.ToArray();
            }
        }

        public static Entity[] ParseEntityUpdates(byte[] packet)
        {
            var entities = new List<Entity>();
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        entities.Add(ReadEntity(reader));
                    }
                }
            }

            return entities.ToArray();
        }

    }

}
