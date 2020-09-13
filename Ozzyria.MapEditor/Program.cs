using Ozzyria.Game;
using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ozzyria.MapEditor
{
    class Program
    {

        class MapViewerSettings
        {
            public int MaxLayers { get; set; } = 2; // TODO support more than 2 layers
            public int TileMapWidth { get; set; } = 24;
            public int TileMapHeight { get; set; } = 24;

            public float MapOffsetX { get; set; } = 0f;
            public float MapOffsetY { get; set; } = 0f;
            public float Zoom { get; set; } = 1f;

            public float TileSize { get => 32f * Zoom; }

            public float TileToScreenX(int TileX)
            {
                return MapOffsetX + (TileX * TileSize);
            }
            public float TileToScreenY(int TileY)
            {
                return MapOffsetY + (TileY * TileSize);
            }

            public int ScreenToTileX(float ScreenX)
            {
                return (int)Math.Floor((ScreenX - MapOffsetX) / TileSize);
            }
            public int ScreenToTileY(float ScreenY)
            {
                return (int)Math.Floor((ScreenY - MapOffsetY) / TileSize);
            }
        }

        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            ViewWindow viewWindow = new ViewWindow(40, 40, (uint)(window.Size.X - 80), (uint)(window.Size.Y - 80), window.Size.X, window.Size.Y);

            var font = new Font("Fonts\\Bitter-Regular.otf");

            window.Closed += (sender, e) =>
            {
                ((Window)sender).Close();
            };
            window.Resized += (sender, e) =>
            {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                viewWindow.ResizeWindow(40, 40, (uint)(window.Size.X - 80), (uint)(window.Size.Y - 80), window.Size.X, window.Size.Y);
            };

            var ctrlKeyDown = false;
            var altKeyDown = false;
            window.KeyPressed += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.LControl || e.Code == Keyboard.Key.RControl)
                {
                    ctrlKeyDown = true;
                }
                else if (e.Code == Keyboard.Key.LAlt || e.Code == Keyboard.Key.RAlt)
                {
                    altKeyDown = true;
                }
            };
            window.KeyReleased += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.LControl || e.Code == Keyboard.Key.RControl)
                {
                    ctrlKeyDown = false;
                }
                else if (e.Code == Keyboard.Key.LAlt || e.Code == Keyboard.Key.RAlt)
                {
                    altKeyDown = false;
                }
            };
            window.MouseWheelScrolled += (sender, e) =>
            {
                if (!viewWindow.IsInWindow(e.X, e.Y))
                {
                    return;
                }

                if(e.Wheel == Mouse.Wheel.HorizontalWheel || (altKeyDown && e.Wheel == Mouse.Wheel.VerticalWheel))
                {
                    viewWindow.OnHorizontalScroll(e.Delta);
                }
                else if (ctrlKeyDown)
                {
                    viewWindow.OnZoom(e.X, e.Y, e.Delta);
                }
                else {
                    viewWindow.OnVerticalScroll(e.Delta);
                }
            };

            var leftMouseDown = false;
            var middleMouseDown = false;
            var middleDownStartX = 0;
            var middleDownStartY = 0;
            window.MouseButtonPressed += (sender, e) =>
            {
                viewWindow.OnMouseMove(e.X, e.Y);
                if (e.Button == Mouse.Button.Middle)
                {
                    middleMouseDown = true;
                    middleDownStartX = e.X;
                    middleDownStartY = e.Y;
                }
                else if(e.Button == Mouse.Button.Left)
                {
                    leftMouseDown = true;
                }
            };
            window.MouseButtonReleased += (sender, e) =>
            {
                viewWindow.OnMouseMove(e.X, e.Y);
                if (e.Button == Mouse.Button.Middle)
                {
                    middleMouseDown = false;
                }
                else if (e.Button == Mouse.Button.Left)
                {
                    leftMouseDown = false;
                    viewWindow.OnPaint(e.X, e.Y, TileType.Ground);
                }
            };
            var previousMouseX = 0;
            var previousMouseY = 0;
            window.MouseMoved += (sender, e) =>
            {
                var mouseDeltaX = e.X - previousMouseX;
                var mouseDeltaY = e.Y - previousMouseY;

                viewWindow.OnMouseMove(e.X, e.Y);
                if (middleMouseDown && viewWindow.IsInWindow(middleDownStartX, middleDownStartY))
                {
                    viewWindow.OnPan(mouseDeltaX, mouseDeltaY);
                }
                previousMouseX = e.X;
                previousMouseY = e.Y;
            };


            Stopwatch stopWatch = new Stopwatch();
            var deltaTime = 0f;
            while (window.IsOpen)
            {
                deltaTime = stopWatch.ElapsedMilliseconds;
                stopWatch.Restart();

                ///
                /// EVENT HANDLING HERE
                ///
                window.DispatchEvents();
                var quit = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Escape);

                // DRAW STUFF
                window.Clear();
                viewWindow.OnRender(window);

                // DEBUG STUFF
                var debugText = new Text();
                debugText.CharacterSize = 32;
                debugText.DisplayedString = $"Ctrl: {ctrlKeyDown}";
                debugText.FillColor = Color.Red;
                debugText.OutlineColor = Color.Black;
                debugText.OutlineThickness = 1;
                debugText.Font = font;
                debugText.Position = new Vector2f(0, 0);

                window.Draw(debugText);

                window.Display();

                if (quit)
                {
                    window.Close();
                }
            }
        }
    }
}
