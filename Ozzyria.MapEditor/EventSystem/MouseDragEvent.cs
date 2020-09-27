namespace Ozzyria.MapEditor.EventSystem
{
    class MouseDragEvent : WindowSpecificEvent
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool LeftMouseDown { get; set; }
        public bool RightMouseDown { get; set; }
        public bool MiddleMouseDown { get; set; }
    }
}
