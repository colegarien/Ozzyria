namespace Ozzyria.Game.Component
{
    public enum SpriteType
    {
        Default = 1,
        Player = 2,
        Slime = 3,
        Particle = 4,
    }

    public class Renderable : Component
    {
        public override ComponentType Type() => ComponentType.Renderable;

        public SpriteType Sprite { get; set; } = SpriteType.Default;
    }
}
