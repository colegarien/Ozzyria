using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{

    public enum SpriteType  // OZ-23 : refactor this to not be an enum (make more data-driven)
    {
        Default = 1,
        Player = 2,
        Slime = 3,
        Particle = 4,
    }

    [Options(Name = "Renderable")]
    public class Renderable : Component
    {
        // TODO find a better way, maybe once entity prefabs are a thing build this into the tooling?
        public const int Z_BACKGROUND = 0;
        public const int Z_ITEMS = 10;
        public const int Z_MIDDLEGROUND = 25;
        public const int Z_FOREGROUND = 50;
        public const int Z_INGAME_UI = 255;
        public const int Z_DEBUG = 99999;

        public override ComponentType Type() => ComponentType.Renderable;

        [Savable]
        public SpriteType Sprite { get; set; } = SpriteType.Default;

        [Savable]
        public int Z { get; set; } = Z_BACKGROUND;
    }
}
