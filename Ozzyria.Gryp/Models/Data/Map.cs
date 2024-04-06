
namespace Ozzyria.Gryp.Models.Data
{
    internal class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Layer> Layers {get; set;}

    }

    internal class Layer
    {
        public int[] Tiles { get; set; }
    }

}
