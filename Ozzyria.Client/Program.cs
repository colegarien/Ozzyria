using Ozzyria.Networking.Model;
using SFML.Graphics;
using SFML.Window;
using System;

namespace Ozzyria.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) => { ((Window)sender).Close(); };

            var client = new Networking.Client("127.0.0.1", 13000);
            if (!client.Connect())
            {
                Console.WriteLine("Join Failed");
                window.Close();
                return;
            }
            Console.WriteLine($"Join as Client #{client.Id}");

            var playerShapes = new PlayerShape[2]{
                new PlayerShape(client.Id == 0),
                new PlayerShape(client.Id == 1)
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

                ///
                /// Do Updates
                ///
                client.SendInput(input);
                var states = client.GetStates();
                foreach (var state in states)
                {
                    playerShapes[state.Id].Update(state.X, state.Y, state.Direction);
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
                    client.Disconnect();
                    window.Close();
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
                nose.Position = new SFML.System.Vector2f(body.Position.X + 10, body.Position.Y + 10);
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
