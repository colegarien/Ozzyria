using Ozzyria.MapEditor.EventSystem;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;

namespace Ozzyria.MapEditor
{

    class Program
    {
        static void Main(string[] args)
        {
            var inputState = new InputState();

            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            ViewWindow viewWindow = new ViewWindow(15, 15, (uint)(window.Size.X * 0.6), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
            BrushWindow brushWindow = new BrushWindow(15, 15 + (int)(15 + window.Size.Y * 0.6), (uint)(window.Size.X * 0.6), 52, window.Size.X, window.Size.Y);
            LayerWindow layerWindow = new LayerWindow((int)(window.Size.X * 0.6) + 30, 15, (uint)(window.Size.X * 0.4) - 45, (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);

            EventQueue.AttachObserver(viewWindow);
            EventQueue.AttachObserver(brushWindow);
            EventQueue.AttachObserver(layerWindow);

            MapManager.LoadMap(new Map(20, 20)); // Needs to happen after observers setup

            window.Resized += (sender, e) =>
            {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));

                // TODO wrap this in a 'Layout' class that calculates all this junk / make a OnResize event?
                viewWindow.OnResize(15, 15, (uint)(window.Size.X * 0.6), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
                brushWindow.OnResize(15, 15 + (int)(15 + window.Size.Y * 0.6), (uint)(window.Size.X * 0.6), 52, window.Size.X, window.Size.Y);
                layerWindow.OnResize((int)(window.Size.X * 0.6) + 30, 15, (uint)(window.Size.X * 0.4) - 45, (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
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
                if (e.Wheel == Mouse.Wheel.HorizontalWheel || (inputState.IsAltHeld && e.Wheel == Mouse.Wheel.VerticalWheel))
                {
                    EventQueue.Queue(new HorizontalScrollEvent
                    {
                        OriginX = e.X,
                        OriginY = e.Y,
                        Delta = e.Delta
                    });
                }
                else if (inputState.IsCtrlHeld)
                {
                    EventQueue.Queue(new ZoomEvent
                    {
                        OriginX = e.X,
                        OriginY = e.Y,
                        Delta = e.Delta
                    });
                }
                else
                {
                    EventQueue.Queue(new VerticalScrollEvent
                    {
                        OriginX = e.X,
                        OriginY = e.Y,
                        Delta = e.Delta
                    });
                }
            };
            window.MouseButtonPressed += (sender, e) =>
            {
                inputState.PreviousMouseX = inputState.CurrentMouseX;
                inputState.PreviousMouseY = inputState.CurrentMouseY;
                inputState.CurrentMouseX = e.X;
                inputState.CurrentMouseY = e.Y;
                EventQueue.Queue(new EventSystem.MouseMoveEvent()
                {
                    DeltaX = inputState.CurrentMouseX - inputState.PreviousMouseX,
                    DeltaY = inputState.CurrentMouseY - inputState.PreviousMouseY,
                    X = e.X,
                    Y = e.Y,
                });

                EventQueue.Queue(new MouseDownEvent
                {
                    OriginX = e.X,
                    OriginY = e.Y,
                    LeftMouseDown = e.Button == Mouse.Button.Left,
                    RightMouseDown = e.Button == Mouse.Button.Right,
                    MiddleMouseDown = e.Button == Mouse.Button.Middle,
                });

                if (e.Button == Mouse.Button.Middle)
                {
                    inputState.MiddleMouseDown = true;
                    inputState.DragStartX = e.X;
                    inputState.DragStartY = e.Y;
                }
                else if (e.Button == Mouse.Button.Left)
                {
                    inputState.LeftMouseDown = true;
                    inputState.DragStartX = e.X;
                    inputState.DragStartY = e.Y;
                }
                else if (e.Button == Mouse.Button.Right)
                {
                    inputState.RightMouseDown = true;
                    inputState.DragStartX = e.X;
                    inputState.DragStartY = e.Y;
                }

            };
            window.MouseButtonReleased += (sender, e) =>
            {
                inputState.PreviousMouseX = inputState.CurrentMouseX;
                inputState.PreviousMouseY = inputState.CurrentMouseY;
                inputState.CurrentMouseX = e.X;
                inputState.CurrentMouseY = e.Y;
                EventQueue.Queue(new EventSystem.MouseMoveEvent()
                {
                    DeltaX = inputState.CurrentMouseX - inputState.PreviousMouseX,
                    DeltaY = inputState.CurrentMouseY - inputState.PreviousMouseY,
                    X = e.X,
                    Y = e.Y,
                });

                if (e.Button == Mouse.Button.Middle)
                {
                    inputState.MiddleMouseDown = false;
                }
                else if (e.Button == Mouse.Button.Left)
                {
                    inputState.LeftMouseDown = false;
                }
                else if (e.Button == Mouse.Button.Right)
                {
                    inputState.RightMouseDown = false;
                }
            };

            window.MouseMoved += (sender, e) =>
            {
                inputState.PreviousMouseX = inputState.CurrentMouseX;
                inputState.PreviousMouseY = inputState.CurrentMouseY;
                inputState.CurrentMouseX = e.X;
                inputState.CurrentMouseY = e.Y;

                EventQueue.Queue(new EventSystem.MouseMoveEvent()
                {
                    DeltaX = inputState.CurrentMouseX - inputState.PreviousMouseX,
                    DeltaY = inputState.CurrentMouseY - inputState.PreviousMouseY,
                    X = e.X,
                    Y = e.Y,
                });

                if (inputState.LeftMouseDown || inputState.RightMouseDown || inputState.MiddleMouseDown)
                {
                    EventQueue.Queue(new MouseDragEvent()
                    {
                        OriginX = inputState.DragStartX,
                        OriginY = inputState.DragStartY,
                        DeltaX = inputState.CurrentMouseX - inputState.PreviousMouseX,
                        DeltaY = inputState.CurrentMouseY - inputState.PreviousMouseY,
                        X = e.X,
                        Y = e.Y,
                        LeftMouseDown = inputState.LeftMouseDown,
                        RightMouseDown = inputState.RightMouseDown,
                        MiddleMouseDown = inputState.MiddleMouseDown,
                    });
                }
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
                EventQueue.DispatchEvents();
                var quit = window.HasFocus() && Keyboard.IsKeyPressed(Keyboard.Key.Escape);

                // DRAW STUFF
                window.Clear();
                viewWindow.OnRender(window);
                brushWindow.OnRender(window);
                layerWindow.OnRender(window);

                // DEBUG STUFF
                var debugText = new Text();
                debugText.CharacterSize = 16;
                debugText.DisplayedString = $"Zoom: {Math.Round(viewWindow.zoomPercent * 100)}%  | Layer: {layerWindow.CurrentLayer} | Brush: {brushWindow.SelectedBrush}";
                debugText.FillColor = Color.Red;
                debugText.OutlineColor = Color.Black;
                debugText.OutlineThickness = 1;
                debugText.Font = FontFactory.GetRegular();
                debugText.Position = new Vector2f(0, 15 + (int)(15 + window.Size.Y * 0.6) + 67);
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
