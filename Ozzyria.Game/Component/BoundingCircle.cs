using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{
    [Options(Name = "BoundingCircle")]
    public class BoundingCircle : Collision
    {
        [Savable]
        public float Radius { get; set; } = 10f;
    }
}
