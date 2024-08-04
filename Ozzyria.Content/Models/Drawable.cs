namespace Ozzyria.Content.Models
{
    public enum DrawableAttachmentType
    {
        Root,
        Weapon,
        Armor,
        Mask,
        Hat
    }

    public enum DrawableColorType
    {
        White,
        Yellow,
    }

    public struct Drawable
    {
        public uint Resource { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int RenderOffset { get; set; }
        public int Subspace { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }
        public DrawableAttachmentType AttachmentType { get; set; }
        public DrawableColorType ColorType { get; set; }

        public float BaseAngle { get; set; }
        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }
    }
}
