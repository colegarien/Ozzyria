using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ozzyria.Networking
{
    public class ConnectionListener
    {

        public async void Run()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var tcp = new TcpListener(ip, 13000);

            tcp.Start();
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for client...");
                    using (var client = tcp.AcceptTcpClient())
                    {
                        Console.WriteLine("Client connected. Waiting for data.");
                        string message = "";

                        using (var clientStream = client.GetStream())
                        {
                            while (message != null && !message.StartsWith("quit"))
                            {
                                byte[] data = Encoding.UTF8.GetBytes("Send next data: [enter 'quit' to terminate]");
                                clientStream.Write(data, 0, data.Length);

                                byte[] buffer = new byte[1024];
                                clientStream.Read(buffer, 0, buffer.Length);

                                message = Encoding.UTF8.GetString(buffer).Trim();
                                Console.WriteLine(message);
                            }
                        }
                        Console.WriteLine("Closing connection.");
                    }

                }
            }
            finally
            {
                tcp.Stop();
            }
        }

    }
}
