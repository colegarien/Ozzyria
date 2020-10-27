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
        static void Main()
        {
            var inputState = new InputState();

            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Ozzyria");
            ViewWindow viewWindow = new ViewWindow(0, 0, (uint)(window.Size.X * 0.6), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y, 10, 0);
            BrushWindow brushWindow = new BrushWindow(0, (int)(window.Size.Y * 0.6), (uint)(window.Size.X * 0.6), 72, window.Size.X, window.Size.Y, 10, 10);
            ToolWindow toolWindow = new ToolWindow(0, (int)(window.Size.Y * 0.6) + 72, (uint)(window.Size.X * 0.6), 72, window.Size.X, window.Size.Y, 10, 10);
            LayerWindow layerWindow = new LayerWindow((int)(window.Size.X * 0.6), 0, (uint)(window.Size.X * 0.4), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y, 10, 10);

            EventQueue.AttachObserver(viewWindow);
            EventQueue.AttachObserver(brushWindow);
            EventQueue.AttachObserver(toolWindow);
            EventQueue.AttachObserver(layerWindow);

            MapManager.LoadMap(new Map(32, 32)); // TODO allow save/load from file

            window.Resized += (sender, e) =>
            {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));

                viewWindow.OnResize(0, 0, (uint)(window.Size.X * 0.6), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
                brushWindow.OnResize(0, (int)(window.Size.Y * 0.6), (uint)(window.Size.X * 0.6), 72, window.Size.X, window.Size.Y);
                toolWindow.OnResize(0, (int)(window.Size.Y * 0.6) + 72, (uint)(window.Size.X * 0.6), 72, window.Size.X, window.Size.Y);
                layerWindow.OnResize((int)(window.Size.X * 0.6), 0, (uint)(window.Size.X * 0.4), (uint)(window.Size.Y * 0.6), window.Size.X, window.Size.Y);
            };
            window.Closed += (sender, e) =>
            {
                ((Window)sender).Close();
            };
            window.KeyPressed += inputState.HandleSfmlKeyPressed;
            window.KeyReleased += inputState.HandleSfmlKeyReleased;
            window.MouseWheelScrolled += inputState.HandleSfmlMouseWheelScrolled;
            window.MouseButtonPressed += inputState.HandleSfmlMousePressed;
            window.MouseButtonReleased += inputState.HandleSfmlMouseReleased;
            window.MouseMoved += inputState.HandleSfmlMouseMoved;


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
                toolWindow.OnRender(window);
                layerWindow.OnRender(window);

                // DEBUG STUFF
                var debugText = new Text
                {
                    CharacterSize = 16,
                    DisplayedString = $"Zoom: {Math.Round(viewWindow.zoomPercent * 100)}%  | Layer: {layerWindow.CurrentLayer} | Brush: {brushWindow.SelectedBrush}",
                    FillColor = Color.Red,
                    OutlineColor = Color.Black,
                    OutlineThickness = 1,
                    Font = FontFactory.GetRegular(),
                    Position = new Vector2f(0, 15 + (int)(15 + window.Size.Y * 0.6) + 110)
                };
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
