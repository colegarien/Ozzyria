using Ozzyria.Networking.Model;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ozzyria.Server
{
    class Program
    {
        const float ACCELERATION = 100f;
        const float MAX_SPEED = 100f;
        const float TURN_SPEED = 5f;
        const float SECONDS_PER_TICK = 0.016f;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //var server = new Networking.Server();
            //await server.StartListening();
            Console.WriteLine("start start");
            UdpClient server = new UdpClient(13000);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);


            var playerState = new PlayerState();

            Stopwatch stopWatch = new Stopwatch();

            var isRunning = true;
            while (isRunning)
            {
                stopWatch.Restart();

                var input = PlayerInput.Deserialize(server.Receive(ref remoteIPEndPoint));

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
                if(input.Quit)
                {
                    isRunning = false;
                }

                ///
                /// UPDATE LOGIC HERE
                ///
                playerState.X += playerState.Speed * SECONDS_PER_TICK * (float)Math.Sin(playerState.Direction);
                playerState.Y += playerState.Speed * SECONDS_PER_TICK * (float)Math.Cos(playerState.Direction);

                var serializedPlayerState = playerState.Serialize();
                server.Send(serializedPlayerState, serializedPlayerState.Length, remoteIPEndPoint);

                Thread.Sleep((int)Math.Max(((SECONDS_PER_TICK * 1000) - stopWatch.ElapsedMilliseconds), 0));
            }

            Console.WriteLine("bye bye");
        }
    }
}
