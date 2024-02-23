using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
    public class Player : Component
    {
        private int _playerId = -1;

        [Savable]
        public int PlayerId
        {
            get => _playerId; set
            {
                if (_playerId != value)
                {
                    _playerId = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
