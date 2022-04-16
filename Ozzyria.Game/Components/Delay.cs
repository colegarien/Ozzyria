using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "Delay")]
    public class Delay : Component
    {
        private float _delayInSeconds = 0.5f;
        private float _timer = 0.5f;
        private bool _ready = false;

        [Savable]
        public float DelayInSeconds { get => _delayInSeconds; set
            {
                if (_delayInSeconds != value)
                {
                    _delayInSeconds = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        [Savable]
        public float Timer { get => _timer; set
            {
                if (_timer != value)
                {
                    _timer = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        public bool Ready { get => _ready; set
            {
                if (_ready != value)
                {
                    _ready = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }
        public void Update(float deltaTime)
        {
            if (Ready)
            {
                return;
            }

            // accumulate time
            Timer += deltaTime;
            Ready = Timer >= DelayInSeconds;
        }

        public bool IsReady()
        {
            if (!Ready)
            {
                return false;
            }

            // Reset Delay
            Timer = 0;
            Ready = false;

            return true;
        }
    }
}
