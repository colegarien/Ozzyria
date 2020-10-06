namespace Ozzyria.MapEditor.EventSystem
{
    class ToolTypeChangeEvent : IEvent
    {
        public ToolType SelectedTool { get; set; }
    }
}
