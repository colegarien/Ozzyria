using Ozzyria.Game.Components.Attribute;

namespace Ozzyria.Game.Components
{
    [Options(Name = "BoundingCircle")]
    public class BoundingCircle : Collision
    {
        [Savable]
        public float Radius { get; set; } = 10f;
    }
}
