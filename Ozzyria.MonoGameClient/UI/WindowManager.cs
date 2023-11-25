﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            window.Manager = this;
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

        public void ToggleWindowVisibility(Window window)
        {
            if (window == null)
                return;

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
        }

        public void CloseWindow(Window window)
        {
            if (window == null)
                return;

            // Close and Refocus Manager
            window.IsVisible = false;
            if(_focusedWindow == window.Guid)
            {
                _focusedWindow = _windows.FirstOrDefault(w => w.Guid != window.Guid && w.IsVisible)?.Guid ?? Guid.Empty;
            }
        }

        public void OpenWindow(Window window)
        {
            if (window == null)
                return;

            // Open and Focus Window
            window.IsVisible = true;
            _focusedWindow = window.Guid;
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

            /*
            // TODO UI move this code somewhere to sync down bags and open them up (.. or periodically just request bags?)
            if (button == MouseButton.Right)
            {
                var clickedBag = context.GetEntities().FirstOrDefault(e =>
                {
                    if (!e.HasComponent(typeof(Ozzyria.Game.Components.Movement)) || !e.HasComponent(typeof(Game.Components.Bag)))
                    {
                        return false;
                    }

                    var m = (Game.Components.Movement)e.GetComponent(typeof(Game.Components.Movement));
                    if (Math.Pow(m.X - mouseState.X / 2, 2) + Math.Pow(m.Y - mouseState.Y / 2, 2) <= 100)
                    {
                        return true;
                    }
                    return false;
                });

                if (clickedBag != null)
                {
                    game.Client?.RequestBagContents(clickedBag.id);
                }
            }
            */
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
                if (window.HandleKeysPressed(tracker))
                    break;
            }
        }

        public void OnKeysHeld(InputTracker tracker)
        {
            foreach (var window in _windows.OrderByDescending(w => w.IsVisible && w.Guid == _focusedWindow))
            {
                if (window.HandleKeysHeld(tracker))
                    break;
            }
        }

        public void OnKeysReleased(InputTracker tracker)
        {
            if (tracker.IsKeyReleased(Keys.Escape))
            {
                if (_windows.Any(w => w.IsVisible))
                {
                    foreach (var window in _windows)
                    {
                        CloseWindow(window);
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
                if (window.HandleKeysReleased(tracker))
                    break;
            }
        }

    }
}
