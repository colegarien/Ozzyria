﻿using Grecs;
using Ozzyria.Game.Components.Attribute;

namespace Ozzyria.Game.Components
{
    public class AttackIntent: PooledComponent<AttackIntent>
    {
        private int _frame = 0;
        private int _decayFrame = 3;
        private int _damageFrame = 1;

        private float _frameTimer = 0f;
        private float _timePerFrame = 0.100f; // 100ms per frame

        [Savable]
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

        [Savable]
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

        [Savable]
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

        [Savable]
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

        [Savable]
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
