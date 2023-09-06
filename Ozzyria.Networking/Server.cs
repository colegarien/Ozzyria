using Ozzyria.Game;
using Ozzyria.Networking.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ozzyria.Networking
{
    public class Server
    {
        const int SERVER_PORT = 13000;

        const float SECONDS_PER_TICK = 0.016f;
        const int TIMEOUT_MINUTES = 2;
        const int MAX_CLIENTS = 8;

        private readonly IPEndPoint[] clients;
        private readonly DateTime[] clientLastHeardFrom;

        private readonly UdpClient server;
        private readonly World world;

        public Server()
        {
            clients = new IPEndPoint[MAX_CLIENTS];
            clientLastHeardFrom = new DateTime[MAX_CLIENTS];

            // TODO OZ-28 add abstract so World is configure in Ozzyria.Server and leave Networking package just for networking
            world = new World();
            server = new UdpClient(SERVER_PORT);
        }

        public void Start(object obj = null)
        {
            CancellationToken ct;
            if (obj != null)
                ct = (CancellationToken)obj;
            else
                ct = new CancellationToken();

            try
            {
                Console.WriteLine($"Server Started - Listening on port {SERVER_PORT}");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Restart();
                var isRunning = true;
                while (isRunning && !ct.IsCancellationRequested)
                {
                    // TODO OZ-28 chunk entity updates sent back
                    // TODO OZ-28 move sending/reading entity updates into separate tasks on the client/server
                    HandleMessages();

                    if (stopWatch.ElapsedMilliseconds >= SECONDS_PER_TICK * 1000)
                    {
                        // update and re-send state every SECONDS_PER_TICK
                        world.Update(SECONDS_PER_TICK);
                        SendLocalState();
                        SendGlobalState();

                        stopWatch.Restart();
                    }
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
                                world.PlayerLeave(messageClient);
                                Console.WriteLine($"Client #{messageClient} Left");
                            }
                            break;
                        case ClientMessage.InputUpdate:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                var input = ClientPacketFactory.ParseInputData(messageData);
                                world.WorldState.PlayerInputBuffer[messageClient].MoveUp = input.MoveUp;
                                world.WorldState.PlayerInputBuffer[messageClient].MoveDown = input.MoveDown;
                                world.WorldState.PlayerInputBuffer[messageClient].MoveLeft = input.MoveLeft;
                                world.WorldState.PlayerInputBuffer[messageClient].MoveRight = input.MoveRight;
                                world.WorldState.PlayerInputBuffer[messageClient].TurnLeft = input.TurnLeft;
                                world.WorldState.PlayerInputBuffer[messageClient].TurnRight = input.TurnRight;
                                world.WorldState.PlayerInputBuffer[messageClient].Attack = input.Attack;

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
            int clientId = 0;
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    clientId = i;
                    clients[i] = clientEndPoint;
                    clientLastHeardFrom[i] = DateTime.Now;
                    world.PlayerJoin(i);
                    Console.WriteLine($"Client #{i} Joined");
                    break;
                }
            }

            return clientId;
        }


        private void SendLocalState()
        {
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    continue;
                }

                var localContext = world.GetLocalContext(i);
                if(localContext == null)
                {
                    continue;
                }

                // TODO OZ-28 add better mechanism for broadcasting packates in local areas to players in those areas
                // TODO OZ-28 get rid of this event system of make it less coupled to the "World" and/or les boxing/unboxing of objects
                foreach(var areaEvent in world.WorldState.AreaEvents)
                {
                    if(areaEvent is EntityLeaveAreaEvent)
                    {
                        EntityLeaveAreaEvent alae = (EntityLeaveAreaEvent)areaEvent;
                        if (alae.PlayerId == i) {
                            // tell client the local player left the area they were in
                            var areaChangePacket = ServerPacketFactory.AreaChanged(i, alae.SourceArea, alae.NewArea);
                            SendToClient(i, areaChangePacket);
                        }
                    }
                }

                var entityPacket = ServerPacketFactory.EntityUpdates(localContext.GetEntities());
                SendToClient(i, entityPacket);

                var destroyPacket = ServerPacketFactory.EntityRemovals(localContext);
                SendToClient(i, destroyPacket);
            }

            world.WorldState.AreaEvents.Clear();
        }

        private void SendGlobalState()
        {
            // TODO Send Globally Broadcasted Packets
        }

        private void SendToAll(byte[] packet, int exclude = -1)
        {
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (exclude == i)
                {
                    continue;
                }

                SendToClient(i, packet);
            }
        }

        private void SendToClient(int clientId, byte[] packet)
        {
            if (!IsConnected(clientId))
            {
                return;
            }

            server.Send(packet, packet.Length, clients[clientId]);
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
                world.PlayerLeave(clientId);
                Console.WriteLine($"Client #{clientId} timed out");

                return false;
            }

            return true;
        }

        private bool IsValidEndPoint(int clientId, IPEndPoint endPoint)
        {
            return clients[clientId] != null && clients[clientId].Equals(endPoint);
        }

    }
}
