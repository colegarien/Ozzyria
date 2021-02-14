namespace Ozzyria.MapEditor.EventSystem
{
    class MapChangeEvent : IEvent
    {
        public string MapName { get; set; }
        public bool SaveCurrentlyLoadedMap { get; set; } = false;
        public bool IsNewMap { get; set; } = false;
        public string NewMapTileSet { get; set; } = "";
        public int NewMapWidth { get; set; } = -1;
        public int NewMapHeight { get; set; } = -1;
    }
}
