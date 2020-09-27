using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Linq;

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

            viewWindow.LoadMap(new Map(20, 20)); // TODO load/unload from file
            layerWindow.NumberOfLayers = viewWindow._map.layers.Count;

            window.Resized += (sender, e) =>
            {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));

                // TODO wrap this in a 'Layout' class that calculates all this junk
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
                var horizontalScroll = false;
                var verticalScroll = false;
                var zooming = false;
                if (e.Wheel == Mouse.Wheel.HorizontalWheel || (inputState.IsAltHeld && e.Wheel == Mouse.Wheel.VerticalWheel))
                {
                    horizontalScroll = true;
                }
                else if (inputState.IsCtrlHeld)
                {
                    zooming = true;
                }
                else
                {
                    verticalScroll = true;
                }


                // TODO make event listeners delegated easier (GWindow event system wrapper?)
                if (viewWindow.IsInWindow(e.X, e.Y))
                {
                    if (horizontalScroll)
                        viewWindow.OnHorizontalScroll(e.Delta);
                    else if (verticalScroll)
                        viewWindow.OnVerticalScroll(e.Delta);
                    else if (zooming)
                        viewWindow.OnZoom(e.X, e.Y, e.Delta);
                }

                if (brushWindow.IsInWindow(e.X, e.Y))
                {
                    if (horizontalScroll)
                        brushWindow.OnHorizontalScroll(e.Delta);
                    else if (verticalScroll)
                        brushWindow.OnVerticalScroll(e.Delta);
                }

                if (layerWindow.IsInWindow(e.X, e.Y))
                {
                    if (horizontalScroll)
                        layerWindow.OnHorizontalScroll(e.Delta);
                    else if (verticalScroll)
                        layerWindow.OnVerticalScroll(e.Delta);
                }
            };
            window.MouseButtonPressed += (sender, e) =>
            {
                viewWindow.OnMouseMove(e.X, e.Y);
                brushWindow.OnMouseMove(e.X, e.Y);
                layerWindow.OnMouseMove(e.X, e.Y);
                if (e.Button == Mouse.Button.Middle)
                {
                    inputState.MiddleMouseDown = true;
                    inputState.MiddleDownStartX = e.X;
                    inputState.MiddleDownStartY = e.Y;
                }
                else if (e.Button == Mouse.Button.Left)
                {
                    inputState.LeftMouseDown = true;
                    inputState.LeftDownStartX = e.X;
                    inputState.LeftDownStartY = e.Y;
                    if (viewWindow.IsInWindow(e.X, e.Y))
                        viewWindow.OnPaint(e.X, e.Y, brushWindow.SelectedBrush);
                    if (brushWindow.IsInWindow(e.X, e.Y))
                        brushWindow.OnPickTool(e.X, e.Y);
                    if (layerWindow.IsInWindow(e.X, e.Y))
                    {
                        var result = layerWindow.OnPickLayer(e.X, e.Y);
                        if (result == -2)
                        {
                            viewWindow._map.AddLayer();
                            layerWindow.NumberOfLayers = viewWindow._map.layers.Count;
                        }
                        else if (result >= 0)
                        {
                            viewWindow._map.RemoveLayer(result);
                            if (result != 0 && result <= layerWindow.CurrentLayer)
                            {
                                layerWindow.CurrentLayer -= result == layerWindow.CurrentLayer && viewWindow._map.layers.ContainsKey(result)
                                    ? 0
                                    : 1;
                            }
                            layerWindow.NumberOfLayers = viewWindow._map.layers.Count;
                            viewWindow.Layer = layerWindow.CurrentLayer;
                        }
                        layerWindow.NumberOfLayers = viewWindow._map.layers.Count;
                        viewWindow.Layer = layerWindow.CurrentLayer;
                    }
                }
            };
            window.MouseButtonReleased += (sender, e) =>
            {
                viewWindow.OnMouseMove(e.X, e.Y);
                brushWindow.OnMouseMove(e.X, e.Y);
                layerWindow.OnMouseMove(e.X, e.Y);
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
                brushWindow.OnMouseMove(e.X, e.Y);
                layerWindow.OnMouseMove(e.X, e.Y);
                if (inputState.MiddleMouseDown && viewWindow.IsInWindow(inputState.MiddleDownStartX, inputState.MiddleDownStartY))
                {
                    viewWindow.OnPan(mouseDeltaX, mouseDeltaY);
                }

                if (inputState.LeftMouseDown && viewWindow.IsInWindow(inputState.LeftDownStartX, inputState.LeftDownStartY))
                {
                    viewWindow.OnPaint(e.X, e.Y, brushWindow.SelectedBrush);
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
