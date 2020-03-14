using Ozzyria.Networking.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Ozzyria.Server
{
    class Program
    {
        const float ACCELERATION = 100f;
        const float MAX_SPEED = 100f;
        const float TURN_SPEED = 5f;
        const float SECONDS_PER_TICK = 0.016f;
        const int TIMEOUT_MINUTES = 5;
        const int MAX_CLIENTS = 2;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("start start");
            var clients = new IPEndPoint[MAX_CLIENTS];
            var clientState = new PlayerState[MAX_CLIENTS];
            var clientInput = new PlayerInput[MAX_CLIENTS];
            var clientLastHeardFrom = new DateTime[MAX_CLIENTS];

            UdpClient server = new UdpClient(13000);

            Stopwatch stopWatch = new Stopwatch();

            var isRunning = true;
            while (isRunning)
            {
                stopWatch.Restart();

                while (server.Available > 0)
                {
                    var remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var packet = Encoding.ASCII.GetString(server.Receive(ref remoteIPEndPoint));
                    var messageType = Enum.Parse<ClientMessage>(packet.Substring(0, packet.IndexOf('>')));
                    var messageClient = int.Parse(packet.Substring(packet.IndexOf('>') + 1, packet.IndexOf('#') - (packet.IndexOf('>') + 1)));
                    var messageData = packet.Substring(packet.IndexOf('#') + 1);

                    switch (messageType)
                    {
                        case ClientMessage.Join:
                            var clientId = -1;
                            for (var i = 0; i < MAX_CLIENTS; i++)
                            {
                                if (clients[i] == null)
                                {
                                    clientId = i;
                                    clients[i] = remoteIPEndPoint;
                                    clientState[i] = new PlayerState() { Id = i };
                                    clientInput[i] = new PlayerInput();
                                    clientLastHeardFrom[i] = DateTime.Now;
                                    Console.WriteLine($"Client #{clientId} Joined");
                                    break;
                                }
                            }

                            var joinResult = Encoding.ASCII.GetBytes($"{(int)ServerMessage.JoinResult}>{clientId}");
                            if (clientId == -1)
                                joinResult = Encoding.ASCII.GetBytes($"{(int)ServerMessage.JoinReject}>");
                            server.Send(joinResult, joinResult.Length, remoteIPEndPoint);

                            break;
                        case ClientMessage.Leave:
                            if (true || clients[messageClient] == remoteIPEndPoint)
                            {
                                Console.WriteLine($"Client #{messageClient} Left");
                                clients[messageClient] = null;
                                clientState[messageClient] = null;
                                clientInput[messageClient] = null;
                            }
                            break;
                        case ClientMessage.InputUpdate:
                            if (true || clients[messageClient] == remoteIPEndPoint)
                            {
                                clientInput[messageClient] = PlayerInput.Deserialize(Encoding.ASCII.GetBytes(messageData));
                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                    }
                }

                // Perform Updates
                for (var i = 0; i < MAX_CLIENTS; i++)
                {
                    if (clients[i] == null)
                    {
                        continue;
                    }
                    else if (clientLastHeardFrom[i].AddMinutes(TIMEOUT_MINUTES) < DateTime.Now)
                    {
                        // Haven't heard from client in a while
                        clients[i] = null;
                        clientState[i] = null;
                        clientInput[i] = null;
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

                ///
                /// Build State Packet
                /// 
                var serializedPlayerState = Encoding.ASCII.GetBytes($"{(int)ServerMessage.StateUpdate}>").ToList();
                for (var i = 0; i < MAX_CLIENTS; i++)
                {
                    if (clients[i] == null)
                    {
                        continue;
                    }

                    serializedPlayerState = serializedPlayerState.Concat(clientState[i].Serialize()).Concat(Encoding.ASCII.GetBytes("@")).ToList();
                }


                ///
                /// Send State Packet
                /// 
                for (var i = 0; i < MAX_CLIENTS; i++)
                {
                    if (clients[i] == null)
                    {
                        continue;
                    }

                    server.Send(serializedPlayerState.ToArray(), serializedPlayerState.Count, clients[i]);
                }

                Thread.Sleep((int)Math.Max((SECONDS_PER_TICK * 1000) - stopWatch.ElapsedMilliseconds, 1));
            }

            server.Close();
            Console.WriteLine("bye bye");
        }
    }
}
