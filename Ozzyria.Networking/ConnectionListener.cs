using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzyria.Networking
{
    public class ConnectionListener : IDisposable
    {
        private readonly TcpListener tcp;
        private bool _listening = false;

        public ConnectionListener()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            tcp = new TcpListener(ip, 13000);
        }

        public async Task Start()
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
                    Console.WriteLine("Waiting for client...");
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
            }
            catch(Exception)
            {
                // Something went wrong LOL
            }
            finally
            {
                client.Close();
                Console.WriteLine("Closing connection.");
            }
        }

        public void Stop()
        {
            _listening = false;
            tcp.Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
