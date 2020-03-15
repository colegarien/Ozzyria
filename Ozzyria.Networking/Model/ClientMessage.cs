using System;
using System.Linq;
using System.Text;

namespace Ozzyria.Networking.Model
{
    public enum ClientMessage
    {
        Join = 0,
        Leave = 1,
        InputUpdate = 2,
    }

    class ClientPacket
    {
        public ClientMessage Type { get; set; }
        public int ClientId { get; set; } = -1;
        public byte[] Data { get; set; }
    }

    class ClientPacketFactory
    {
        public static ClientPacket Parse(byte[] packet)
        {
            var packetString = Encoding.ASCII.GetString(packet);
            return new ClientPacket
            {
                Type = Enum.Parse<ClientMessage>(packetString.Substring(0, packetString.IndexOf('>'))),
                ClientId = int.Parse(packetString.Substring(packetString.IndexOf('>') + 1, packetString.IndexOf('#') - (packetString.IndexOf('>') + 1))),
                Data = Encoding.ASCII.GetBytes(packetString.Substring(packetString.IndexOf('#') + 1))
            };
        }

        public static byte[] Join()
        {
            return Encoding.ASCII.GetBytes($"{(int)ClientMessage.Join}>-1#");
        }
        public static byte[] Leave(int clientId)
        {
            return Encoding.ASCII.GetBytes($"{(int)ClientMessage.Leave}>{clientId}#");
        }

        public static byte[] PlayerInput(int clientId, PlayerInput input)
        {
            return Encoding.ASCII.GetBytes($"{(int)ClientMessage.InputUpdate}>{clientId}#").Concat(input.Serialize()).ToArray();
        }
    }
}
