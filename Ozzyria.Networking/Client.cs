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
                    while (input != "quit")
                    {
                        var message = PacketBuilder.ReadPacket(serverStream);
                        if (message.Length > 0)
                        {
                            Console.WriteLine(message.Trim());
                        }

                        input = "";
                        while (input.Length <= 0)
                        {
                            input = Console.ReadLine().Trim();
                        }
                        PacketBuilder.WritePacket(serverStream, input);
                    }
                }
            }
        }
    }
}
