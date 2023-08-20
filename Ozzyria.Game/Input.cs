using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game
{
    public class Input
    {
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }
        public bool TurnLeft { get; set; }
        public bool TurnRight { get; set; }
        public bool Attack { get; set; }
    }
}
