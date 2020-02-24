using Ozzyria.Networking.Model;
using System;
using System.Net.Sockets;

namespace Ozzyria.Networking
{
    class ClientHandler
    {
        private readonly int _id;
        private bool _connected = false;
        private TcpClient _client = null;

        public ClientHandler(int id)
        {
            _id = id;
        }

        public bool IsConnected()
        {
            return _connected;
        }

        public void Handle(object obj)
        {
            _connected = true;
            _client = (TcpClient)obj;

            try
            {
                using (var clientStream = _client.GetStream())
                {
                    Console.WriteLine($"{_id} Connected");
                    var running = AuthenticateClient(clientStream);
                    while (running)
                    {
                        PacketFactory.WritePacket(clientStream, new Packet { MessageType = MessageType.CHAT, Data = "Send next data: [enter 'quit' to terminate]" });
                        var packet = PacketFactory.ReadPacket(clientStream);

                        running = packet.MessageType != MessageType.CLIENT_LEAVE;
                        Console.WriteLine(packet.Data);
                    }
                }
            }
            catch (Exception e)
            {
                // TODO : add logger
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine($"{_id} Closed");
                _connected = false;
                _client.Close();
            }
        }

        private bool AuthenticateClient(NetworkStream clientStream)
        {
            PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = MessageType.HAS_SLOT, Data = "Slot Available" });
            var packet = PacketFactory.ReadPacket(clientStream);
            if (packet.MessageType != MessageType.CLIENT_JOIN)
            {
                Console.WriteLine($"{_id} Rejected");
                PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = MessageType.SERVER_REJECT, Data = "Bad Request" });
                return false;
            }

            var userData = packet.Data;
            var username = userData.Substring(0, userData.IndexOf(":"));
            var password = userData.Substring(userData.IndexOf(":") + 1);
            if (username != "username" || password != "password")
            {
                Console.WriteLine($"{_id} Rejected");
                PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = MessageType.SERVER_REJECT, Data = "Bad Credentials" });
                return false;
            }
            PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = MessageType.SERVER_JOIN, Data = "id:" + _id });

            Console.WriteLine($"{_id} Authorized");
            return true;
        }
    }
}
