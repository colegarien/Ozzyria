using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class AttackIntent : Component
    {
        private int _frame = 0;
        private int _decayFrame = 3;
        private int _damageFrame = 1;

        private float _frameTimer = 0f;
        private float _timePerFrame = 100f; // 100ms per frame

        public int Frame
        {
            get => _frame; set
            {
                if (_frame != value)
                {
                    _frame = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public int DecayFrame
        {
            get => _decayFrame; set
            {
                if (_decayFrame != value)
                {
                    _decayFrame = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public int DamageFrame
        {
            get => _damageFrame; set
            {
                if (_damageFrame != value)
                {
                    _damageFrame = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public float FrameTimer
        {
            get => _frameTimer; set
            {
                if (_frameTimer != value)
                {
                    _frameTimer = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public float TimePerFrame
        {
            get => _timePerFrame; set
            {
                if (_timePerFrame != value)
                {
                    _timePerFrame = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

    }
}
