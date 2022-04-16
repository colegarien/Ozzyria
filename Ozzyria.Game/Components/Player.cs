using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "Player")]
    public class Player : Component
    {
        private int _playerId = -1;

        [Savable]
        public int PlayerId { get => _playerId; set
            {
                if (_playerId != value)
                {
                    _playerId = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
