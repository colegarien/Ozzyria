namespace Ozzyria.Gryp.Models.Data
{
    internal class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public void PushLayer()
        {
            Layers.Add(new Layer(new LayerBoundary
            {
                TileWidth = Width,
                TileHeight = Height
            }));
        }
    }

}
