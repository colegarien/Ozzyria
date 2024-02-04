using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using System.Collections.Generic;
using System.IO;

namespace Ozzyria.Networking.Model
{
    public enum ServerMessage
    {
        JoinResult = 0,
        JoinReject = 1,
        EntityUpdate = 2,
        EntityRemoval = 3,
        AreaChanged = 4,
        BagContents = 5,
    }

    class ServerPacket
    {
        public ServerMessage Type { get; set; }
        public byte[] Data { get; set; }
    }

    class BagContentsResponse
    {
        public bool Failed { get; set; }
        public uint BagEntityId { get; set; }
        public List<Entity> Contents { get; set; }
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

        public static byte[] EntityUpdates(Entity[] entities)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.EntityUpdate);
                    foreach (var entity in entities)
                    {
                        WorldPersistence.WriteEntity(writer, entity);
                    }
                }

                return m.ToArray();
            }
        }

        public static void ParseEntityUpdates(EntityContext context, byte[] packet)
        {
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        WorldPersistence.ReadEntity(context, reader);
                    }
                }
            }
        }

        public static byte[] EntityRemovals(EntityContext context)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.EntityRemoval);
                    foreach (var id in context.GetRecentlyDestroyed()) {
                        writer.Write(id);
                    }
                }

                return m.ToArray();
            }
        }

        public static void ParseEntityRemovals(EntityContext context, byte[] packet)
        {
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        uint id = reader.ReadUInt32();
                        context.DestroyEntity(id);
                    }
                }
            }
        }

        public static byte[] AreaChanged(int clientId, string oldArea, string newArea)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.AreaChanged);
                    writer.Write(clientId);
                    writer.Write(oldArea);
                    writer.Write(newArea);
                }

                return m.ToArray();
            }
        }

        public static void ParseAreaChanged(EntityContext context, byte[] packet)
        {
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        int clientId = reader.ReadInt32();
                        string oldArea = reader.ReadString();
                        string newArea = reader.ReadString();

                        // clear out context (entity updates should rehydrate everything)
                        foreach(var entity in context.GetEntities())
                        {
                            context.DestroyEntity(entity.id);
                        }
                    }
                }
            }
        }

        public static byte[] BagContents(uint bagEntityId, Entity[] entities)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.BagContents);
                    writer.Write(bagEntityId);
                    writer.Write(false);
                    foreach (var entity in entities)
                    {
                        WorldPersistence.WriteDetachedEntity(writer, entity);
                    }
                }

                return m.ToArray();
            }
        }

        public static byte[] CannotOpenBagContents(uint bagEntityId)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.BagContents);
                    writer.Write(bagEntityId);
                    writer.Write(true);
                }

                return m.ToArray();
            }
        }

        public static BagContentsResponse ParseBagContents(byte[] packet)
        {
            var response = new BagContentsResponse();
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    response.BagEntityId = reader.ReadUInt32();
                    response.Failed = reader.ReadBoolean();
                    response.Contents = new List<Entity>();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        response.Contents.Add(WorldPersistence.ReadDetachedEntity(reader));
                    }
                }
            }

            return response;
        }
    }

}
