using Ozzyria.Game;
using Ozzyria.Game.Component;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Diagnostics;

namespace Ozzyria.MapEditor
{
    class Program
    {
        enum ToolType
        {
            Pan,
            Paint,
            Erase
        }

        enum BrushType
        {
            Ground,
            Water,
            Fence
        }


        class MapViewerSettings
        {
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

        class Tool
        {
            public float PreviousX { get; set; }
            public float PreviousY { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float DeltaX { get => X - PreviousX; }
            public float DeltaY { get => Y - PreviousY; }

            public ToolType Type { get; set; } = ToolType.Pan;

            public void MoveTool(float x, float y)
            {
                PreviousX = X;
                PreviousY = Y;
                X = x;
                Y = y;
            }

        }

        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) =>
            {
                ((Window)sender).Close();
            };
            var font = new Font("Fonts\\Bitter-Regular.otf");

            var settings = new MapViewerSettings
            {
                TileMapWidth = 32,
                TileMapHeight = 32,
                MapOffsetX = 0,
                MapOffsetY = 0
            };

            var tool = new Tool();
            var mousePosition = Mouse.GetPosition(window);
            tool.MoveTool(mousePosition.X, mousePosition.Y);

            var contextMenuOpen = false;
            var contextTileX = 0f;
            var contextTileY = 0f;

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


                mousePosition = Mouse.GetPosition(window);
                tool.MoveTool(mousePosition.X, mousePosition.Y);
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    contextMenuOpen = false;

                    settings.MapOffsetX += tool.DeltaX;
                    settings.MapOffsetY += tool.DeltaY;
                }
                else if (Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    contextMenuOpen = true;
                    contextTileX = tool.X;
                    contextTileY = tool.Y;
                }

                // DRAW STUFF
                window.Clear();

                for(var x = 0; x < settings.TileMapWidth; x++)
                {
                    for (var y = 0; y < settings.TileMapHeight; y++)
                    {
                        var shape = new RectangleShape(new SFML.System.Vector2f(settings.TileSize, settings.TileSize));
                        shape.FillColor = Color.Transparent;
                        shape.OutlineColor = Color.White;
                        shape.OutlineThickness = 1;
                        shape.Position = new SFML.System.Vector2f(settings.TileToScreenX(x), settings.TileToScreenY(y));

                        window.Draw(shape);
                    }
                }

                if (contextMenuOpen)
                {
                    var shape = new RectangleShape(new SFML.System.Vector2f(100, 200));
                    shape.FillColor = Color.Blue;
                    shape.OutlineColor = Color.White;
                    shape.OutlineThickness = 2;
                    shape.Position = new SFML.System.Vector2f(contextTileX, contextTileY);

                    window.Draw(shape);


                    var someText = new Text();
                    someText.DisplayedString = "menu";
                    someText.FillColor = Color.Black;
                    someText.Font = font;
                    someText.Position = new SFML.System.Vector2f(contextTileX, contextTileY);

                    window.Draw(someText);
                }

                window.Display();

                if (quit)
                {
                    window.Close();
                }
            }
        }
    }
}
