using System;
using System.Net.Sockets;

namespace Ozzyria.Networking
{
    public class Client
    {

        public void Start()
        {
            using (var client = new TcpClient("localhost", 13000))
            {
                using (var serverStream = client.GetStream())
                {
                    var clientId = AuthenticateToServer(serverStream);

                    var running = clientId != -1;
                    while (running)
                    {
                        var packet = PacketFactory.ReadPacket(serverStream);
                        if (packet.MessageType == Model.MessageType.CHAT && packet.Data.Length > 0)
                        {
                            Console.WriteLine(packet.Data);
                        }

                        var input = "";
                        while (input.Length <= 0)
                        {
                            input = Console.ReadLine().Trim();
                        }
                        running = !input.Contains("quit");
                        PacketFactory.WritePacket(serverStream, new Model.Packet { 
                            MessageType = !running ? Model.MessageType.CLIENT_LEAVE : Model.MessageType.CHAT,
                            Data = !running ? "Quit" : input
                        });
                    }
                }
            }
        }

        private int AuthenticateToServer(NetworkStream serverStream)
        {
            var packet = PacketFactory.ReadPacket(serverStream);
            if(packet.MessageType != Model.MessageType.HAS_SLOT)
            {
                Console.WriteLine(packet.Data);
                return -1;
            }

            do
            {
                Console.WriteLine("What is your username:");
                var input = "";
                while (input.Length <= 0)
                {
                    input = Console.ReadLine().Trim();
                }
                var username = input;

                Console.WriteLine("What is your password:");
                input = "";
                while (input.Length <= 0)
                {
                    input = Console.ReadLine().Trim();
                }
                var password = input;

                PacketFactory.WritePacket(serverStream, new Model.Packet
                {
                    MessageType = Model.MessageType.CLIENT_JOIN,
                    Data = username + ":" + password
                });

                packet = PacketFactory.ReadPacket(serverStream);
                if (packet.MessageType == Model.MessageType.SERVER_REJECT)
                {
                    Console.WriteLine(packet.Data);
                    return -1;
                }
                else if(packet.MessageType == Model.MessageType.JOIN_FAILED)
                {
                    Console.WriteLine(packet.Data);

                }
            } while (packet.MessageType != Model.MessageType.SERVER_JOIN);

            int id;
            if(!int.TryParse(packet.Data.Substring(packet.Data.IndexOf(":") + 1), out id))
            {
                id = -1;
            }

            return id;

        }
    }
}
