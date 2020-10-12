using Ozzyria.MapEditor.EventSystem;
using SFML.Window;

namespace Ozzyria.MapEditor
{
    class InputState
    {
        public bool IsCtrlHeld { get; set; }
        public bool IsAltHeld { get; set; }
        public bool IsShiftHeld { get; set; }

        public bool LeftMouseDown { get; set; }
        public bool RightMouseDown { get; set; }
        public bool MiddleMouseDown { get; set; }
        public int DragStartX { get; set; }
        public int DragStartY { get; set; }

        public int PreviousMouseX { get; set; }
        public int PreviousMouseY { get; set; }
        public int CurrentMouseX { get; set; }
        public int CurrentMouseY { get; set; }

        public void HandleSfmlKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.LControl || e.Code == Keyboard.Key.RControl)
            {
                IsCtrlHeld = true;
            }
            else if (e.Code == Keyboard.Key.LAlt || e.Code == Keyboard.Key.RAlt)
            {
                IsAltHeld = true;
            }
            else if (e.Code == Keyboard.Key.LShift || e.Code == Keyboard.Key.RShift)
            {
                IsShiftHeld = true;
            }
        }

        public void HandleSfmlKeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.LControl || e.Code == Keyboard.Key.RControl)
            {
                IsCtrlHeld = false;
            }
            else if (e.Code == Keyboard.Key.LAlt || e.Code == Keyboard.Key.RAlt)
            {
                IsAltHeld = false;
            }
            else if (e.Code == Keyboard.Key.LShift || e.Code == Keyboard.Key.RShift)
            {
                IsShiftHeld = false;
            }

            if(e.Code == Keyboard.Key.S && IsCtrlHeld)
            {
                // TODO little ghetto... probably make a ShortCut handler or something
                MapManager.SaveMap();
            } else if(e.Code == Keyboard.Key.B && IsCtrlHeld)
            {
                // TODO little sad...
                MapManager.BakeMap();
            }
        }

        public void HandleSfmlMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (e.Wheel == Mouse.Wheel.HorizontalWheel || (IsAltHeld && e.Wheel == Mouse.Wheel.VerticalWheel))
            {
                EventQueue.Queue(new HorizontalScrollEvent
                {
                    OriginX = e.X,
                    OriginY = e.Y,
                    Delta = e.Delta
                });
            }
            else if (IsCtrlHeld)
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
        }

        public void HandleSfmlMousePressed(object sender, MouseButtonEventArgs e)
        {
            PreviousMouseX = CurrentMouseX;
            PreviousMouseY = CurrentMouseY;
            CurrentMouseX = e.X;
            CurrentMouseY = e.Y;
            EventQueue.Queue(new EventSystem.MouseMoveEvent()
            {
                DeltaX = CurrentMouseX - PreviousMouseX,
                DeltaY = CurrentMouseY - PreviousMouseY,
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
                MiddleMouseDown = true;
                DragStartX = e.X;
                DragStartY = e.Y;
            }
            else if (e.Button == Mouse.Button.Left)
            {
                LeftMouseDown = true;
                DragStartX = e.X;
                DragStartY = e.Y;
            }
            else if (e.Button == Mouse.Button.Right)
            {
                RightMouseDown = true;
                DragStartX = e.X;
                DragStartY = e.Y;
            }
        }

        public void HandleSfmlMouseReleased(object sender, MouseButtonEventArgs e)
        {
            PreviousMouseX = CurrentMouseX;
            PreviousMouseY = CurrentMouseY;
            CurrentMouseX = e.X;
            CurrentMouseY = e.Y;
            EventQueue.Queue(new EventSystem.MouseMoveEvent()
            {
                DeltaX = CurrentMouseX - PreviousMouseX,
                DeltaY = CurrentMouseY - PreviousMouseY,
                X = e.X,
                Y = e.Y,
            });

            if (e.Button == Mouse.Button.Middle)
            {
                MiddleMouseDown = false;
            }
            else if (e.Button == Mouse.Button.Left)
            {
                LeftMouseDown = false;
            }
            else if (e.Button == Mouse.Button.Right)
            {
                RightMouseDown = false;
            }
        }

        public void HandleSfmlMouseMoved(object sender, MouseMoveEventArgs e)
        {
            PreviousMouseX = CurrentMouseX;
            PreviousMouseY = CurrentMouseY;
            CurrentMouseX = e.X;
            CurrentMouseY = e.Y;

            EventQueue.Queue(new EventSystem.MouseMoveEvent()
            {
                DeltaX = CurrentMouseX - PreviousMouseX,
                DeltaY = CurrentMouseY - PreviousMouseY,
                X = e.X,
                Y = e.Y,
            });

            if (LeftMouseDown || RightMouseDown || MiddleMouseDown)
            {
                EventQueue.Queue(new MouseDragEvent()
                {
                    OriginX = DragStartX,
                    OriginY = DragStartY,
                    DeltaX = CurrentMouseX - PreviousMouseX,
                    DeltaY = CurrentMouseY - PreviousMouseY,
                    X = e.X,
                    Y = e.Y,
                    LeftMouseDown = LeftMouseDown,
                    RightMouseDown = RightMouseDown,
                    MiddleMouseDown = MiddleMouseDown,
                });
            }
        }
    }
}
