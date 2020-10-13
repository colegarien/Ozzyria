namespace Ozzyria.MapEditor.EventSystem
{
    class MouseMoveEvent : IEvent
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
