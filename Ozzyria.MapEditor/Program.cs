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
        static void Main(string[] args)
        {
            var font = new Font("Fonts\\Bitter-Regular.otf");
            var paintType = TileType.Ground;
            var inputState = new InputState();

            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            ViewWindow viewWindow = new ViewWindow(15, 15, (uint)(window.Size.X * 0.6), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
            viewWindow.LoadMap(new Map(10, 10)); // TODO load/unload from file
            window.Resized += (sender, e) =>
            {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                viewWindow.ResizeWindow(40, 40, (uint)(window.Size.X * 0.6), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
            };
            window.Closed += (sender, e) =>
            {
                ((Window)sender).Close();
            };
            window.KeyPressed += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.LControl || e.Code == Keyboard.Key.RControl)
                {
                    inputState.IsCtrlHeld = true;
                }
                else if (e.Code == Keyboard.Key.LAlt || e.Code == Keyboard.Key.RAlt)
                {
                    inputState.IsAltHeld = true;
                }
                else if (e.Code == Keyboard.Key.LShift || e.Code == Keyboard.Key.RShift)
                {
                    inputState.IsShiftHeld = true;
                }
            };
            window.KeyReleased += (sender, e) =>
            {
                if (e.Code == Keyboard.Key.LControl || e.Code == Keyboard.Key.RControl)
                {
                    inputState.IsCtrlHeld = false;
                }
                else if (e.Code == Keyboard.Key.LAlt || e.Code == Keyboard.Key.RAlt)
                {
                    inputState.IsAltHeld = false;
                }
                else if (e.Code == Keyboard.Key.LShift || e.Code == Keyboard.Key.RShift)
                {
                    inputState.IsShiftHeld = false;
                }
            };
            window.MouseWheelScrolled += (sender, e) =>
            {
                if (!viewWindow.IsInWindow(e.X, e.Y))
                {
                    return;
                }

                if(e.Wheel == Mouse.Wheel.HorizontalWheel || (inputState.IsAltHeld && e.Wheel == Mouse.Wheel.VerticalWheel))
                {
                    viewWindow.OnHorizontalScroll(e.Delta);
                }
                else if (inputState.IsCtrlHeld)
                {
                    viewWindow.OnZoom(e.X, e.Y, e.Delta);
                }
                else {
                    viewWindow.OnVerticalScroll(e.Delta);
                }
            };
            window.MouseButtonPressed += (sender, e) =>
            {
                viewWindow.OnMouseMove(e.X, e.Y);
                if (e.Button == Mouse.Button.Middle)
                {
                    inputState.MiddleMouseDown = true;
                    inputState.MiddleDownStartX = e.X;
                    inputState.MiddleDownStartY = e.Y;
                }
                else if(e.Button == Mouse.Button.Left)
                {
                    inputState.LeftMouseDown = true;
                    inputState.LeftDownStartX = e.X;
                    inputState.LeftDownStartY = e.Y;
                    viewWindow.OnPaint(e.X, e.Y, paintType);
                }
            };
            window.MouseButtonReleased += (sender, e) =>
            {
                viewWindow.OnMouseMove(e.X, e.Y);
                if (e.Button == Mouse.Button.Middle)
                {
                    inputState.MiddleMouseDown = false;
                }
                else if (e.Button == Mouse.Button.Left)
                {
                    inputState.LeftMouseDown = false;
                }
            };
            var previousMouseX = 0;
            var previousMouseY = 0;
            window.MouseMoved += (sender, e) =>
            {
                var mouseDeltaX = e.X - previousMouseX;
                var mouseDeltaY = e.Y - previousMouseY;

                viewWindow.OnMouseMove(e.X, e.Y);
                if (inputState.MiddleMouseDown && viewWindow.IsInWindow(inputState.MiddleDownStartX, inputState.MiddleDownStartY))
                {
                    viewWindow.OnPan(mouseDeltaX, mouseDeltaY);
                }

                if(inputState.LeftMouseDown && viewWindow.IsInWindow(inputState.LeftDownStartX, inputState.LeftDownStartY))
                {
                    viewWindow.OnPaint(e.X, e.Y, paintType);
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
                debugText.DisplayedString = $"Ctrl: {inputState.IsCtrlHeld}";
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
