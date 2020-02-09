using System;
using System.Net.Sockets;
using System.Text;

namespace Ozzyria.Networking
{
    public class Connection
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
                        byte[] bytes = new byte[1024];
                        serverStream.Read(bytes, 0, 1024);
                        var message = Encoding.UTF8.GetString(bytes).Trim();
                        if (message.Length > 0)
                        {
                            Console.WriteLine(message);
                        }

                        input = "";
                        while (input.Length <= 0)
                        {
                            input = Console.ReadLine().Trim();
                        }
                        byte[] data = Encoding.UTF8.GetBytes(input);
                        serverStream.Write(data, 0, data.Length);
                    }
                }
            }
        }
    }
}
