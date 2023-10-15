using Ozzyria.Game;
using System.IO;
namespace Ozzyria.Networking.Model
{
    public enum ClientMessage
    {
        Join = 0,
        Leave = 1,
        InputUpdate = 2,
        OpenBag = 3,
        EquipItem = 4,
        UnequipItem = 5,
        DropItem = 6,
    }

    class ClientPacket
    {
        public ClientMessage Type { get; set; }
        public int ClientId { get; set; } = 0;
        public byte[] Data { get; set; }
    }

    class BagItemRequest
    {
        public uint BagEntityId { get; set; }
        public int ItemSlot { get; set; }
    }

    class ClientPacketFactory
    {
        public static ClientPacket Parse(byte[] packet)
        {
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return new ClientPacket {
                        Type = (ClientMessage)reader.ReadInt32(),
                        ClientId = reader.ReadInt32(),
                        Data = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position))
                    };
                }
            }
        }

        public static byte[] Join()
        {
            return Pack(ClientMessage.Join, -1, new byte[] { });
        }
        public static byte[] Leave(int clientId)
        {
            return Pack(ClientMessage.Leave, clientId, new byte[] { });
        }

        public static byte[] InputUpdate(int clientId, Input input)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ClientMessage.InputUpdate);
                    writer.Write(clientId);
                    writer.Write(input.MoveUp);
                    writer.Write(input.MoveDown);
                    writer.Write(input.MoveLeft);
                    writer.Write(input.MoveRight);
                    writer.Write(input.TurnLeft);
                    writer.Write(input.TurnRight);
                    writer.Write(input.Attack);
                }
                return m.ToArray();
            }
        }

        public static Input ParseInputData(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return new Input()
                    {
                        MoveUp = reader.ReadBoolean(),
                        MoveDown = reader.ReadBoolean(),
                        MoveLeft = reader.ReadBoolean(),
                        MoveRight = reader.ReadBoolean(),
                        TurnLeft = reader.ReadBoolean(),
                        TurnRight = reader.ReadBoolean(),
                        Attack = reader.ReadBoolean(),
                    };
                }
            }
        }

        public static byte[] OpenBag(int clientId, uint bagEntityId)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ClientMessage.OpenBag);
                    writer.Write(clientId);
                    writer.Write(bagEntityId);
                }
                return m.ToArray();
            }
        }

        public static uint ParseOpenBagData(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return reader.ReadUInt32();
                }
            }
        }

        public static byte[] EquipItem(int clientId, uint bagEntityId, int itemSlot)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ClientMessage.EquipItem);
                    writer.Write(clientId);
                    writer.Write(bagEntityId);
                    writer.Write(itemSlot);
                }
                return m.ToArray();
            }
        }

        public static BagItemRequest ParseEquipItemData(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return new BagItemRequest
                    {
                        BagEntityId = reader.ReadUInt32(),
                        ItemSlot = reader.ReadInt32(),
                    };
                }
            }
        }

        public static byte[] UnequipItem(int clientId, uint bagEntityId, int itemSlot)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ClientMessage.UnequipItem);
                    writer.Write(clientId);
                    writer.Write(bagEntityId);
                    writer.Write(itemSlot);
                }
                return m.ToArray();
            }
        }

        public static BagItemRequest ParseUnequipItemData(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return new BagItemRequest
                    {
                        BagEntityId = reader.ReadUInt32(),
                        ItemSlot = reader.ReadInt32(),
                    };
                }
            }
        }

        public static byte[] DropItem(int clientId, uint bagEntityId, int itemSlot)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ClientMessage.DropItem);
                    writer.Write(clientId);
                    writer.Write(bagEntityId);
                    writer.Write(itemSlot);
                }
                return m.ToArray();
            }
        }

        public static BagItemRequest ParseDropItemData(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    return new BagItemRequest
                    {
                        BagEntityId = reader.ReadUInt32(),
                        ItemSlot = reader.ReadInt32(),
                    };
                }
            }
        }

        private static byte[] Pack(ClientMessage type, int clientId, byte[] data)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)type);
                    writer.Write(clientId);
                    writer.Write(data);
                }
                return m.ToArray();
            }
        }
    }
}
