using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Utility;
using Ozzyria.Game.Persistence;
using SFML.Graphics;
using SFML.System;
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
            var graphicsManger = GraphicsManager.GetInstance();
            var worldLoader = new WorldPersistence();
            var tileMap = worldLoader.LoadMap("test_m"); // TODO server should be telling user what map
            // TODO think about how multiple side-by-side tile mpas might work
            var worldRenderTexture = new RenderTexture((uint)(tileMap.Width * Tile.DIMENSION), (uint)(tileMap.Height * Tile.DIMENSION));


            var client = new Networking.Client();
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            var camera = new Camera(window.Size.X, window.Size.Y);
            var renderSystem = new RenderSystem();

            window.Closed += (sender, e) =>
            {
                client.Disconnect();
                ((Window)sender).Close();
            };
            window.Resized += (sender, e) =>
            {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                camera.ResizeView(e.Width, e.Height);
            };

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
                        var sfmlSprite = graphicsManger.CreateSprite(sprite);
                        sfmlSprite.Position = graphicsManger.CreateSpritePosition(movement.X, movement.Y);
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
                        if (entity.Id == client.Id)
                        {
                            camera.CenterView(movement.X, movement.Y);
                        }
                    }

                    if (RenderSystem.DEBUG_SHOW_COLLISIONS && entity.HasComponent(ComponentType.Collision))
                    {
                        var collision = entity.GetComponent<Collision>(ComponentType.Collision);

                        Shape shape;
                        if (collision is BoundingCircle)
                        {
                            var radius = ((BoundingCircle)collision).Radius;
                            shape = new CircleShape(radius);
                            shape.Position = graphicsManger.CreateSpritePosition(movement.X - radius, movement.Y - radius);
                        }
                        else
                        {
                            var width = ((BoundingBox)collision).Width;
                            var height = ((BoundingBox)collision).Height;
                            shape = new RectangleShape(new Vector2f(width, height));
                            shape.Position = graphicsManger.CreateSpritePosition(movement.X - (width / 2f), movement.Y - (height / 2f));
                        }
                        collisionShapes.Add(new DebugCollisionShape(shape));
                    }
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();

                worldRenderTexture.Clear();
                renderSystem.Render(worldRenderTexture, camera, tileMap, sprites.ToArray(), hoverStatBars.ToArray(), collisionShapes.ToArray());
                worldRenderTexture.Display();
                window.Draw(new Sprite(worldRenderTexture.Texture)
                {
                    Position = camera.GetTranslationVector()
                });


                ///
                /// Render UI Overlay
                ///
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
    }
}
