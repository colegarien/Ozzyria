using System.Collections.Generic;

namespace Ozzyria.Game
{
    public enum ZLayer
    {
        Background = 0,
        Items = 10,
        Middleground = 25,
        Foreground = 50,
        InGameUi = 255,
        Debug = 99999,
    }

    public enum EdgeTransitionType
    {
        None = 0,
        Left = 1,
        Up = 2,
        UpLeft = 3,
        Right = 4,
        LeftRight = 5,
        UpRight = 6,
        UpLeftRight = 7,
        Down = 8,
        DownLeft = 9,
        UpDown = 10,
        UpDownLeft = 11,
        DownRight = 12,
        DownLeftRight = 13,
        UpDownRight = 14,
        UpDownLeftRight = 15,
    }

    public enum CornerTransitionType
    {
        None = 0,
        UpLeft = 1,
        UpRight = 2,
        UpLeftUpRight = 3,
        DownRight = 4,
        UpLeftDownRight = 5,
        UpRightDownRight = 6,
        UpLeftUpRightDownRight = 7,
        DownLeft = 8,
        UpLeftDownLeft = 9,
        UpRightDownLeft = 10,
        UpLeftUpRightDownLeft = 11,
        DownLeftDownRight = 12,
        UpLeftDownLeftDownRight = 13,
        UpRightDownLeftDownRight = 14,
        UpLeftUpRightDownLeftDownRight = 15,
    }

    public enum PathDirection
    {
        None,
        All,
        Up,
        Down,
        Left,
        Right,
        UpDown,
        LeftRight,
        UpLeft,
        UpRight,
        DownRight,
        DownLeft,
        UpT,
        DownT,
        LeftT,
        RightT,
    }

    public class Tile
    {
        public const int DIMENSION = 32;
        public const int HALF_DIMENSION = 16;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;
        public int TextureCoordX { get; set; } = 0;
        public int TextureCoordY { get; set; } = 0;
        public TileDecal[] Decals { get; set; }


        public int Type { get; set; } = 0;
        public IDictionary<int, EdgeTransitionType> EdgeTransition { get; set; } = new Dictionary<int, EdgeTransitionType>();
        public IDictionary<int, CornerTransitionType> CornerTransition { get; set; } = new Dictionary<int, CornerTransitionType>();
        public PathDirection Direction { get; set; } = PathDirection.None;
    }

    public class TileDecal
    {
        public int TextureCoordX { get; set; } = 0;
        public int TextureCoordY { get; set; } = 0;
    }

    public class TileMap
    {
        public string Name { get; set; } = "";
        public string TileSet { get; set; } = "";
        public uint Resource { get; set; } = 1;
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;

        public IDictionary<int, List<Tile>> Layers { get; set; } = new Dictionary<int, List<Tile>>();

        public bool HasLayer(int layer)
        {
            return Layers.ContainsKey(layer);
        }

        public IEnumerable<Tile> GetTiles(int layer)
        {
            if (!HasLayer(layer))
            {
                return System.Array.Empty<Tile>();
            }

            return Layers[layer];
        }
    }
}
