using Ozzyria.Game;
using Ozzyria.Game.Event;
using Ozzyria.Networking.Model;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ozzyria.Networking
{
    public class Server : IEventHandler
    {
        const int SERVER_PORT = 13000;

        const float SECONDS_PER_TICK = 0.016f;
        const int TIMEOUT_MINUTES = 2;
        const int MAX_CLIENTS = 8;

        private readonly IPEndPoint[] clients;
        private readonly DateTime[] clientLastHeardFrom;

        private readonly UdpClient server;
        private readonly Game.Game game;

        public Server()
        {
            clients = new IPEndPoint[MAX_CLIENTS];
            clientLastHeardFrom = new DateTime[MAX_CLIENTS];

            game = new Game.Game();
            game.AttachEventHandler(this);

            server = new UdpClient(SERVER_PORT);
        }

        public void Start()
        {
            try
            {
                Console.WriteLine($"Server Started - Listening on port {SERVER_PORT}");
                Stopwatch stopWatch = new Stopwatch();
                var isRunning = true;
                while (isRunning)
                {
                    stopWatch.Restart();

                    HandleMessages();
                    game.Update(SECONDS_PER_TICK);
                    SendState();

                    Thread.Sleep((int)Math.Max((SECONDS_PER_TICK * 1000) - stopWatch.ElapsedMilliseconds, 1));
                }
            }
            finally {
                server.Close();
                Console.WriteLine("Server Stopped");
            }
        }

        private void HandleMessages()
        {
            while (server.Available > 0)
            {
                try
                {
                    var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var packet = ClientPacketFactory.Parse(server.Receive(ref clientEndPoint));
                    var messageType = packet.Type;
                    var messageClient = packet.ClientId;
                    var messageData = packet.Data;
                

                    switch (messageType)
                    {
                        case ClientMessage.Join:
                            var clientId = PrepareClientSlot(clientEndPoint);
                            var joinPacket = ServerPacketFactory.Join(clientId);
                            server.Send(joinPacket, joinPacket.Length, clientEndPoint);
                            break;
                        case ClientMessage.Leave:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                clients[messageClient] = null;
                                game.OnPlayerLeave(messageClient);
                                Console.WriteLine($"Client #{messageClient} Left");
                            }
                            break;
                        case ClientMessage.InputUpdate:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                game.OnPlayerInput(messageClient, ClientPacketFactory.ParseInputData(messageData));
                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                    }
                }
                catch (SocketException)
                {
                    // Likely a client connection was reset (will get cleaned up by clientLastHeardFrom)
                }
            }
        }

        private int PrepareClientSlot(IPEndPoint clientEndPoint)
        {
            var clientId = -1;
            for (var i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    clientId = i;
                    clients[i] = clientEndPoint;
                    clientLastHeardFrom[i] = DateTime.Now;
                    game.OnPlayerJoin(clientId);
                    Console.WriteLine($"Client #{i} Joined");
                    break;
                }
            }

            return clientId;
        }

        private void SendState()
        {
            var entityPacket = ServerPacketFactory.EntityUpdates(game.entityManager.GetEntities());
            SendToAll(entityPacket);
        }

        private void SendToAll(byte[] packet, int exclude = -1)
        {
            for (var i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i) || exclude == i)
                {
                    continue;
                }

                server.Send(packet, packet.Length, clients[i]);
            }
        }

        private bool IsConnected(int clientId)
        {
            if (clients[clientId] == null)
            {
                return false;
            }
            else if (clientLastHeardFrom[clientId].AddMinutes(TIMEOUT_MINUTES) < DateTime.Now)
            {
                // Haven't heard from client in a while
                clients[clientId] = null;
                game.OnPlayerLeave(clientId);
                Console.WriteLine($"Client #{clientId} timed out");

                return false;
            }

            return true;
        }

        private bool IsValidEndPoint(int clientId, IPEndPoint endPoint)
        {
            return clients[clientId] != null && clients[clientId].Equals(endPoint);
        }

        public bool CanHandle(IEvent gameEvent)
        {
            // handle player deaths only
            return gameEvent is EntityDead && ((EntityDead)gameEvent).Id >= 0 && ((EntityDead)gameEvent).Id < MAX_CLIENTS;
        }

        public void Handle(IEvent gameEvent)
        {
            var playerId = ((EntityDead)gameEvent).Id;
            // just re-join game on death
            game.OnPlayerJoin(playerId);
        }
    }
}
