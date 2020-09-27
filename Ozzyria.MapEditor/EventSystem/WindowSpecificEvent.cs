namespace Ozzyria.MapEditor.EventSystem
{
    abstract class WindowSpecificEvent : IEvent
    {
        public int OriginX { get; set; }
        public int OriginY { get; set; }
    }
}
