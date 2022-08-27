using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class AnimationState : Component
    {
        private string _state = "idle";

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
    }
}
