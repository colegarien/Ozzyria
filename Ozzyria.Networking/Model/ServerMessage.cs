using Ozzyria.Game;
using Ozzyria.Game.Component;
using System.Collections.Generic;
using System.IO;

namespace Ozzyria.Networking.Model
{
    public enum ServerMessage
    {
        JoinResult = 0,
        JoinReject = 1,
        PlayerStateUpdate = 2,
        EntityUpdate = 3,
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

        private static void WriteMovement(BinaryWriter writer, Movement movement)
        {
            writer.Write(movement.X);
            writer.Write(movement.Y);
            writer.Write(movement.Speed);
            writer.Write(movement.MoveDirection);
            writer.Write(movement.LookDirection);
        }

        private static Movement ReadMovement(BinaryReader reader)
        {
            return new Movement
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Speed = reader.ReadSingle(),
                MoveDirection = reader.ReadSingle(),
                LookDirection = reader.ReadSingle(),
            };
        }

        private static void WriteStats(BinaryWriter writer, Stats stats)
        {
            writer.Write(stats.Health);
            writer.Write(stats.MaxHealth);
            writer.Write(stats.Experience);
            writer.Write(stats.MaxExperience);
        }

        private static Stats ReadStats(BinaryReader reader)
        {
            return new Stats
            {
                Health = reader.ReadInt32(),
                MaxHealth = reader.ReadInt32(),
                Experience = reader.ReadInt32(),
                MaxExperience = reader.ReadInt32()
            };
        }

        private static void WriteExperienceBoost(BinaryWriter writer, ExperienceBoost exp)
        {
            writer.Write(exp.Experience);
            writer.Write(exp.HasBeenAbsorbed);
        }

        private static ExperienceBoost ReadExperienceBoost(BinaryReader reader)
        {
            return new ExperienceBoost
            {
                Experience = reader.ReadInt32(),
                HasBeenAbsorbed = reader.ReadBoolean()
            };
        }

        private static void WriteCombat(BinaryWriter writer, Combat combat)
        {
            writer.Write(combat.Delay.DelayInSeconds);
            writer.Write(combat.Delay.Timer);
            writer.Write(combat.Attacking);
        }

        private static Combat ReadCombat(BinaryReader reader)
        {
            return new Combat
            {
                Delay = new Delay { DelayInSeconds = reader.ReadSingle(), Timer = reader.ReadSingle() },
                Attacking = reader.ReadBoolean()
            };
        }

        private static void WriteEntity(BinaryWriter writer, Entity entity)
        {
            writer.Write(entity.Id);
            foreach(var component in entity.Components.Values)
            {
                writer.Write((int)component.Type());
                if (component.Type() == ComponentType.Movement)
                {
                    WriteMovement(writer, (Movement)component);
                }
                else if (component.Type() == ComponentType.Combat)
                {
                    WriteCombat(writer, (Combat)component);
                }
                else if (component.Type() == ComponentType.Stats)
                {
                    WriteStats(writer, (Stats)component);
                }
                else if (component.Type() == ComponentType.ExperienceBoost)
                {
                    WriteExperienceBoost(writer, (ExperienceBoost)component);
                }
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
                if (componentType == ComponentType.Movement)
                {
                    entity.AttachComponent(ReadMovement(reader));
                }
                else if (componentType == ComponentType.Combat)
                {
                    entity.AttachComponent(ReadCombat(reader));
                }
                else if (componentType == ComponentType.Stats)
                {
                    entity.AttachComponent(ReadStats(reader));
                }
                else if (componentType == ComponentType.ExperienceBoost)
                {
                    entity.AttachComponent(ReadExperienceBoost(reader));
                }
                else if(componentType == ComponentType.None)
                {
                    break; // None type signals end of entity
                }
            }

            return entity;
        }

        public static byte[] PlayerUpdates(Player[] players)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.PlayerStateUpdate);
                    foreach(var player in players)
                    {
                        writer.Write(player.Id);
                        WriteMovement(writer, player.Movement);
                        WriteStats(writer, player.Stats);
                        WriteCombat(writer, player.Combat);
                    }
                }
                return m.ToArray();
            }
        }

        public static Player[] ParsePlayerState(byte[] packet)
        {
            var players = new List<Player>();

            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while(reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        players.Add(new Player
                        {
                            Id = reader.ReadInt32(),
                            Movement = ReadMovement(reader),
                            Stats = ReadStats(reader),
                            Combat = ReadCombat(reader)
                        });
                    }
                }
            }

            return players.ToArray();
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
