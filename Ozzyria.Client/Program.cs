using Ozzyria.Game;
using Ozzyria.Game.Component;
using Ozzyria.Game.Persistence;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Diagnostics;

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

                ///
                /// DRAWING HERE
                ///
                window.Clear();

                worldRenderTexture.Clear();
                renderSystem.Render(worldRenderTexture, camera, tileMap, client.Id, entities);
                worldRenderTexture.Display();
                window.Draw(new Sprite(worldRenderTexture.Texture)
                {
                    Position = camera.GetTranslationVector()
                });


                ///
                /// Render UI Overlay
                ///
                // TODO OZ-13 : bring back UI overlay
                /*foreach (var uiStatBar in uiStatBars)
                {
                    uiStatBar.Draw(window);
                }*/
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
