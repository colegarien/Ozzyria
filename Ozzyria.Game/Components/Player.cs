using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Player : Component
    {
        private int _playerId = -1;
        private string _map = "";

        [Savable]
        public int PlayerId
        {
            get => _playerId; set
            {
                if (_playerId != value)
                {
                    _playerId = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        [Savable]
        public string Map { get => _map; set
            {
                if (_map != value)
                {
                    _map = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
    }
}
