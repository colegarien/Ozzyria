using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ozzyria.MonoGameClient.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using static Ozzyria.MonoGameClient.UI.InputTracker;

namespace Ozzyria.MonoGameClient.UI
{
    internal class WindowManager
    {
        private InputTracker _inputTracker;
        private bool _quitRequested = false;

        // UI Components
        List<Window> _windows = new List<Window>();
        Guid _focusedWindow = Guid.Empty;

        public WindowManager(InputTracker inputTracker)
        {
            _inputTracker = inputTracker;

            ///
            /// Register Event Pipeline
            ///
            _inputTracker.OnMouseUp += OnMouseUp;
            _inputTracker.OnMouseDown += OnMouseDown;
            _inputTracker.OnMouseMove += OnMouseMove;
            _inputTracker.OnMouseVerticalScroll += OnMouseVerticalScroll;
            _inputTracker.OnMouseHorizontalScroll += OnMouseHorizontalScroll;
            _inputTracker.OnKeysPressed += OnKeysPressed;
            _inputTracker.OnKeysHeld += OnKeysHeld;
            _inputTracker.OnKeysReleased += OnKeysReleased;
    }

        public void AddWindow(Window window)
        {
            _windows.Add(window);
        }

        public bool QuitRequested()
        {
            return _quitRequested;
        }

        public void Update(float deltaTime)
        {
            _inputTracker.Calculate(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var window in _windows.OrderBy(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                window.Draw(spriteBatch);
            }
        }

        public void OnMouseDown(MouseButton button, int x, int y)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                if (window.HandleMouseDown(button, x, y))
                {
                    _focusedWindow = window.Guid;
                    break;
                }
            }
        }

        public void OnMouseMove(int previousX, int previousY, int x, int y)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                if (window.HandleMouseMove(previousX, previousY, x, y))
                    break;
            }
        }

        public void OnMouseUp(MouseButton button, int x, int y)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                if (window.HandleMouseUp(button, x, y))
                    break;
            }

        }

        public void OnMouseVerticalScroll(int x, int y, float delta)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                if (window.HandleVerticalScroll(x, y, delta))
                    break;
            }
        }

        public void OnMouseHorizontalScroll(int x, int y, float delta)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                if (window.HandleHorizontalScroll(x, y, delta))
                    break;
            }
        }

        public void OnKeysPressed(InputTracker tracker)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                // TODO UI wire up into Window class
            }
        }

        public void OnKeysHeld(InputTracker tracker)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                // TODO UI wire up into Window class
            }
        }

        public void OnKeysReleased(InputTracker tracker)
        {
            if (tracker.IsKeyReleased(Keys.Escape))
            {
                if (_windows.Any(w => w.IsVisible))
                {
                    _focusedWindow = Guid.Empty;
                    foreach (var window in _windows)
                    {
                        window.IsVisible = false;
                    }
                }
                else
                {
                    _quitRequested = true;
                }

                return;
            }

            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                // TODO UI move key handling logic into Window class itself, Also centralize _focusedWindow handling / focus requests
                if((window is InventoryWindow && tracker.IsKeyReleased(Keys.I)) || (window is ConsoleWindow && tracker.IsKeyReleased(Keys.OemTilde)))
                {
                    if (window.IsVisible)
                    {
                        if (_focusedWindow == window.Guid)
                        {
                            // Close window and focus on other open window
                            window.IsVisible = false;
                            _focusedWindow = _windows.FirstOrDefault(w => w.Guid != window.Guid && w.IsVisible)?.Guid ?? Guid.Empty;
                        }
                        else
                        {
                            // Bring window back into focus
                            _focusedWindow = window.Guid;
                        }
                    }
                    else
                    {
                        // Open and Focus Window
                        window.IsVisible = true;
                        _focusedWindow = window.Guid;
                    }

                    break;
                }
            }
        }

    }
}
