namespace Ozzyria.MapEditor.EventSystem
{
    class LayerChangedEvent : IEvent
    {
        public int SelectedLayer { get; set; }
    }
}
