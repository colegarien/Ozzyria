using Ozzyria.Game;
using Grecs;
using Ozzyria.Networking.Model;
using System.Linq;
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

        public void RequestBagContents(uint bagEntityId)
        {
            if (!connected)
            {
                return;
            }

            try
            {
                var openBagPacket = ClientPacketFactory.OpenBag(Id, bagEntityId);
                udpClient.Send(openBagPacket, openBagPacket.Length);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        public void RequestEquipItem(uint bagEntityId, int itemSlot)
        {
            if (!connected)
            {
                return;
            }

            try
            {
                var equipItemPacket = ClientPacketFactory.EquipItem(Id, bagEntityId, itemSlot);
                udpClient.Send(equipItemPacket, equipItemPacket.Length);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        public void RequestUnequipItem(uint bagEntityId, int itemSlot)
        {
            if (!connected)
            {
                return;
            }

            try
            {
                var equipItemPacket = ClientPacketFactory.UnequipItem(Id, bagEntityId, itemSlot);
                udpClient.Send(equipItemPacket, equipItemPacket.Length);
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }

        public void RequestDropItem(uint bagEntityId, int itemSlot)
        {
            if (!connected)
            {
                return;
            }

            try
            {
                var equipItemPacket = ClientPacketFactory.DropItem(Id, bagEntityId, itemSlot);
                udpClient.Send(equipItemPacket, equipItemPacket.Length);
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
                        case ServerMessage.BagContents:
                            var response = ServerPacketFactory.ParseBagContents(messageData);
                            if (!response.Failed)
                            {
                                var bagEntity = context.GetEntity(response.BagEntityId);
                                if (bagEntity != null && bagEntity.HasComponent(typeof(Game.Components.Bag)))
                                {
                                    var bag = (Game.Components.Bag)bagEntity.GetComponent(typeof(Game.Components.Bag));
                                    bag.Contents = response.Contents;
                                }
                            }
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
