using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "Player")]
    public class Player : Component
    {
        [Savable]
        public int PlayerId { get; set; } = -1;
    }
}
