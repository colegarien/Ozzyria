using Microsoft.Xna.Framework;

namespace Ozzyria.MonoGameClient.Rendering
{
    internal enum DrawableOffsetType
    {
        Root,
        Weapon,
        Armor,
        Mask,
        Hat
    }

    internal struct Drawable
    {
        public uint Resource { get; set; }
        public Rectangle Source { get; set; }
        public int Subspace { get; set; }
        public Vector2 Origin { get; set; }
        public DrawableOffsetType SkeletonOffset { get; set; }

        public float BaseAngle { get; set; }
        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }



        // TODO move these to a file
        public static Drawable PIXEL = new Drawable()
        {
            Source = new Rectangle(943, 56, 1, 1)
        };

        public static Drawable PLAYER_RIGHT_HIGH = new Drawable
        {
            Source = new Rectangle(0, 128, 32, 32),
            Subspace = 0,
            Origin = new Vector2(16, 32)
        };

        public static Drawable PLAYER_RIGHT = new Drawable
        {
            Source = new Rectangle(32, 128, 32, 32),
            Subspace = 0,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_DOWN_HIGH = new Drawable
        {
            Source = new Rectangle(0, 160, 32, 32),
            Subspace = 0,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_DOWN = new Drawable
        {
            Source = new Rectangle(32, 160, 32, 32),
            Subspace = 0,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_DOWN_LOW = new Drawable
        {
            Source = new Rectangle(64, 160, 32, 32),
            Subspace = 0,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_LEFT_HIGH = new Drawable
        {
            Source = new Rectangle(0, 192, 32, 32),
            Subspace = 3,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_LEFT = new Drawable
        {
            Source = new Rectangle(32, 192, 32, 32),
            Subspace = 3,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_UP_HIGH = new Drawable
        {
            Source = new Rectangle(0, 224, 32, 32),
            Subspace = 3,
            Origin = new Vector2(16, 32)
        };
        public static Drawable PLAYER_UP = new Drawable
        {
            Source = new Rectangle(32, 224, 32, 32),
            Subspace = 3,
            Origin = new Vector2(16, 32)
        };

        public static Drawable SLIME_LEFT_HIGH = new Drawable
        {
            Source = new Rectangle(71, 331, 18, 20),
            Subspace = 0,
            Origin = Vector2.Zero
        };
        public static Drawable SLIME_LEFT = new Drawable
        {
            Source = new Rectangle(7, 331, 18, 20),
            Subspace = 0,
            Origin = Vector2.Zero
        };

        public static Drawable WEAPON_TRAIL = new Drawable
        {
            Source = new Rectangle(914, 33, 11, 28),
            Subspace = 1,
            Origin = new Vector2(-5.5f, 28),
            SkeletonOffset = DrawableOffsetType.Weapon
        };
        public static Drawable WEAPON_DAGGER = new Drawable
        {
            Source = new Rectangle(32, 109, 21, 6),
            Subspace = 2,
            Origin = new Vector2(0, 3),
            SkeletonOffset = DrawableOffsetType.Weapon
        };
}
}
