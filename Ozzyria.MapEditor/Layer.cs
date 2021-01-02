using System.Collections.Generic;

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

        public IDictionary<TileType, EdgeTransitionType> GetEdgeTransitions(int x, int y)
        {
            return GetTile(x, y)?.EdgeTransition ?? new Dictionary<TileType, EdgeTransitionType>();
        }

        public void SetEdgeTransitions(int x, int y, IDictionary<TileType, EdgeTransitionType> transitions)
        {
            var tile = GetTile(x, y);
            if (tile != null)
                tile.EdgeTransition = transitions;
        }

        public IDictionary<TileType, CornerTransitionType> GetCornerTransitions(int x, int y)
        {
            return GetTile(x, y)?.CornerTransition ?? new Dictionary<TileType, CornerTransitionType>(); ;
        }

        public void SetCornerTransitions(int x, int y, IDictionary<TileType, CornerTransitionType> transitions)
        {
            var tile = GetTile(x, y);
            if (tile != null)
                tile.CornerTransition = transitions;
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
