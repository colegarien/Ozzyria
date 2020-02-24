using System;
using System.Collections.Generic;
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

        protected void HandleClient(object obj)
        {
            var client = (TcpClient)obj;
            try
            {
                using (var clientStream = client.GetStream())
                {
                    var packet = PacketFactory.ReadPacket(clientStream);
                    if(packet.MessageType != Model.MessageType.CLIENT_JOIN)
                    {
                        PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = Model.MessageType.SERVER_REJECT, Data = "Bad Request" });
                        return;
                    }

                    var userData = packet.Data;
                    var username = packet.Data.Substring(0, packet.Data.IndexOf(":"));
                    var password = packet.Data.Substring(packet.Data.IndexOf(":")+1);
                    if(username != "username" || password != "password")
                    {
                        PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = Model.MessageType.SERVER_REJECT, Data = "Bad Credentials" });
                        return;
                    }
                    PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = Model.MessageType.SERVER_JOIN, Data = "client stream id here" }); // TODO implement client stream number

                    var done = false;
                    while (!done)
                    {
                        PacketFactory.WritePacket(clientStream, new Model.Packet { MessageType = Model.MessageType.CHAT, Data = "Send next data: [enter 'quit' to terminate]" });
                        packet = PacketFactory.ReadPacket(clientStream);

                        done = packet.MessageType == Model.MessageType.CLIENT_LEAVE;
                        Console.WriteLine(packet.Data);
                    }
                }
            }
            catch(Exception e)
            {
                // TODO : add logger
                Console.WriteLine(e);
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
