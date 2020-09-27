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
    }
}
