using Ozzyria.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ozzyria.Networking.Model
{
    public enum ServerMessage
    {
        JoinResult = 0,
        JoinReject = 1,
        StateUpdate = 2,
    }

    class ServerPacketFactory
    {
        public static byte[] Join(int clientId)
        {
            return clientId <= -1
                ? Encoding.ASCII.GetBytes($"{(int)ServerMessage.JoinReject}>")
                : Encoding.ASCII.GetBytes($"{(int)ServerMessage.JoinResult}>{clientId}");
        }

        public static int ParseJoin(byte[] packet)
        {
            var packetString = Encoding.ASCII.GetString(packet);
            var messageType = Enum.Parse<ServerMessage>(packetString.Substring(0, packetString.IndexOf('>')));
            if (messageType != ServerMessage.JoinResult)
            {
                return -1;
            }

            return int.Parse(packetString.Substring(packetString.IndexOf('>') + 1));
        }


        public static byte[] PlayerUpdates(Player[] players)
        {
            var serializedPlayerState = Encoding.ASCII.GetBytes($"{(int)ServerMessage.StateUpdate}>").ToList();
            foreach (var state in players)
            {
                if (state == null)
                {
                    continue;
                }

                serializedPlayerState = serializedPlayerState.Concat(state.Serialize()).Concat(Encoding.ASCII.GetBytes("@")).ToList();
            }

            return serializedPlayerState.ToArray();
        }

        public static Player[] ParsePlayerState(byte[] packet)
        {
            var players = new List<Player>();

            var packetString = Encoding.ASCII.GetString(packet);
            var messageType = Enum.Parse<ServerMessage>(packetString.Substring(0, packetString.IndexOf('>')));
            if (messageType == ServerMessage.StateUpdate)
            {
                var serializedStates = packetString.Substring(packetString.IndexOf('>') + 1).Split('@');
                foreach (var serializedState in serializedStates)
                {
                    if (serializedState.Trim().Length == 0)
                    {
                        continue;
                    }

                    players.Add(Player.Deserialize(Encoding.ASCII.GetBytes(serializedState)));
                }
            }

            return players.ToArray();
        }

    }

}
