using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    class SlimeSpawner : Component
    {
        private float _x = 500f;
        private float _y = 400f;

        public int SLIME_LIMIT { get; set; } = 3;

        [Savable]
        public float X { get => _x; set
            {
                if (_x != value)
                {
                    _x = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public float Y { get => _y; set
            {
                if (_y != value)
                {
                    _y = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public Delay ThinkDelay { get; set; } = new Delay
        {
            DelayInSeconds = 5f
        };
    }
}
