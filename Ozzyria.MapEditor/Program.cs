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
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            window.Closed += (sender, e) =>
            {
                ((Window)sender).Close();
            };
            var font = new Font("Fonts\\Bitter-Regular.otf");

            var controlled = new Entity();
            controlled.AttachComponent(new Movement() { X = 100, Y = 100, PreviousX = 100, PreviousY = 100 });
            controlled.AttachComponent(new BoundingBox() { Width = 20, Height = 20 });


            var tileSize = 32f;
            var mapWidth = 32;
            var mapHeight = 32;
            var mapXOffset = 0f;
            var mapYOffset = 0f;
            var map = new int[mapWidth, mapHeight];

            var mouseCurrentPosition = Mouse.GetPosition(window);
            var mousePreviousPosition = mouseCurrentPosition;

            var contextMenuOpen = false;
            var contextTileX = -1;
            var contextTileY = -1;

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


                mousePreviousPosition = mouseCurrentPosition;
                mouseCurrentPosition = Mouse.GetPosition(window);
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    contextMenuOpen = false;
                    contextTileX = -1;
                    contextTileY = -1;

                    mapXOffset += mouseCurrentPosition.X - mousePreviousPosition.X;
                    mapYOffset += mouseCurrentPosition.Y - mousePreviousPosition.Y;
                }
                else if (Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    contextMenuOpen = true;
                    contextTileX = mouseCurrentPosition.X;
                    contextTileY = mouseCurrentPosition.Y;
                }

                // DRAW STUFF
                window.Clear();

                for(var x = 0; x < mapWidth; x++)
                {
                    for (var y = 0; y < mapHeight; y++)
                    {
                        var shape = new RectangleShape(new SFML.System.Vector2f(tileSize, tileSize));
                        shape.FillColor = Color.Transparent;
                        shape.OutlineColor = Color.White;
                        shape.OutlineThickness = 1;
                        shape.Position = new SFML.System.Vector2f(mapXOffset + (x * tileSize), mapYOffset + (y * tileSize));

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
