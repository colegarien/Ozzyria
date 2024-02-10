using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public enum ClipType
    {
        Decay,
        Loop,
        Stall
    }

    public  class Animator : Component
    {
        private SkeletonPose _currentPose;
        private ClipType _type;

        private int _frame;
        private int _numberOfFrames;
        private float _frameTimer;
        private float _timePerFrame = 100f; // 100ms per frame

        public SkeletonPose CurrentPose
        {
            get => _currentPose; set
            {
                if (_currentPose != value)
                {
                    _currentPose = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public ClipType Type
        {
            get => _type; set
            {
                if (_type != value)
                {
                    _type = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

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

        public int NumberOfFrames
        {
            get => _numberOfFrames; set
            {
                if (_numberOfFrames != value)
                {
                    _numberOfFrames = value;
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

