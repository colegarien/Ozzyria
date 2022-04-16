using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "Input")]
    public class Input : Component
    {

        [Savable]
        public bool MoveUp { get; set; } = false;
        [Savable]
        public bool MoveDown { get; set; } = false;
        [Savable]
        public bool MoveLeft { get; set; } = false;
        [Savable]
        public bool MoveRight { get; set; } = false;
        [Savable]
        public bool TurnLeft { get; set; } = false;
        [Savable]
        public bool TurnRight { get; set; } = false;
        [Savable]
        public bool Attack { get; set; } = false;
    }
}
