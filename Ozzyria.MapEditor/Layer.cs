namespace Ozzyria.MapEditor
{
    class Layer
    {
        private int width;
        private int height;
        private Tile[] tiles;

        public Layer(int width, int height)
        {
            this.width = width;
            this.height = height;
            tiles = new Tile[width * height];
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile();
            }
        }

        private Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return null;
            }

            return tiles[(y * width) + x];
        }

        public TileType GetTileType(int x, int y)
        {
            return GetTile(x, y)?.Type ?? TileType.None;
        }

        public void SetTileType(int x, int y, TileType type)
        {
            var tile = GetTile(x, y);
            if (tile != null)
                tile.Type = type;
        }

        public EdgeTransitionType GetEdgeTransitionType(int x, int y)
        {
            return GetTile(x, y)?.EdgeTransition ?? EdgeTransitionType.None;
        }

        public void SetEdgeTransitionType(int x, int y, EdgeTransitionType type)
        {
            var tile = GetTile(x, y);
            if (tile != null)
                tile.EdgeTransition = type;
        }

        public CornerTransitionType GetCornerTransitionType(int x, int y)
        {
            return GetTile(x, y)?.CornerTransition ?? CornerTransitionType.None;
        }

        public void SetCornerTransitionType(int x, int y, CornerTransitionType type)
        {
            var tile = GetTile(x, y);
            if (tile != null)
                tile.CornerTransition = type;
        }

        public PathDirection GetPathDirection(int x, int y)
        {
            return GetTile(x, y)?.Direction ?? PathDirection.None;
        }

        public void SetPathDirection(int x, int y, PathDirection direction)
        {
            var tile = GetTile(x, y);
            if (tile != null)
                tile.Direction = direction;
        }
    }
}
