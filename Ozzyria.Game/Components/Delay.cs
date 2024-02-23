using Ozzyria.Game.Components.Attribute;
using Grecs;

namespace Ozzyria.Game.Components
{
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
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float Timer { get => _timer; set
            {
                if (_timer != value)
                {
                    _timer = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        public bool Ready { get => _ready; set
            {
                if (_ready != value)
                {
                    _ready = value;
                    Owner?.TriggerComponentChanged(this);
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
