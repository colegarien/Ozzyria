using Ozzyria.Networking.Model;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ozzyria.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) => { ((Window)sender).Close(); };

            UdpClient udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Parse("127.0.0.1"), 13000);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            var joinPacket = Encoding.ASCII.GetBytes($"{(int)ClientMessage.Join}>-1#");
            udpClient.Send(joinPacket, joinPacket.Length);
            var packet = Encoding.ASCII.GetString(udpClient.Receive(ref remoteIPEndPoint));
            var messageType = Enum.Parse<ServerMessage>(packet.Substring(0, packet.IndexOf('>')));
            if(messageType != ServerMessage.JoinResult)
            {
                Console.WriteLine("Join Failed");
                window.Close();
                udpClient.Close();
                return;
            }

            var clientId = int.Parse(packet.Substring(packet.IndexOf('>') + 1));
            Console.WriteLine($"Joined as client #{clientId}");

            var playerShapes = new PlayerShape[2]{
                new PlayerShape(clientId == 0),
                new PlayerShape(clientId == 1)
            };

            while (window.IsOpen)
            {
                ///
                /// EVENT HANDLING HERE
                ///
                window.DispatchEvents();
                var input = new PlayerInput
                {
                    Up = Keyboard.IsKeyPressed(Keyboard.Key.W),
                    Down = Keyboard.IsKeyPressed(Keyboard.Key.S),
                    Left = Keyboard.IsKeyPressed(Keyboard.Key.A),
                    Right = Keyboard.IsKeyPressed(Keyboard.Key.D),
                    Quit = Keyboard.IsKeyPressed(Keyboard.Key.Escape),
                };

                var serializedInput = Encoding.ASCII.GetBytes($"{(int)ClientMessage.InputUpdate}>{clientId}#").Concat(input.Serialize()).ToArray();
                udpClient.Send(serializedInput, serializedInput.Length);

                packet = Encoding.ASCII.GetString(udpClient.Receive(ref remoteIPEndPoint));
                messageType = Enum.Parse<ServerMessage>(packet.Substring(0, packet.IndexOf('>')));
                if (messageType == ServerMessage.StateUpdate)
                {
                    var serializedStates = packet.Substring(packet.IndexOf('>') + 1).Split('@');
                    foreach (var serializedState in serializedStates)
                    {
                        if (serializedState.Trim().Length == 0)
                        {
                            continue;
                        }
                        var state = PlayerState.Deserialize(Encoding.ASCII.GetBytes(serializedState));

                        ///
                        /// UPDATE LOGIC HERE
                        ///
                        playerShapes[state.Id].Update(state.X, state.Y, state.Direction);
                    }
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();
                foreach (var playerShape in playerShapes)
                {
                    playerShape.Draw(window);
                }
                window.Display();

                if (input.Quit)
                {
                    var leavePacket = Encoding.ASCII.GetBytes($"{(int)ClientMessage.Leave}>{clientId}#");
                    udpClient.Send(leavePacket, leavePacket.Length);

                    window.Close();
                    udpClient.Close();
                }
            }
        }

        class PlayerShape
        {
            private CircleShape body;
            private RectangleShape nose;

            public PlayerShape(bool isLocalPlayer)
            {
                body = new CircleShape(10f);
                body.FillColor = isLocalPlayer ? Color.Green : Color.Cyan;
                nose = new RectangleShape(new SFML.System.Vector2f(2, 15));
                nose.Origin = new SFML.System.Vector2f(1, 0);
                nose.FillColor = isLocalPlayer ? Color.Red : Color.Magenta;
            }

            public void Update(float x, float y, float angle)
            {
                body.Position = new SFML.System.Vector2f(x, y);
                nose.Position = new SFML.System.Vector2f(x + 10, y + 10);
                nose.Rotation = -(180f / (float)Math.PI) * angle;
            }

            public void Draw(RenderWindow window)
            {
                window.Draw(body);
                window.Draw(nose);
            }
        }
    }
}
