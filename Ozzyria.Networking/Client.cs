using Ozzyria.Game;
using Ozzyria.Networking.Model;
using System.Net;
using System.Net.Sockets;

namespace Ozzyria.Networking
{
    public class Client
    {
        public int Id { get; set; }
        private bool connected;
        private UdpClient udpClient;

        public Player[] Players { get; set; }
        public Entity[] Entities { get; set; }

        public Client()
        {
            Id = -1;
            connected = false;

            udpClient = new UdpClient();
        }

        public bool Connect(string hostname, int port)
        {
            if (connected)
            {
                return true;
            }

            try
            {
                IPAddress ip;
                if (!IPAddress.TryParse(hostname, out ip))
                {
                    ip = Dns.GetHostEntry(hostname).AddressList[0];
                }
                udpClient.Connect(ip, port);

                var joinPacket = ClientPacketFactory.Join();
                udpClient.Send(joinPacket, joinPacket.Length);

                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Id = ServerPacketFactory.ParseJoin(udpClient.Receive(ref remoteIPEndPoint));
                if (Id == -1)
                {
                    udpClient.Close();
                    return false;
                }

                connected = true;
                return true;
            }
            catch(System.Exception)
            {
                connected = false;
                return false;
            }
        }

        public void SendInput(Input input)
        {
            if (!connected)
            {
                return; 
            }

            var inputPacket = ClientPacketFactory.InputUpdate(Id, input);
            udpClient.Send(inputPacket, inputPacket.Length);
        }

        public void HandleIncomingMessages()
        {
            if (!connected)
            {
                return;
            }

            while (udpClient.Available > 0)
            {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var packet = ServerPacketFactory.Parse(udpClient.Receive(ref clientEndPoint));
                var messageType = packet.Type;
                var messageData = packet.Data;

                switch (messageType)
                {
                    case ServerMessage.PlayerStateUpdate:
                        Players = ServerPacketFactory.ParsePlayerState(messageData);
                        break;
                    case ServerMessage.EntityUpdate:
                        Entities = ServerPacketFactory.ParseEntityUpdates(messageData);
                        break;
                }
            }
        }

        public void Disconnect()
        {
            if (!connected)
            {
                return;
            }

            try
            {
                var leavePacket = ClientPacketFactory.Leave(Id);
                udpClient.Send(leavePacket, leavePacket.Length);

                udpClient.Close();
                connected = false;
            }
            catch (System.Exception)
            {
                connected = false;
            }
        }
    }
}
