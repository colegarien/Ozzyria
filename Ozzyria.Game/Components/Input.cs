using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Input : Component
    {
        private bool _moveUp = false;
        private bool _moveDown = false;
        private bool _moveLeft = false;
        private bool _moveRight = false;
        private bool _turnLeft = false;
        private bool _turnRight = false;
        private bool _attack = false;

        [Savable]
        public bool MoveUp { get => _moveUp; set
            {
                if (_moveUp != value)
                {
                    _moveUp = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool MoveDown { get => _moveDown; set
            {
                if (_moveDown != value)
                {
                    _moveDown = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool MoveLeft { get => _moveLeft; set
            {
                if (_moveLeft != value)
                {
                    _moveLeft = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool MoveRight { get => _moveRight; set
            {
                if (_moveRight != value)
                {
                    _moveRight = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool TurnLeft { get => _turnLeft; set
            {
                if (_turnLeft != value)
                {
                    _turnLeft = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool TurnRight { get => _turnRight; set
            {
                if (_turnRight != value)
                {
                    _turnRight = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool Attack { get => _attack; set
            {
                if (_attack != value)
                {
                    _attack = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
