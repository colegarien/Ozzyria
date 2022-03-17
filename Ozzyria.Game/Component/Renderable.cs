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
        public override ComponentType Type() => ComponentType.Renderable;

        [Savable]
        public SpriteType Sprite { get; set; } = SpriteType.Default;

        [Savable]
        public int Z { get; set; } = (int)ZLayer.Background;
    }
}
