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

        public Client(string hostname, int port)
        {
            Id = -1;
            connected = false;

            udpClient = new UdpClient();
            IPAddress ip;
            if (!IPAddress.TryParse(hostname, out ip))
            {
                ip = Dns.GetHostEntry(hostname).AddressList[0];
            }
            udpClient.Connect(ip, port);
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

        public void SendInput(Input input)
        {
            if (!connected)
            {
                return; 
            }

            var inputPacket = ClientPacketFactory.InputUpdate(Id, input);
            udpClient.Send(inputPacket, inputPacket.Length);
        }

        public Player[] GetPlayers()
        {
            if (!connected)
            {
                return new Player[] { };
            }

            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            return ServerPacketFactory.ParsePlayerState(udpClient.Receive(ref remoteIPEndPoint));
        }

        public ExperienceOrb[] GetExperienceOrbs()
        {
            if (!connected)
            {
                return new ExperienceOrb[] { };
            }

            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            return ServerPacketFactory.ParseExperenceOrbs(udpClient.Receive(ref remoteIPEndPoint));
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
