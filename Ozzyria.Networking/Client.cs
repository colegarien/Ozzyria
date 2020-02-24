using System;
using System.Net.Sockets;

namespace Ozzyria.Networking
{
    public class Client
    {
        public void Start()
        {
            var input = "";
            using (var client = new TcpClient("localhost", 13000))
            {
                using (var serverStream = client.GetStream())
                {
                    PacketFactory.WritePacket(serverStream, new Model.Packet
                    {
                        MessageType = Model.MessageType.CLIENT_JOIN,
                        Data = "username:password"
                    });

                    var packet = PacketFactory.ReadPacket(serverStream);
                    if(packet.MessageType != Model.MessageType.SERVER_JOIN)
                    {
                        Console.WriteLine(packet.Data);
                        return;
                    }

                    var clientId = packet.Data; // TODO do something with this
                    while (!input.Contains("quit"))
                    {
                        packet = PacketFactory.ReadPacket(serverStream);
                        if (packet.MessageType == Model.MessageType.CHAT && packet.Data.Length > 0)
                        {
                            Console.WriteLine(packet.Data);
                        }

                        input = "";
                        while (input.Length <= 0)
                        {
                            input = Console.ReadLine().Trim();
                        }
                        PacketFactory.WritePacket(serverStream, new Model.Packet { 
                            MessageType = input.Contains("quit") ? Model.MessageType.CLIENT_LEAVE : Model.MessageType.CHAT,
                            Data = input
                        });
                    }
                }
            }
        }
    }
}
