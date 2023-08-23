using Ozzyria.Game;
using Ozzyria.Game.ECS;
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

        public Client()
        {
            Id = -1;
            connected = false;

            udpClient = new UdpClient();
        }

        public bool IsConnected()
        {
            return connected;
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

                Id= ServerPacketFactory.ParseJoin(udpClient.Receive(ref remoteIPEndPoint));
                if (Id == -1)
                {
                    udpClient.Close();
                    return false;
                }

                connected = true;
                return true;
            }
            catch(SocketException)
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

            try
            {
                var inputPacket = ClientPacketFactory.InputUpdate(Id, input);
                udpClient.Send(inputPacket, inputPacket.Length);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        public void HandleIncomingMessages(EntityContext context)
        {
            if (!connected)
            {
                return;
            }

            try
            {
                while (udpClient.Available > 0)
                {
                    var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var packet = ServerPacketFactory.Parse(udpClient.Receive(ref clientEndPoint));
                    var messageType = packet.Type;
                    var messageData = packet.Data;

                    switch (messageType)
                    {
                        case ServerMessage.EntityUpdate:
                            ServerPacketFactory.ParseEntityUpdates(context, messageData);
                            break;
                        case ServerMessage.EntityRemoval:
                            ServerPacketFactory.ParseEntityRemovals(context, messageData);
                            break;
                        case ServerMessage.AreaChanged:
                            ServerPacketFactory.ParseAreaChanged(context, messageData);
                            break;
                    }
                }
            }
            catch (SocketException)
            {
                Disconnect();
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
