namespace Ozzyria.Gryp.Models.Form
{
    /// <summary>
    /// Minimal Settings required for the basic new map creation/initialization
    /// </summary>
    internal class NewMapSettings
    {
        public string Id { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public int Width { get; set; } = 1;
        public int Height { get; set; } = 1;
    }
}
