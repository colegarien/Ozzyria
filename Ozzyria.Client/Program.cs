using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Utility;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ozzyria.Client
{
    class Program
    {
        const bool DEBUG_SHOW_COLLISIONS = true;

        static void Main(string[] args)
        {
            var entityTexture = new Texture("Resources/Sprites/entity_set_001.png");
            var tileSetTexture = new Texture("Resources/Sprites/outside_tileset_001.png");

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
                var collisionShapes = new List<DebugCollisionShape>();
                var sprites = new List<Sprite>();
                var hoverStatBars = new List<HoverStatBar>();
                var uiStatBars = new List<UIProgressBar>();
                foreach (var entity in entities)
                {
                    var movement = entity.GetComponent<Movement>(ComponentType.Movement);
                    if (entity.HasComponent(ComponentType.Renderable))
                    {
                        var sprite = entity.GetComponent<Renderable>(ComponentType.Renderable).Sprite;
                        var sfmlSprite = new Sprite(entityTexture);
                        switch (sprite)
                        {
                            case SpriteType.Particle:
                                sfmlSprite.TextureRect = new IntRect(0, 96, 32, 32);
                                sfmlSprite.Color = Color.Yellow;
                                break;
                            case SpriteType.Player:
                                sfmlSprite.TextureRect = new IntRect(0, 32, 32, 32);
                                break;
                            case SpriteType.Slime:
                            default:
                                sfmlSprite.TextureRect = new IntRect(0, 0, 32, 32);
                                break;
                        }
                        sfmlSprite.Origin = new Vector2f(16, 16);
                        sfmlSprite.Position = new Vector2f(movement.X, movement.Y);
                        sfmlSprite.Rotation = AngleHelper.RadiansToDegrees(movement.LookDirection);

                        if (entity.HasComponent(ComponentType.Stats))
                        {
                            var stats = entity.GetComponent<Stats>(ComponentType.Stats);

                            // only show health bar for entities that are not the local player
                            if (entity.Id != client.Id)
                            {
                                var hoverStatBar = new HoverStatBar();
                                hoverStatBar.Move(movement.X, movement.Y);
                                hoverStatBar.SetMagnitude(stats.Health, stats.MaxHealth);
                                hoverStatBars.Add(hoverStatBar);
                            }
                            else
                            {
                                var healthBar = new UIProgressBar(0, 578, Color.Magenta, Color.Green);
                                healthBar.SetMagnitude(stats.Health, stats.MaxHealth);

                                var expBar = new UIProgressBar(0, 590, Color.Magenta, Color.Yellow);
                                expBar.SetMagnitude(stats.Experience, stats.MaxExperience);

                                uiStatBars.Add(healthBar);
                                uiStatBars.Add(expBar);
                            }
                        }

                        if (entity.HasComponent(ComponentType.Combat))
                        {
                            // show as attacking for a brief period
                            var combat = entity.GetComponent<Combat>(ComponentType.Combat);
                            sfmlSprite.Color = (combat.Delay.Timer / combat.Delay.DelayInSeconds >= 0.3f) ? Color.White : Color.Red;
                        }
                        sprites.Add(sfmlSprite);

                        // center camera on entity
                        if(entity.Id == client.Id)
                        {
                            cameraX = movement.X - (window.Size.X / 2f);
                            cameraY = movement.Y - (window.Size.Y / 2f);
                        }
                    }
                    
                    if (DEBUG_SHOW_COLLISIONS && entity.HasComponent(ComponentType.Collision))
                    {
                        var collision = entity.GetComponent<Collision>(ComponentType.Collision);

                        Shape shape;
                        if (collision is BoundingCircle)
                        {
                            var radius = ((BoundingCircle)collision).Radius;
                            shape = new CircleShape(radius);
                            shape.Position = new Vector2f(movement.X - radius, movement.Y - radius);
                        }
                        else
                        {
                            var width = ((BoundingBox)collision).Width;
                            var height = ((BoundingBox)collision).Height;
                            shape = new RectangleShape(new Vector2f(width, height));
                            shape.Position = new Vector2f(movement.X - (width/2f), movement.Y - (height/2f));
                        }
                        collisionShapes.Add(new DebugCollisionShape(shape));
                    }
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();
                foreach(var sprite in sprites)
                {
                    sprite.Position = new Vector2f(sprite.Position.X - cameraX, sprite.Position.Y - cameraY);
                    window.Draw(sprite);
                    sprite.Position = new Vector2f(sprite.Position.X + cameraX, sprite.Position.Y + cameraY);
                }

                foreach (var collisionShape in collisionShapes)
                {
                    collisionShape.Draw(window, cameraX, cameraY);
                }

                foreach (var hoverStatBar in hoverStatBars)
                {
                    hoverStatBar.Draw(window, cameraX, cameraY);
                }

                foreach (var uiStatBar in uiStatBars)
                {
                    uiStatBar.Draw(window);
                }
                window.Display();

                if (quit || !client.IsConnected())
                {
                    client.Disconnect();
                    window.Close();
                }
            }
        }

        class DebugCollisionShape
        {
            private Shape shape;
            public DebugCollisionShape(Shape shape)
            {
                this.shape = shape;
                shape.FillColor = Color.Transparent;
                shape.OutlineColor = Color.Magenta;
                shape.OutlineThickness = 1;
            }

            public void Draw(RenderWindow window, float cameraX, float cameraY)
            {
                shape.Position = new Vector2f(shape.Position.X - cameraX, shape.Position.Y - cameraY);
                window.Draw(shape);
                shape.Position = new Vector2f(shape.Position.X + cameraX, shape.Position.Y + cameraY);
            }
        }

        class UIProgressBar
        {
            private const int NUM_SEGMENTS = 10;
            private RectangleShape[] segments = new RectangleShape[NUM_SEGMENTS];
            private Color background;
            private Color foreground;

            public UIProgressBar(float X, float Y, Color backgroundColor, Color foregroundColor)
            {
                background = backgroundColor;
                foreground = foregroundColor;
                for(var segment = 0; segment < NUM_SEGMENTS; segment++)
                {
                    segments[segment] = new RectangleShape()
                    {
                        Position = new Vector2f(X + (22 * segment), Y),
                        Size = new Vector2f(20, 10),
                    };
                }
            }

            public void SetMagnitude(int current, int max)
            {
                var fillToSegment = Math.Round((float)(current) / (float)(max) * NUM_SEGMENTS);
                for (var segment = 0; segment < NUM_SEGMENTS; segment++)
                {
                    var fillSegment = segment < fillToSegment;
                    segments[segment].FillColor = fillSegment ? foreground : background;
                }
            }

            public void Draw(RenderWindow window)
            {
                foreach(var segment in segments)
                {
                    window.Draw(segment);
                }
            }
        }

        class HoverStatBar
        {
            private RectangleShape background;
            private RectangleShape overlay;

            public HoverStatBar()
            {
                var offset = new Vector2f(0, 14);

                background = new RectangleShape(new Vector2f(26, 5));
                background.Origin = new Vector2f(background.Size.X / 2 + offset.X, background.Size.Y + offset.Y);
                background.FillColor = Color.Red;

                overlay = new RectangleShape(background.Size);
                overlay.Origin = background.Origin;
                overlay.FillColor = Color.Green;
            }

            public void Move(float X, float Y)
            {
                background.Position = new Vector2f(X, Y);
                overlay.Position = new Vector2f(X, Y);
            }

            public void SetMagnitude(int current, int max)
            {
                overlay.Size = new Vector2f(((float)current / (float)max) * background.Size.X, overlay.Size.Y);
            }

            public virtual void Draw(RenderWindow window, float cameraX, float cameraY)
            {
                background.Position = new Vector2f(background.Position.X - cameraX, background.Position.Y - cameraY);
                overlay.Position = new Vector2f(overlay.Position.X - cameraX, overlay.Position.Y - cameraY);

                window.Draw(background);
                window.Draw(overlay);

                background.Position = new Vector2f(background.Position.X + cameraX, background.Position.Y + cameraY);
                overlay.Position = new Vector2f(overlay.Position.X + cameraX, overlay.Position.Y + cameraY);
            }
        }
    }
}
