namespace Ozzyria.MapEditor.EventSystem
{
    class MouseDownEvent : WindowSpecificEvent
    {
        public bool LeftMouseDown { get; set; }
        public bool RightMouseDown { get; set; }
        public bool MiddleMouseDown { get; set; }
    }
}
