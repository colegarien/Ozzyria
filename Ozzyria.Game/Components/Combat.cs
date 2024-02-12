using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Combat : Component
    {
        // State Flags
        private bool _wantsToAttack = false;
        private bool _startedAttack = false;
        private bool _attacking = false;

        // Attack Progress
        private int _frame = 0;
        private int _decayFrame = 3;
        private int _damageFrame = 1;

        // Attack Timing
        private float _frameTimer = 0f;
        private float _timePerFrame = 0.100f; // 100ms per frame

        [Savable]
        public bool WantToAttack
        {
            get => _wantsToAttack; set
            {
                if (_wantsToAttack != value)
                {
                    _wantsToAttack = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool StartedAttack
        {
            get => _startedAttack; set
            {
                if (_startedAttack != value)
                {
                    _startedAttack = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public bool Attacking { get => _attacking; set
            {
                if (_attacking != value)
                {
                    _attacking = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
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





        public float AttackAngle { get; set; } = 0.78f; // forty-five degrees-ish
        public float AttackRange { get; set; } = 21f;
        public int AttackDamage { get; set; } = 5;
    }
}
