using Microsoft.Xna.Framework.Input;
using Ozzyria.Game.ECS;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient.UI
{
    internal class InputTracker
    {
        private Camera _camera;


        // Keyboard Tracking
        List<Keys> _pressedKeys = new List<Keys>();
        List<Keys> _downKeys = new List<Keys>();
        List<Keys> _releasedKeys = new List<Keys>();

        // Mouse Tracking
        int prevMouseX = -1;
        int prevMouseY = -1;
        int mouseX = -1;
        int mouseY = -1;

        bool prevLeftMouseDown = false;
        bool leftMouseDown = false;
        int leftMouseDownStartX = -1;
        int leftMouseDownStartY = -1;

        bool prevRightMouseDown = false;
        bool rightMouseDown = false;
        int rightMouseDownStartX = -1;
        int rightMouseDownStartY = -1;

        bool prevMiddleMouseDown = false;
        bool middleMouseDown = false;
        int middleMouseDownStartX = -1;
        int middleMouseDownStartY = -1;


        public enum MouseButton
        {
            Left,
            Right,
            Middle
        }
        public delegate void MouseButtonEvent(MouseButton button, int x, int y);
        public delegate void MouseMoveEvent(int previousX, int previousY, int x, int y);
        public event MouseButtonEvent OnMouseDown;
        public event MouseMoveEvent OnMouseMove;
        public event MouseButtonEvent OnMouseUp;


        public InputTracker(Camera camera)
        {
            _camera = camera;
        }

        public void Calculate(float deltaTime)
        {
            TrackKeyboard(deltaTime);
            TrackMouse(deltaTime);
        }

        private void TrackKeyboard(float deltaTime)
        {
            // Pressed => Freshly downed Keys
            // Down => Currently held down keys
            // Released => Freshly released keys
            var currentlyPressedKeys = Keyboard.GetState().GetPressedKeys().ToList();
            _pressedKeys.Clear();
            _releasedKeys.Clear();
            for (var i = _downKeys.Count - 1; i >= 0; i--)
            {
                var key = _downKeys[i];
                if (!currentlyPressedKeys.Contains(key))
                {
                    _releasedKeys.Add(key);
                    _downKeys.Remove(key);
                }
                else
                {
                    currentlyPressedKeys.Remove(key);
                }
            }
            foreach (var key in currentlyPressedKeys)
            {
                _pressedKeys.Add(key);
                _downKeys.Add(key);
            }
        }

        private void TrackMouse(float deltaTime)
        {
            var mouseState = Mouse.GetState();

            prevMouseX = mouseX;
            prevMouseY = mouseY;
            mouseX = (int)System.Math.Floor(mouseState.X / _camera.hScale);
            mouseY = (int)System.Math.Floor(mouseState.Y / _camera.vScale);

            prevLeftMouseDown = leftMouseDown;
            prevRightMouseDown = rightMouseDown;
            prevMiddleMouseDown = middleMouseDown;
            leftMouseDown = mouseState.LeftButton == ButtonState.Pressed;
            rightMouseDown = mouseState.RightButton == ButtonState.Pressed;
            middleMouseDown = mouseState.MiddleButton == ButtonState.Pressed;

            if(prevMouseX != mouseX || prevMouseY != mouseY)
            {
                OnMouseMove?.Invoke(prevMouseX, prevMouseY, mouseX, mouseY);
            }

            if(leftMouseDown && !prevLeftMouseDown)
            {
                leftMouseDownStartX = mouseX;
                leftMouseDownStartY = mouseY;

                OnMouseDown?.Invoke(MouseButton.Left, mouseX, mouseY);
            }
            else if (!leftMouseDown && prevLeftMouseDown)
            {
                OnMouseUp?.Invoke(MouseButton.Left, mouseX, mouseY);
            }

            if (rightMouseDown && !prevRightMouseDown)
            {
                rightMouseDownStartX = mouseX;
                rightMouseDownStartY = mouseY;

                OnMouseDown?.Invoke(MouseButton.Right, mouseX, mouseY);
            }
            else if (!rightMouseDown && prevRightMouseDown)
            {
                OnMouseUp?.Invoke(MouseButton.Right, mouseX, mouseY);
            }

            if (middleMouseDown && !prevMiddleMouseDown)
            {
                middleMouseDownStartX = mouseX;
                middleMouseDownStartY = mouseY;

                OnMouseDown?.Invoke(MouseButton.Middle, mouseX, mouseY);
            } 
            else if (!middleMouseDown && prevMiddleMouseDown)
            {
                OnMouseUp?.Invoke(MouseButton.Middle, mouseX, mouseY);
            }
        }

        public bool IsKeyReleased(Keys key)
        {
            return _releasedKeys.Contains(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return _releasedKeys.Contains(key);
        }
        public bool IsKeyPressed(Keys key)
        {
            return _releasedKeys.Contains(key);
        }
        public int MouseX()
        {
            return mouseX;
        }
        public int MouseY()
        {
            return mouseY;
        }
        public bool IsLeftMouseDown()
        {
            return leftMouseDown;
        }
        public bool IsRightMouseDown()
        {
            return rightMouseDown;
        }
        public bool IsMiddleMouseDown()
        {
            return middleMouseDown;
        }
    }
}
