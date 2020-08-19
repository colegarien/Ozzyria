using Ozzyria.Game;
using Ozzyria.Game.Component;
using SFML.Graphics;
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

        class Tool
        {
            public float PreviousX { get; set; }
            public float PreviousY { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float DeltaX { get => X - PreviousX; }
            public float DeltaY { get => Y - PreviousY; }
            public int Layer { get; set; } = 0;

            public ToolType Type { get; set; } = ToolType.Pan;
            public BrushType Brush { get; set; } = BrushType.Ground;

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
            var keyDelayTime = 200f;
            var keyTimer = keyDelayTime;

            var settings = new MapViewerSettings
            {
                TileMapWidth = 32,
                TileMapHeight = 32,
                MapOffsetX = 0,
                MapOffsetY = 0
            };

            var tiles = new Dictionary<int, int[,]>();
            for (var layer = 0; layer < settings.MaxLayers; layer++)
            {
                tiles[layer] = new int[settings.TileMapWidth, settings.TileMapHeight];
            }

            var tool = new Tool();
            var mousePosition = Mouse.GetPosition(window);
            tool.MoveTool(mousePosition.X, mousePosition.Y);

            window.MouseWheelScrolled += (sender, e) =>
            {
                if (e.Delta == 1)
                {
                    // Scroll through tools
                    tool.Type = (ToolType)(((int)tool.Type + 1) % Enum.GetValues(typeof(ToolType)).Length);
                }
                if (e.Delta == -1)
                {
                    // Scroll through brushes
                    tool.Brush = (BrushType)(((int)tool.Brush+1) % Enum.GetValues(typeof(BrushType)).Length);
                    
                }
            };

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

                if (keyTimer < keyDelayTime)
                {
                    // elapse time between last key press
                    keyTimer += deltaTime;
                }

                if(Mouse.IsButtonPressed(Mouse.Button.Middle) && keyTimer >= keyDelayTime)
                {
                    keyTimer = 0f;
                    tool.Layer = (tool.Layer + 1) % settings.MaxLayers;
                }

                mousePosition = Mouse.GetPosition(window);
                tool.MoveTool(mousePosition.X, mousePosition.Y);
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    contextMenuOpen = false;

                    if (tool.Type == ToolType.Pan)
                    {
                        settings.MapOffsetX += tool.DeltaX;
                        settings.MapOffsetY += tool.DeltaY;
                    }
                    else if(tool.Type == ToolType.Paint)
                    {
                        var tileX = settings.ScreenToTileX(tool.X);
                        var tileY = settings.ScreenToTileY(tool.Y);

                        if(tileX >= 0 && tileX < settings.TileMapWidth && tileY >= 0 && tileY < settings.TileMapHeight)
                        {
                            tiles[tool.Layer][tileX, tileY] = (int)tool.Brush;
                        }
                    }
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
                        switch(tiles[tool.Layer][x, y])
                        {
                            case (int)BrushType.Ground:
                                shape.FillColor = Color.Green;
                                break;
                            case (int)BrushType.Water:
                                shape.FillColor = Color.Blue;
                                break;
                            case (int)BrushType.Fence:
                                shape.FillColor = Color.Red;
                                break;
                            default:
                                shape.FillColor = Color.Transparent;
                                break;
                        }
                        shape.OutlineColor = Color.White;
                        shape.OutlineThickness = 1;
                        shape.Position = new SFML.System.Vector2f(settings.TileToScreenX(x), settings.TileToScreenY(y));

                        window.Draw(shape);
                    }
                }

                var cursor = new RectangleShape(new SFML.System.Vector2f(settings.TileSize, settings.TileSize));
                cursor.FillColor = Color.Transparent;
                cursor.OutlineColor = Color.Blue;
                cursor.OutlineThickness = 4;
                cursor.Position = new SFML.System.Vector2f(settings.TileToScreenX(settings.ScreenToTileX(tool.X)), settings.TileToScreenY(settings.ScreenToTileY(tool.Y)));
                window.Draw(cursor);

                if (contextMenuOpen)
                {
                    var shape = new RectangleShape(new SFML.System.Vector2f(100, 200));
                    shape.FillColor = Color.Blue;
                    shape.OutlineColor = Color.White;
                    shape.OutlineThickness = 2;
                    shape.Position = new SFML.System.Vector2f(contextTileX, contextTileY);

                    window.Draw(shape);


                    var someText = new Text();
                    someText.CharacterSize = 16;
                    someText.DisplayedString = "menu";
                    someText.FillColor = Color.Red;
                    someText.Font = font;
                    someText.Position = new SFML.System.Vector2f(contextTileX, contextTileY);

                    window.Draw(someText);
                }

                // DEBUG STUFF
                var debugText = new Text();
                debugText.CharacterSize = 32;
                debugText.DisplayedString = $"Layer: {tool.Layer}\nTool Type: {tool.Type}\nBrush Type: {tool.Brush}\nTile X: {settings.ScreenToTileX(tool.X)}\nTile Y: {settings.ScreenToTileY(tool.Y)}";
                debugText.FillColor = Color.Red;
                debugText.OutlineColor = Color.Black;
                debugText.OutlineThickness = 1;
                debugText.Font = font;
                debugText.Position = new SFML.System.Vector2f(0, 0);

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
