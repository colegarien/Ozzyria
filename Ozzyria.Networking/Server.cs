using System;
using System.Linq;
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
        private ClientHandler[] handlers = new ClientHandler[2];

        public Server()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            tcp = new TcpListener(ip, 13000);

            for(var i = 0; i < handlers.Length; i++)
            {
                handlers[i] = new ClientHandler(i);
            }
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

                    var openHandler = handlers.FirstOrDefault(h => !h.IsConnected());
                    if(openHandler == null)
                    {
                        // TODO make less weird
                        PacketFactory.WritePacket(client.GetStream(), new Model.Packet { MessageType = Model.MessageType.SERVER_REJECT, Data = "Bad Credentials" });
                        client.Close();
                        continue;
                    }

                    ThreadPool.QueueUserWorkItem(openHandler.Handle, client);
                }
            });
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
