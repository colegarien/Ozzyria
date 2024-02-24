using Grecs;

namespace Ozzyria.Game.Components
{
    public class MovementIntent: PooledComponent<MovementIntent>
    {
        public bool _moveRight = false;
        public bool _moveLeft = false;
        public bool _moveDown = false;
        public bool _moveUp = false;

        public bool MoveRight
        {
            get => _moveRight; set
            {
                if (_moveRight != value)
                {
                    _moveRight = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public bool MoveLeft
        {
            get => _moveLeft; set
            {
                if (_moveLeft != value)
                {
                    _moveLeft = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public bool MoveDown
        {
            get => _moveDown; set
            {
                if (_moveDown != value)
                {
                    _moveDown = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public bool MoveUp
        {
            get => _moveUp; set
            {
                if (_moveUp != value)
                {
                    _moveUp = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
