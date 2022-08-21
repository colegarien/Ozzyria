using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Animation
{
    public struct FrameTransform
    {
        public int RelativeX { get; set; }
        public int RelativeY { get; set; }
        public int DestinationW { get; set; }
        public int DestinationH { get; set; }

        public bool RelativeRotation { get; set; }
        public float Rotation { get; set; }
        public float OriginOffsetX { get; set; }
        public float OriginOffsetY { get; set; }

        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Alpha { get; set; }
    }

    public struct FrameSource
    {
        public int Resource { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public struct Frame
    {
        public FrameTransform Transform { get; set; }
        public string SourceId { get; set; }
    }

}
