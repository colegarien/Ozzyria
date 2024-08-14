using Grecs;
using Ozzyria.Model.Types;

namespace Ozzyria.Game.Components
{
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
                    TriggerChange();
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
                    TriggerChange();
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
                    TriggerChange();
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
                    TriggerChange();
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
                    TriggerChange();
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
                    TriggerChange();
                }
            }
        }

    }
}

