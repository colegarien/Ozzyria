using Ozzyria.Networking.Model;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Net;
using System.Net.Sockets;

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

            CircleShape shape = new CircleShape(10f);
            shape.FillColor = Color.Green;
            RectangleShape shape2 = new RectangleShape(new SFML.System.Vector2f(2, 15));
            shape2.Origin = new SFML.System.Vector2f(1, 0);
            shape2.FillColor = Color.Red;

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

                var serializedInput = input.Serialize();
                udpClient.Send(serializedInput, serializedInput.Length);

                var state = PlayerState.Deserialize(udpClient.Receive(ref remoteIPEndPoint));

                ///
                /// UPDATE LOGIC HERE
                ///
                shape.Position = new SFML.System.Vector2f(state.X, state.Y);
                shape2.Position = new SFML.System.Vector2f(state.X +10, state.Y +10);
                shape2.Rotation = -(180f / (float)Math.PI) * state.Direction;

                ///
                /// DRAWING HERE
                ///
                window.Clear();
                window.Draw(shape);
                window.Draw(shape2);
                window.Display();


                if (input.Quit)
                    window.Close();
            }
        }
    }
}
