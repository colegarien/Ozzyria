using Ozzyria.Game;
using System.Collections.Generic;
using System.IO;

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

    class ServerPacketFactory
    {
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
                        writer.Write(player.X);
                        writer.Write(player.Y);
                        writer.Write(player.Speed);
                        writer.Write(player.MoveDirection);
                        writer.Write(player.LookDirection);
                        writer.Write(player.Experience);
                        writer.Write(player.MaxExperience);
                        writer.Write(player.Health);
                        writer.Write(player.MaxHealth);
                        writer.Write(player.AttackDelay);
                        writer.Write(player.AttackTimer);
                        writer.Write(player.Attacking);
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
                    var packetType = (ClientMessage)reader.ReadInt32();
                    while(reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        players.Add(new Player
                        {
                            Id = reader.ReadInt32(),
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Speed = reader.ReadSingle(),
                            MoveDirection = reader.ReadSingle(),
                            LookDirection = reader.ReadSingle(),
                            Experience = reader.ReadInt32(),
                            MaxExperience = reader.ReadInt32(),
                            Health = reader.ReadInt32(),
                            MaxHealth = reader.ReadInt32(),
                            AttackDelay = reader.ReadSingle(),
                            AttackTimer = reader.ReadSingle(),
                            Attacking = reader.ReadBoolean()
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
                    foreach (var player in orbs)
                    {
                        writer.Write(player.X);
                        writer.Write(player.Y);
                        writer.Write(player.Speed);
                        writer.Write(player.Experience);
                        writer.Write(player.HasBeenAbsorbed);
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
                    var packetType = (ClientMessage)reader.ReadInt32();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        orbs.Add(new ExperienceOrb
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Speed = reader.ReadSingle(),
                            Experience = reader.ReadInt32(),
                            HasBeenAbsorbed = reader.ReadBoolean()
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
                        writer.Write(slime.X);
                        writer.Write(slime.Y);
                        writer.Write(slime.Speed);
                        writer.Write(slime.MoveDirection);
                        writer.Write(slime.LookDirection);
                        writer.Write(slime.Health);
                        writer.Write(slime.MaxHealth);
                        writer.Write(slime.AttackDelay);
                        writer.Write(slime.AttackTimer);
                        writer.Write(slime.Attacking);
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
                    var packetType = (ClientMessage)reader.ReadInt32();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        slimes.Add(new Slime
                        {
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            Speed = reader.ReadSingle(),
                            MoveDirection = reader.ReadSingle(),
                            LookDirection = reader.ReadSingle(),
                            Health = reader.ReadInt32(),
                            MaxHealth = reader.ReadInt32(),
                            AttackDelay = reader.ReadSingle(),
                            AttackTimer = reader.ReadSingle(),
                            Attacking = reader.ReadBoolean()
                        });
                    }
                }
            }

            return slimes.ToArray();
        }

    }

}
