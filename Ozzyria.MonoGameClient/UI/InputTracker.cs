using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.MonoGameClient.UI
{
    internal class InputTracker
    {
        const float SCROLL_SENSITIVITY = 3333f;

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

        int prevVerticalMouseScroll = -1;
        int verticalMouseScroll = -1;

        int prevHorizontalMouseScroll = -1;
        int horizontalMouseScroll = -1;

        public enum MouseButton
        {
            Left,
            Right,
            Middle
        }
        public delegate void MouseButtonEvent(MouseButton button, int x, int y);
        public delegate void MouseMoveEvent(int previousX, int previousY, int x, int y);
        public delegate void MouseScrollEvent(int x, int y, float delta);
        public event MouseButtonEvent OnMouseDown;
        public event MouseMoveEvent OnMouseMove;
        public event MouseButtonEvent OnMouseUp;
        public event MouseScrollEvent OnMouseVerticalScroll;
        public event MouseScrollEvent OnMouseHorizontalScroll;

        public delegate void KeyboardEvent(InputTracker tracker);
        public event KeyboardEvent OnKeysPressed;
        public event KeyboardEvent OnKeysHeld;
        public event KeyboardEvent OnKeysReleased;


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
            // Pressed => Freshly downed Keys (is a sub-set of held Down keys)
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

            if( _pressedKeys.Count > 0 )
            {
                OnKeysPressed?.Invoke(this);
            }
            if (_downKeys.Count > 0)
            {
                OnKeysHeld?.Invoke(this);
            }
            if (_releasedKeys.Count > 0)
            {
                OnKeysReleased?.Invoke(this);
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


            if (prevMouseX != mouseX || prevMouseY != mouseY)
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


            if (IsKeyPressed(Keys.LeftShift))
            {
                // on initial press switch horizontal to track vertical mouse wheel values
                horizontalMouseScroll = verticalMouseScroll;
            }

            if (IsKeyDown(Keys.LeftShift))
            {
                // if shift is down, use vertical wheel to scroll horizontally
                prevHorizontalMouseScroll = horizontalMouseScroll;
                horizontalMouseScroll = mouseState.ScrollWheelValue;
            }
            else if (IsKeyReleased(Keys.LeftShift))
            {
                // switch values back to normal
                prevVerticalMouseScroll = mouseState.ScrollWheelValue;
                verticalMouseScroll = mouseState.ScrollWheelValue;
                prevHorizontalMouseScroll = mouseState.HorizontalScrollWheelValue;
                horizontalMouseScroll = mouseState.HorizontalScrollWheelValue;
            }
            else
            {
                // read from normal mouse-wheels
                prevVerticalMouseScroll = verticalMouseScroll;
                verticalMouseScroll = mouseState.ScrollWheelValue;
                prevHorizontalMouseScroll = horizontalMouseScroll;
                horizontalMouseScroll = mouseState.HorizontalScrollWheelValue;
            }
            if (prevVerticalMouseScroll != verticalMouseScroll)
            {
                OnMouseVerticalScroll?.Invoke(mouseX, mouseY, (verticalMouseScroll - prevVerticalMouseScroll) / SCROLL_SENSITIVITY);
            }
            if (prevHorizontalMouseScroll != horizontalMouseScroll)
            {
                OnMouseHorizontalScroll?.Invoke(mouseX, mouseY, (horizontalMouseScroll - prevHorizontalMouseScroll) / SCROLL_SENSITIVITY);
            }
        }

        public bool IsKeyReleased(Keys key)
        {
            return _releasedKeys.Contains(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return _downKeys.Contains(key);
        }
        public bool IsKeyPressed(Keys key)
        {
            return _pressedKeys.Contains(key);
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
