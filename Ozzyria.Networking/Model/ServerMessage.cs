using Ozzyria.Game;
using Ozzyria.Game.Component;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ozzyria.Networking.Model
{
    public enum ServerMessage
    {
        JoinResult = 0,
        JoinReject = 1,
        PlayerStateUpdate = 2,
        ExperienceOrbsUpdate = 3,
        SlimeUpdate = 4,
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
                writer.Write(component.GetType().Name);
                if (component.GetType() == typeof(Movement))
                {
                    WriteMovement(writer, (Movement)component);
                }
                else if (component.GetType() == typeof(Combat))
                {
                    WriteCombat(writer, (Combat)component);
                }
                else if (component.GetType() == typeof(Stats))
                {
                    WriteStats(writer, (Stats)component);
                }
                else if (component.GetType() == typeof(ExperienceBoost))
                {
                    WriteExperienceBoost(writer, (ExperienceBoost)component);
                }
            }
            writer.Write("DONE");
        }

        private static Entity ReadEntity(BinaryReader reader)
        {
            var entity = new Entity
            {
                Id = reader.ReadInt32(),
            };
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var componentName = reader.ReadString();
                if (componentName == typeof(Movement).Name)
                {
                    entity.AttachComponent(ReadMovement(reader));
                }
                else if (componentName == typeof(Combat).Name)
                {
                    entity.AttachComponent(ReadCombat(reader));
                }
                else if (componentName == typeof(Stats).Name)
                {
                    entity.AttachComponent(ReadStats(reader));
                }
                else if (componentName == typeof(ExperienceBoost).Name)
                {
                    entity.AttachComponent(ReadExperienceBoost(reader));
                }
                else if(componentName == "DONE")
                {
                    break;
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

        public static byte[] ExperienceOrbUpdates(ExperienceOrb[] orbs)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.ExperienceOrbsUpdate);
                    foreach (var orb in orbs)
                    {
                        WriteEntity(writer, new Entity
                        {
                            Id = 1,
                            Components = new Dictionary<System.Type, IComponent>
                            {
                                { typeof(Movement), orb.Movement },
                                { typeof(ExperienceBoost), orb.Boost },
                            }
                        });
                    }
                }
                return m.ToArray();
            }
        }

        public static ExperienceOrb[] ParseExperenceOrbs(byte[] packet)
        {
            var orbs = new List<ExperienceOrb>();

            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var entity = ReadEntity(reader);
                        orbs.Add(new ExperienceOrb
                        {
                            Movement = (Movement)entity.Components[typeof(Movement)],
                            Boost = (ExperienceBoost)entity.Components[typeof(ExperienceBoost)],
                        });
                    }
                }
            }

            return orbs.ToArray();
        }

        public static byte[] SlimeUpdates(Slime[] slimes)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((int)ServerMessage.SlimeUpdate);
                    foreach (var slime in slimes)
                    {
                        WriteEntity(writer, new Entity
                        {
                            Id = 1,
                            Components = new Dictionary<System.Type, IComponent>
                            {
                                { typeof(Movement), slime.Movement },
                                { typeof(Stats), slime.Stats },
                                { typeof(Combat), slime.Combat },
                            }
                        });
                    }
                }
                return m.ToArray();
            }
        }

        public static Slime[] ParseSlimeState(byte[] packet)
        {
            var slimes = new List<Slime>();

            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var entity = ReadEntity(reader);
                        slimes.Add(new Slime
                        {
                            Movement = (Movement)entity.Components[typeof(Movement)],
                            Stats = (Stats)entity.Components[typeof(Stats)],
                            Combat = (Combat)entity.Components[typeof(Combat)],
                        });
                    }
                }
            }

            return slimes.ToArray();
        }

    }

}
