using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class AnimationState : Component
    {
        private string _state = "idle";
        private string _direction = "south";

        [Savable]
        public string State
        {
            get => _state; set
            {
                if (_state != value)
                {
                    _state = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        [Savable]
        public string Direction
        {
            get => _direction; set
            {
                if (_direction != value)
                {
                    _direction = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
