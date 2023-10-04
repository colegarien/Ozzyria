using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.MonoGameClient.UI.Windows;
using System.Collections.Generic;

namespace Ozzyria.MonoGameClient.UI
{
    internal class WindowManager
    {
        private InputTracker _inputTracker;

        // UI Components
        List<Window> _windows = new List<Window>();

        public WindowManager(InputTracker inputTracker)
        {
            _inputTracker = inputTracker;
        }

        public void AddWindow(Window window)
        {
            _windows.Add(window);

            ///
            /// Register Events
            ///
            _inputTracker.OnMouseUp += window.HandleMouseUp;
            _inputTracker.OnMouseDown += window.HandleMouseDown;
            _inputTracker.OnMouseMove += window.HandleMouseMove;
            _inputTracker.OnMouseVerticalScroll += window.HandleVerticalScroll;
            _inputTracker.OnMouseHorizontalScroll += window.HandleHorizontalScroll;
        }

        public void Update(float deltaTime)
        {
            _inputTracker.Calculate(deltaTime);


            // TODO UI Add in some kind of optional key listener/event so windows control their own visibility?
            if (_inputTracker.IsKeyReleased(Keys.I))
            {
                foreach (var window in _windows)
                {
                    window.IsVisible = !window.IsVisible;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO UI consider "focus" when/if there are multiple windows layering
            foreach (var window in _windows)
            {
                window.Draw(spriteBatch);
            }
        }
    }
}
