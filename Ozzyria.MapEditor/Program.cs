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
        enum ToolType
        {
            Pan,
            Paint
        }

        enum BrushType
        {
            None,
            Ground,
            Water,
            Fence
        }


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
            var keyDelayTime = 200f;
            var keyTimer = keyDelayTime;

            var settings = new MapViewerSettings
            {
                TileMapWidth = 32,
                TileMapHeight = 32,
                MapOffsetX = 0,
                MapOffsetY = 0
            };

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

            var middleMouseDown = false;
            var middleDownStartX = 0;
            var middleDownStartY = 0;
            window.MouseButtonPressed += (sender, e) =>
            {
                if(e.Button == Mouse.Button.Middle)
                {
                    middleMouseDown = true;
                    middleDownStartX = e.X;
                    middleDownStartY = e.Y;
                }
            };
            window.MouseButtonReleased += (sender, e) =>
            {
                if (e.Button == Mouse.Button.Middle)
                {
                    middleMouseDown = false;
                }
            };
            var previousMouseX = 0;
            var previousMouseY = 0;
            window.MouseMoved += (sender, e) =>
            {
                if (middleMouseDown && viewWindow.IsInWindow(middleDownStartX, middleDownStartY))
                {
                    viewWindow.OnPan(e.X - previousMouseX, e.Y - previousMouseY);
                }
                previousMouseX = e.X;
                previousMouseY = e.Y;
            };



            var mousePosition = Mouse.GetPosition(window);

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

                if (keyTimer < keyDelayTime)
                {
                    // elapse time between last key press
                    keyTimer += deltaTime;
                }

                if (Mouse.IsButtonPressed(Mouse.Button.Middle) && keyTimer >= keyDelayTime)
                {
                    keyTimer = 0f;
                }

                mousePosition = Mouse.GetPosition(window);

                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {

                }
                else if (Mouse.IsButtonPressed(Mouse.Button.Right))
                {

                }

                // DRAW STUFF
                window.Clear();

                //for (var x = 0; x < settings.TileMapWidth; x++)
                //{
                //    for (var y = 0; y < settings.TileMapHeight; y++)
                //    {
                //        var shape = new RectangleShape(new SFML.System.Vector2f(settings.TileSize, settings.TileSize));
                //        switch (tiles[tool.Layer][x, y])
                //        {
                //            case (int)BrushType.Ground:
                //                shape.FillColor = Color.Green;
                //                break;
                //            case (int)BrushType.Water:
                //                shape.FillColor = Color.Blue;
                //                break;
                //            case (int)BrushType.Fence:
                //                shape.FillColor = Color.Red;
                //                break;
                //            default:
                //                shape.FillColor = Color.Transparent;
                //                break;
                //        }
                //        shape.OutlineColor = Color.White;
                //        shape.OutlineThickness = 1;
                //        shape.Position = new SFML.System.Vector2f(settings.TileToScreenX(x), settings.TileToScreenY(y));

                //        window.Draw(shape);
                //    }
                //}

                viewWindow.OnRender(window);

                var cursor = new RectangleShape(new Vector2f(settings.TileSize, settings.TileSize));
                cursor.FillColor = Color.Transparent;
                cursor.OutlineColor = Color.Blue;
                cursor.OutlineThickness = 4;
                // cursor.Position = new SFML.System.Vector2f(settings.TileToScreenX(settings.ScreenToTileX(tool.X)), settings.TileToScreenY(settings.ScreenToTileY(tool.Y)));
                window.Draw(cursor);

                //if (contextMenuOpen)
                //{
                //    var shape = new RectangleShape(new SFML.System.Vector2f(100, 200));
                //    shape.FillColor = Color.Blue;
                //    shape.OutlineColor = Color.White;
                //    shape.OutlineThickness = 2;
                //    shape.Position = new SFML.System.Vector2f(contextTileX, contextTileY);

                //    window.Draw(shape);


                //    var someText = new Text();
                //    someText.CharacterSize = 16;
                //    someText.DisplayedString = "menu";
                //    someText.FillColor = Color.Red;
                //    someText.Font = font;
                //    someText.Position = new SFML.System.Vector2f(contextTileX, contextTileY);

                //    window.Draw(someText);
                //}

                // DEBUG STUFF
                var debugText = new Text();
                debugText.CharacterSize = 32;
                debugText.DisplayedString = $"Ctrl: {ctrlKeyDown}" ;
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
