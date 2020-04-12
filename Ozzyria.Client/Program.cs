using Ozzyria.Game;
using Ozzyria.Game.Utility;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Ozzyria.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) => { ((Window)sender).Close(); };

            var cameraX = 0f;
            var cameraY = 0f;

            var client = new Networking.Client();
            if (!client.Connect("127.0.0.1", 13000))
            {
                Console.WriteLine("Join Failed");
                window.Close();
                return;
            }
            Console.WriteLine($"Join as Client #{client.Id}");

            var playerShapes = new Dictionary<int, EntityShape>();
            playerShapes[client.Id] = new EntityShape();

            Stopwatch stopWatch = new Stopwatch();
            var deltaTime = 0f;
            while (window.IsOpen)
            {
                deltaTime = stopWatch.ElapsedMilliseconds / 1000f;
                stopWatch.Restart();

                ///
                /// EVENT HANDLING HERE
                ///
                window.DispatchEvents();
                var quit = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Escape);
                var input = new Input
                {
                    MoveUp = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.W),
                    MoveDown = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.S),
                    MoveLeft = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.A),
                    MoveRight = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.D),
                    TurnLeft = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Q),
                    TurnRight = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.E),
                    Attack = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Space)
                };

                ///
                /// Do Updates
                ///
                client.SendInput(input);
                client.HandleIncomingMessages();

                var players = client.Players;
                foreach (var player in players)
                {
                    if (!playerShapes.ContainsKey(player.Id))
                    {
                        playerShapes[player.Id] = new EntityShape();
                    }
                    playerShapes[player.Id].Visible = true;
                    playerShapes[player.Id].ShowHealth = player.Id != client.Id;
                    playerShapes[player.Id].Color = player.Id != client.Id ? Color.Cyan : Color.Blue;
                    playerShapes[player.Id].SetHealth(player.Health, player.MaxHealth);
                    if (player.Attacking)
                    {
                        // show player as attacking for a brief period
                        playerShapes[player.Id].LastAttack = player.AttackDelay / 3f;
                    }
                    playerShapes[player.Id].Update(deltaTime, player.Movement.X, player.Movement.Y, player.Movement.LookDirection);

                    // center camera on player
                    /*if(player.Id == client.Id)
                    {
                        cameraX = player.X - (window.Size.X / 2f);
                        cameraY = player.Y - (window.Size.Y / 2f);
                    }*/
                }

                var orbs = client.Orbs;
                var orbShapes = new List<ProjectileShape>();
                foreach(var orb in orbs)
                {
                    orbShapes.Add(new ProjectileShape(orb.Movement.X, orb.Movement.Y));
                }


                var slimes = client.Slimes;
                var slimeShapes = new List<EntityShape>();
                foreach (var slime in slimes)
                {
                    var shape = new EntityShape();
                    shape.Visible = true;
                    shape.ShowHealth = true;
                    shape.Color = Color.Green;
                    shape.SetHealth(slime.Health, slime.MaxHealth);
                    shape.Update(deltaTime, slime.Movement.X, slime.Movement.Y, slime.Movement.LookDirection);
                    if (slime.Attacking)
                    {
                        // show slime as attacking for a brief period
                        shape.LastAttack = slime.AttackDelay / 3f;
                    }
                    slimeShapes.Add(shape);
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();
                foreach(var shape in orbShapes)
                {
                    shape.Draw(window, cameraX, cameraY);
                }
                foreach (var shape in slimeShapes)
                {
                    shape.Draw(window, cameraX, cameraY);
                }
                foreach (var playerShape in playerShapes.Values.Reverse())
                {
                    playerShape.Draw(window, cameraX, cameraY);
                    playerShape.Visible = false;
                }

                for (var i = 0; i<10;  i++) {
                    var player = players.Where(p => p.Id == client.Id).FirstOrDefault();
                    var fillHpBar = i < Math.Round((float)(player?.Health ?? 0f) / (float)(player?.MaxHealth ?? 1f) * 10);
                    window.Draw(new RectangleShape()
                    {
                        Position = new SFML.System.Vector2f(22 * i, 578),
                        Size = new SFML.System.Vector2f(20, 10),
                        FillColor = fillHpBar ? Color.Green : Color.Magenta
                    });
                    var fillExpBar = i < Math.Round((float)(player?.Experience ?? 0f) / (float)(player?.MaxExperience ?? 1f) * 10);
                    window.Draw(new RectangleShape()
                    {
                        Position = new SFML.System.Vector2f(22*i, 590),
                        Size = new SFML.System.Vector2f(20, 10),
                        FillColor = fillExpBar ? Color.Yellow : Color.Magenta
                    });
                }
                window.Display();

                if (quit)
                {
                    client.Disconnect();
                    window.Close();
                }
            }
        }

        class ProjectileShape
        {
            private CircleShape body;

            public ProjectileShape(float x, float y)
            {
                body = new CircleShape(3f);
                body.FillColor = Color.Yellow;
                body.Position = new SFML.System.Vector2f(x - body.Radius, y - body.Radius);
            }

            public void Draw(RenderWindow window, float cameraX, float cameraY)
            {
                body.Position = new SFML.System.Vector2f(body.Position.X - cameraX, body.Position.Y - cameraY);
                window.Draw(body);
                body.Position = new SFML.System.Vector2f(body.Position.X + cameraX, body.Position.Y + cameraY);
            }

        }

        class EntityShape
        {
            public bool Visible { get; set; }
            public Color Color { get; set; }
            public bool ShowHealth { get; set; } = true;

            public float LastAttack { get; set; }

            private CircleShape body;
            private RectangleShape nose;

            private RectangleShape healthUnderBar;
            private RectangleShape healthOverBar;


            public EntityShape()
            {
                body = new CircleShape(10f);

                nose = new RectangleShape(new SFML.System.Vector2f(2, 15));
                nose.Origin = new SFML.System.Vector2f(1, 0);

                healthUnderBar = new RectangleShape(new SFML.System.Vector2f(26, 5));
                healthUnderBar.FillColor = Color.Red;

                healthOverBar = new RectangleShape(new SFML.System.Vector2f(15, 5));
                healthOverBar.FillColor = Color.Green;

                // initialize positioning
                MoveBody(0, 0);
            }

            public void Update(float deltaTime, float x, float y, float angle)
            {
                // Draw body centered on x and y
                MoveBody(x - body.Radius, y - body.Radius);
                nose.Rotation = AngleHelper.RadiansToDegrees(angle);

                if (LastAttack > 0) {
                    LastAttack -= deltaTime;
                }
            }

            public void Draw(RenderWindow window, float cameraX, float cameraY)
            {
                if (!Visible)
                    return;

                body.FillColor = Color;
                nose.FillColor = Color.Red;
                if (LastAttack > 0f)
                {
                    body.FillColor = Color.Red;
                }


                MoveBody(body.Position.X - cameraX, body.Position.Y - cameraY);
                window.Draw(body);
                window.Draw(nose);
                if (ShowHealth)
                {
                    window.Draw(healthUnderBar);
                    window.Draw(healthOverBar);
                }
                MoveBody(body.Position.X + cameraX, body.Position.Y + cameraY);
            }

            public void SetHealth(int health, int maxHealth)
            {
                healthOverBar.Size = new SFML.System.Vector2f(((float)health / (float)maxHealth) * healthUnderBar.Size.X, healthOverBar.Size.Y);
            }

            private void MoveBody(float x, float y)
            {
                body.Position = new SFML.System.Vector2f(x, y);
                nose.Position = new SFML.System.Vector2f(body.Position.X + body.Radius, body.Position.Y + body.Radius);
                healthUnderBar.Position = new SFML.System.Vector2f(body.Position.X-3, body.Position.Y - healthUnderBar.Size.Y - 5);
                healthOverBar.Position = new SFML.System.Vector2f(body.Position.X-3, body.Position.Y - healthOverBar.Size.Y - 5);
            }
        }
    }
}
