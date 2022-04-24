﻿using Ozzyria.Client.Graphics.UI;
using Ozzyria.Game;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.Game.Persistence;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Linq;

namespace Ozzyria.Client
{
    class Program
    {

        static void Main(string[] args)
        {
            var context = new EntityContext();

            var graphicsManger = GraphicsManager.GetInstance();
            var worldLoader = new WorldPersistence();

            // Loaded / Reloaded as player changes maps
            TileMap tileMap = null;
            RenderTexture worldRenderTexture = null;

            var client = new Networking.Client();

            var videoMode = new VideoMode(1280, 720);
            //var videoMode = new VideoMode((uint)Math.Max(VideoMode.DesktopMode.Width, Camera.RENDER_RESOLUTION_W), (uint)Math.Max(VideoMode.DesktopMode.Height, Camera.RENDER_RESOLUTION_H));
            RenderWindow window = new RenderWindow(videoMode, "Ozzyria", Styles.None | Styles.Close);
            window.SetView(new View(new FloatRect(0, 0, Camera.RENDER_RESOLUTION_W, Camera.RENDER_RESOLUTION_H)));
            var camera = new Camera(Camera.RENDER_RESOLUTION_W, Camera.RENDER_RESOLUTION_H);

            var renderSystem = new RenderSystem();

            var healthBar = new OverlayProgressBar(0, Camera.RENDER_RESOLUTION_H - 22, Color.Magenta, Color.Green);
            var experienceBar = new OverlayProgressBar(0, Camera.RENDER_RESOLUTION_H - 10, Color.Magenta, Color.Yellow);

            Console.WriteLine($"Window Size {window.Size.X}x{window.Size.Y} - View Size {window.GetView().Size.X}x{window.GetView().Size.Y}");

            window.Closed += (sender, e) =>
            {
                client.Disconnect();
                ((Window)sender).Close();
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
                client.HandleIncomingMessages(context);

                var localPlayer = context.GetEntities(new EntityQuery().And(typeof(Player))).FirstOrDefault(e => ((Player)e.GetComponent(typeof(Player))).PlayerId == client.Id);
                var playerEntityId = localPlayer?.id ?? 0;
                var playerEntityMap = ((Player)localPlayer?.GetComponent(typeof(Player)))?.Map ?? "";

                if ((tileMap == null || playerEntityMap != tileMap?.Name) && playerEntityMap != "")
                {
                    tileMap = worldLoader.LoadMap(playerEntityMap);
                    worldRenderTexture = new RenderTexture((uint)(tileMap.Width * Tile.DIMENSION), (uint)(tileMap.Height * Tile.DIMENSION));
                }

                ///
                /// DRAWING HERE
                ///
                window.Clear();

                if (tileMap != null && worldRenderTexture != null)
                {
                    worldRenderTexture.Clear();
                    renderSystem.Render(worldRenderTexture, camera, tileMap, context, playerEntityId);
                    worldRenderTexture.Display();
                    window.Draw(new Sprite(worldRenderTexture.Texture)
                    {
                        Position = camera.GetTranslationVector()
                    });
                }


                ///
                /// Render UI Overlay
                ///
                var localPlayerStats = (Stats)localPlayer?.GetComponent(typeof(Stats));
                if (localPlayerStats != null)
                {
                    healthBar.SetMagnitude(localPlayerStats.Health, localPlayerStats.MaxHealth);
                    experienceBar.SetMagnitude(localPlayerStats.Experience, localPlayerStats.MaxExperience);

                    healthBar.Draw(window);
                    experienceBar.Draw(window);
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
