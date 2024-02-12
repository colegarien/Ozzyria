
namespace Ozzyria.Game.Animation
{
    public enum DrawableAttachmentType
    {
        Root,
        Weapon,
        Armor,
        Mask,
        Hat
    }

    public struct Drawable
    {
        public uint Resource { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Subspace { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }
        public DrawableAttachmentType AttachmentType { get; set; }

        public float BaseAngle { get; set; }
        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }
    }
}
