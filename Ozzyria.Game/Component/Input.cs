﻿namespace Ozzyria.Game.Component
{
    public class Input : Component
    {
        public override ComponentType Type() => ComponentType.Input;

        public bool MoveUp { get; set; } = false;
        public bool MoveDown { get; set; } = false;
        public bool MoveLeft { get; set; } = false;
        public bool MoveRight { get; set; } = false;
        public bool TurnLeft { get; set; } = false;
        public bool TurnRight { get; set; } = false;
        public bool Attack { get; set; } = false;
    }
}