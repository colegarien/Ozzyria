namespace Ozzyria.MapEditor
{
    class InputState
    {
        public bool IsCtrlHeld { get; set; }
        public bool IsAltHeld { get; set; }
        public bool IsShiftHeld { get; set; }

        public bool LeftMouseDown { get; set; }
        public int LeftDownStartX { get; set; }
        public int LeftDownStartY { get; set; }

        public bool MiddleMouseDown { get; set; }
        public int MiddleDownStartX { get; set; }
        public int MiddleDownStartY { get; set; }
    }
}
