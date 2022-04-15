using Ozzyria.Game.Component.Attribute;

namespace Ozzyria.Game.Component
{
    [Options(Name = "Player")]
    public class Player : Component
    {
        [Savable]
        public int PlayerId { get; set; } = -1;
    }
}
