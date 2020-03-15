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

        public Client(string hostname, int port)
        {
            Id = -1;
            connected = false;

            udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Parse(hostname), port);
        }

        public bool Connect()
        {
            if (connected)
            {
                return true;
            }

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

        public void SendInput(PlayerInput input)
        {
            if (!connected)
            {
                return; 
            }

            var inputPacket = ClientPacketFactory.PlayerInput(Id, input);
            udpClient.Send(inputPacket, inputPacket.Length);
        }

        public PlayerState[] GetStates()
        {
            if (!connected)
            {
                return new PlayerState[] { };
            }

            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            return ServerPacketFactory.ParsePlayerState(udpClient.Receive(ref remoteIPEndPoint));
        }

        public void Disconnect()
        {
            if (!connected)
            {
                return;
            }

            var leavePacket = ClientPacketFactory.Leave(Id);
            udpClient.Send(leavePacket, leavePacket.Length);

            udpClient.Close();
            connected = false;
        }
    }
}
