namespace Ozzyria.MapEditor.EventSystem
{
    class BrushTypeChangeEvent : IEvent
    {
        public TileType SelectedBrush { get; set; }
    }
}
