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

            MapManager.LoadMap(new Map(20, 20)); // TODO allow save/load from file

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
