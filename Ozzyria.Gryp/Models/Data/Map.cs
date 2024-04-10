
namespace Ozzyria.Gryp.Models.Data
{
    internal class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public void PushLayer()
        {
            Layers.Add(new Layer {
                Width = Width,
                Height = Height,
                Tiles = new Tile[Width * Height]
            });
        }
    }

    internal class Layer
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Tile[] Tiles { get; set; }

        public Tile Get(int x, int y)
        {
            if(x < 0 || y < 0 || x >= Width || y >= Height)
                throw new Exception("outta bounds dawg");

            return Tiles[x + (y * Width)];
        }

        public void Set(int x, int y, Tile tile)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new Exception("outta bounds dawg");

            Tiles[x + (y * Width)] = tile;
        }
    }

    internal class Tile
    {
        public List<TextureCoords> Images { get; set; } = new List<TextureCoords>();
    }

    internal class TextureCoords
    {
        public int TextureX { get; set; }
        public int TextureY { get; set; }
    }
}
