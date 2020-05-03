﻿using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Utility;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ozzyria.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Networking.Client();
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) => {
                client.Disconnect();
                ((Window)sender).Close();
            };

            var cameraX = 0f;
            var cameraY = 0f;

            if (!client.Connect("127.0.0.1", 13000))
            {
                Console.WriteLine("Join Failed");
                window.Close();
                return;
            }
            Console.WriteLine($"Join as Client #{client.Id}");

            var entityShapes = new Dictionary<int, EntityShape>();
            
            Stopwatch stopWatch = new Stopwatch();
            var deltaTime = 0f;
            while (window.IsOpen && client.IsConnected())
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

                var entities = client.Entities;
                var orbShapes = new List<ProjectileShape>();
                foreach (var entity in entities)
                {
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                    if (entity.HasComponent(ComponentType.ExperienceBoost))
                    {
                        // Orb stuff
                        orbShapes.Add(new ProjectileShape(movement.X, movement.Y));
                    }
                    else if (entity.HasComponent(ComponentType.Stats) && entity.HasComponent(ComponentType.Combat))
                    {
                        var stats = entity.GetComponent<Stats>(ComponentType.Stats);
                        var combat = entity.GetComponent<Combat>(ComponentType.Combat);

                        if (!entityShapes.ContainsKey(entity.Id))
                        {
                            entityShapes[entity.Id] = new EntityShape();
                        }

                        entityShapes[entity.Id].Visible = true;
                        entityShapes[entity.Id].ShowHealth = entity.Id != client.Id; // don't show health bar for local player
                        entityShapes[entity.Id].Color = entity.Id < 1000 
                            ? (entity.Id != client.Id ? Color.Cyan : Color.Blue) // is a player
                            : Color.Green; // is not a player
                        entityShapes[entity.Id].SetHealth(stats.Health, stats.MaxHealth);
                        // show as attacking for a brief period
                        entityShapes[entity.Id].LastAttack = combat.Delay.Timer / combat.Delay.DelayInSeconds;
                        entityShapes[entity.Id].Update(movement.X, movement.Y, movement.LookDirection);

                        // center camera on entity
                        /*if(entity.Id == client.Id)
                        {
                            cameraX = player.X - (window.Size.X / 2f);
                            cameraY = player.Y - (window.Size.Y / 2f);
                        }*/
                    }else if (entity.HasComponent(ComponentType.Collision))
                    {
                        var collision = entity.GetComponent<Collision>(ComponentType.Collision);

                        Shape shape;
                        if (collision is BoundingCircle)
                        {
                            var radius = ((BoundingCircle)collision).Radius;
                            shape = new CircleShape(radius);
                            shape.Position = new SFML.System.Vector2f(movement.X - radius, movement.Y - radius);
                        }
                        else
                        {
                            var width = ((BoundingBox)collision).Width;
                            var height = ((BoundingBox)collision).Height;
                            shape = new RectangleShape(new SFML.System.Vector2f(width, height));
                            shape.Position = new SFML.System.Vector2f(movement.X - (width/2f), movement.Y - (height/2f));
                        }
                        shape.FillColor = Color.Magenta;

                        entityShapes[entity.Id] = new SimpleShape(shape)
                        {
                            Visible = true
                        };
                    }
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();
                foreach(var shape in orbShapes)
                {
                    shape.Draw(window, cameraX, cameraY);
                }
                foreach (var entityShape in entityShapes.Values.Reverse())
                {
                    entityShape.Draw(window, cameraX, cameraY);
                    entityShape.Visible = false;
                }

                for (var i = 0; i<10;  i++) {
                    var player = entities.Where(e => e.Id == client.Id).FirstOrDefault();
                    var stats = player?.GetComponent<Stats>(ComponentType.Stats);
                    var fillHpBar = i < Math.Round((float)(stats?.Health ?? 0f) / (float)(stats?.MaxHealth ?? 1f) * 10);
                    window.Draw(new RectangleShape()
                    {
                        Position = new SFML.System.Vector2f(22 * i, 578),
                        Size = new SFML.System.Vector2f(20, 10),
                        FillColor = fillHpBar ? Color.Green : Color.Magenta
                    });
                    var fillExpBar = i < Math.Round((float)(stats?.Experience ?? 0f) / (float)(stats?.MaxExperience ?? 1f) * 10);
                    window.Draw(new RectangleShape()
                    {
                        Position = new SFML.System.Vector2f(22*i, 590),
                        Size = new SFML.System.Vector2f(20, 10),
                        FillColor = fillExpBar ? Color.Yellow : Color.Magenta
                    });
                }
                window.Display();

                if (quit || !client.IsConnected())
                {
                    client.Disconnect();
                    window.Close();
                }
            }
        }

        class SimpleShape : EntityShape
        {
            private Shape shape;
            public SimpleShape(Shape shape)
            {
                this.shape = shape;
            }

            public override void Draw(RenderWindow window, float cameraX, float cameraY)
            {
                shape.Position = new SFML.System.Vector2f(shape.Position.X - cameraX, shape.Position.Y - cameraY);
                window.Draw(shape);
                shape.Position = new SFML.System.Vector2f(shape.Position.X + cameraX, shape.Position.Y + cameraY);
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

            public void Update(float x, float y, float angle)
            {
                // Draw body centered on x and y
                MoveBody(x - body.Radius, y - body.Radius);
                nose.Rotation = AngleHelper.RadiansToDegrees(angle);
            }

            public virtual void Draw(RenderWindow window, float cameraX, float cameraY)
            {
                if (!Visible)
                    return;

                body.FillColor = Color;
                nose.FillColor = Color.Red;
                if (LastAttack < 0.25f)
                {
                    // only show red for 25% of the time between attacks
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