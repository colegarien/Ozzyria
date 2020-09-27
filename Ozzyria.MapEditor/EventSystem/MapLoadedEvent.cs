namespace Ozzyria.MapEditor.EventSystem
{
    class MapLoadedEvent : IEvent
    {
        public int TileDimension { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumberOfLayers { get; set; }
    }
}
