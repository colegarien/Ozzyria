namespace Ozzyria.MapEditor
{
    class Layer
    {
        private int width;
        private int height;  
        private TileType[] tiles;

        public Layer(int width, int height)
        {
            this.width = width;
            this.height = height;
            tiles = new TileType[width * height];
        }

        public TileType GetTileType(int x, int y)
        {
            if(x < 0 || x >= width || y < 0 || y >= height)
            {
                return TileType.None;
            }

            return tiles[(y * width) + x];
        }

        public void SetTileType(int x, int y, TileType type)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            tiles[(y * width) + x] = type;
        }
    }
}
