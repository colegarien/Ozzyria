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

    enum EdgeTransitionType
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

    enum CornerTransitionType
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
        public EdgeTransitionType EdgeTransition { get; set; } = EdgeTransitionType.None;
        public CornerTransitionType CornerTransition { get; set; } = CornerTransitionType.None;
        public PathDirection Direction { get; set; } = PathDirection.None;
    }
}
