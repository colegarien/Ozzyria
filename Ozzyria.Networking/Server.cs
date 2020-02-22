using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzyria.Networking
{
    public class Server : IDisposable
    {
        private readonly TcpListener tcp;
        private bool _listening = false;

        public Server()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            tcp = new TcpListener(ip, 13000);
        }

        public async Task StartListening()
        {
            if (_listening)
            {
                return;
            }

            tcp.Start();
            _listening = true;

            await Task.Run(() => { 
                while (_listening)
                {
                    TcpClient client = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(HandleClient, client);
                }
            });
        }

        protected static void HandleClient(object obj)
        {
            var client = (TcpClient)obj;
            try
            {
                string message = "";
                using (var clientStream = client.GetStream())
                {
                    while (message != null && !message.StartsWith("quit"))
                    {
                        PacketBuilder.WritePacket(clientStream, "Send next data: [enter 'quit' to terminate]");
                        message = PacketBuilder.ReadPacket(clientStream);
                        Console.WriteLine(message);
                    }
                }
            }
            catch(Exception)
            {
                // Something went wrong LOL
            }
            finally
            {
                client.Close();
            }
        }

        public void StopListening()
        {
            if (_listening)
            {
                _listening = false;
                tcp.Stop();
            }
        }

        public void Dispose()
        {
            StopListening();
        }
    }
}
