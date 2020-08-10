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

                // DRAW STUFF
                window.Clear();
                
                var someText = new Text();
                someText.DisplayedString = "test";
                someText.FillColor = Color.Red;
                someText.Font = font;
                window.Draw(someText);

                window.Display();

                if (quit)
                {
                    window.Close();
                }
            }
        }
    }
}
