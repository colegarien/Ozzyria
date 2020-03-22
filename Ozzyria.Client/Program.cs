using Ozzyria.Game;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var playerShapes = new Dictionary<int, PlayerShape>();
            playerShapes[client.Id] = new PlayerShape();

            while (window.IsOpen)
            {
                ///
                /// EVENT HANDLING HERE
                ///
                window.DispatchEvents();
                var quit = Keyboard.IsKeyPressed(Keyboard.Key.Escape);
                var input = new Input
                {
                    MoveUp = Keyboard.IsKeyPressed(Keyboard.Key.W),
                    MoveDown = Keyboard.IsKeyPressed(Keyboard.Key.S),
                    MoveLeft = Keyboard.IsKeyPressed(Keyboard.Key.A),
                    MoveRight = Keyboard.IsKeyPressed(Keyboard.Key.D),
                    TurnLeft = Keyboard.IsKeyPressed(Keyboard.Key.Q),
                    TurnRight = Keyboard.IsKeyPressed(Keyboard.Key.E)
                };

                ///
                /// Do Updates
                ///
                client.SendInput(input);
                var players = client.GetPlayers();
                foreach (var player in players)
                {
                    if (!playerShapes.ContainsKey(player.Id))
                    {
                        playerShapes[player.Id] = new PlayerShape();
                    }
                    playerShapes[player.Id].Visible = true;
                    playerShapes[player.Id].IsLocalPlayer = player.Id == client.Id;
                    playerShapes[player.Id].Update(player.X, player.Y, player.LookDirection);
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();
                foreach (var playerShape in playerShapes.Values.Reverse())
                {
                    playerShape.Draw(window);
                    playerShape.Visible = false;
                }
                window.Display();

                if (quit)
                {
                    client.Disconnect();
                    window.Close();
                }
            }
        }

        class PlayerShape
        {
            public bool Visible { get; set; }
            public bool IsLocalPlayer { get; set; }

            private CircleShape body;
            private RectangleShape nose;

            public PlayerShape()
            {
                body = new CircleShape(10f);

                nose = new RectangleShape(new SFML.System.Vector2f(2, 15));
                nose.Position = new SFML.System.Vector2f(body.Position.X + 10, body.Position.Y + 10);
                nose.Origin = new SFML.System.Vector2f(1, 0);
            }

            public void Update(float x, float y, float angle)
            {
                body.Position = new SFML.System.Vector2f(x, y);
                nose.Position = new SFML.System.Vector2f(x + 10, y + 10);
                nose.Rotation = -(180f / (float)Math.PI) * angle;
            }

            public void Draw(RenderWindow window)
            {
                if (!Visible)
                    return;

                if (IsLocalPlayer)
                {
                    body.FillColor = Color.Green;
                    nose.FillColor = Color.Red;
                }
                else
                {
                    body.FillColor = Color.Cyan;
                    nose.FillColor = Color.Magenta;
                }

                window.Draw(body);
                window.Draw(nose);
            }
        }
    }
}
