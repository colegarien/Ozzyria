using Ozzyria.Networking.Model;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Ozzyria.Networking
{
    public class Server
    {
        const int SERVER_PORT = 13000;

        const float ACCELERATION = 100f;
        const float MAX_SPEED = 100f;
        const float TURN_SPEED = 5f;
        const float SECONDS_PER_TICK = 0.016f;
        const int TIMEOUT_MINUTES = 5;
        const int MAX_CLIENTS = 2;

        private readonly IPEndPoint[] clients;
        private readonly PlayerState[] clientState;
        private readonly PlayerInput[] clientInput;
        private readonly DateTime[] clientLastHeardFrom;
        private readonly UdpClient server;

        public Server()
        {
            clients = new IPEndPoint[MAX_CLIENTS];
            clientState = new PlayerState[MAX_CLIENTS];
            clientInput = new PlayerInput[MAX_CLIENTS];
            clientLastHeardFrom = new DateTime[MAX_CLIENTS];
            
            server = new UdpClient(SERVER_PORT);
        }

        public void Start()
        {
            try
            {
                Console.WriteLine($"Serer Started - Listening on port {SERVER_PORT}");
                Stopwatch stopWatch = new Stopwatch();
                var isRunning = true;
                while (isRunning)
                {
                    stopWatch.Restart();

                    GatherInput();
                    Update();
                    SendState();

                    Thread.Sleep((int)Math.Max((SECONDS_PER_TICK * 1000) - stopWatch.ElapsedMilliseconds, 1));
                }
            }
            finally {
                server.Close();
                Console.WriteLine("Server Stopped");
            }
        }

        private void GatherInput()
        {
            while (server.Available > 0)
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
                            Console.WriteLine($"Client #{messageClient} Left");
                            clients[messageClient] = null;
                            clientState[messageClient] = null;
                            clientInput[messageClient] = null;
                        }
                        break;
                    case ClientMessage.InputUpdate:
                        if (IsValidEndPoint(messageClient, clientEndPoint))
                        {
                            clientInput[messageClient] = PlayerInput.Deserialize(messageData);
                            clientLastHeardFrom[messageClient] = DateTime.Now;
                        }
                        break;
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
                    clientState[i] = new PlayerState() { Id = i };
                    clientInput[i] = new PlayerInput();
                    clientLastHeardFrom[i] = DateTime.Now;
                    Console.WriteLine($"Client #{i} Joined");
                    break;
                }
            }

            return clientId;
        }

        private void Update()
        {
            for (var i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    continue;
                }

                var input = clientInput[i];
                var playerState = clientState[i];

                if (input.Left)
                {
                    playerState.Direction += TURN_SPEED * SECONDS_PER_TICK;
                }
                if (input.Right)
                {
                    playerState.Direction -= TURN_SPEED * SECONDS_PER_TICK;
                }
                if (input.Up)
                {
                    playerState.Speed += ACCELERATION * SECONDS_PER_TICK;
                    if (playerState.Speed > MAX_SPEED)
                    {
                        playerState.Speed = MAX_SPEED;
                    }
                }
                if (input.Down)
                {
                    playerState.Speed -= ACCELERATION * SECONDS_PER_TICK;
                    if (playerState.Speed < 0.0f)
                    {
                        playerState.Speed = 0.0f;
                    }
                }

                ///
                /// UPDATE LOGIC HERE
                ///
                playerState.X += playerState.Speed * SECONDS_PER_TICK * (float)Math.Sin(playerState.Direction);
                playerState.Y += playerState.Speed * SECONDS_PER_TICK * (float)Math.Cos(playerState.Direction);

                clientState[i] = playerState;
            }
        }

        private void SendState()
        {
            var statePacket = ServerPacketFactory.PlayerState(clientState);
            for (var i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    continue;
                }

                server.Send(statePacket, statePacket.Length, clients[i]);
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
                clientState[clientId] = null;
                clientInput[clientId] = null;

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
