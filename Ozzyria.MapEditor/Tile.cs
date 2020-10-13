namespace Ozzyria.MapEditor
{
    enum TileType
    {
        None,
        Ground,
        Water,
        Fence,
        Road
    }

    enum TransitionType
    {
        None,
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        UpLeftDiagonal,
        UpRightDiagonal,
        DownLeftDiagonal,
        DownRightDiagonal,
    }

    enum PathDirection
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

    class Tile
    {
        public TileType Type { get; set; } = TileType.None;
        public TransitionType Transition { get; set; } = TransitionType.None;
        public PathDirection Direction { get; set; } = PathDirection.None;
    }
}
