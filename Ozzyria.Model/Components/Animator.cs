using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Animator : Grecs.Component, ISerializable, IHydrateable
    {
        private SkeletonPose _currentPose;
        public SkeletonPose CurrentPose
        {
            get => _currentPose; set
            {
                if (!_currentPose.Equals(value))
                {
                    _currentPose = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private ClipType _type;
        public ClipType Type
        {
            get => _type; set
            {
                if (!_type.Equals(value))
                {
                    _type = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _frame;
        public int Frame
        {
            get => _frame; set
            {
                if (!_frame.Equals(value))
                {
                    _frame = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _numberOfFrames;
        public int NumberOfFrames
        {
            get => _numberOfFrames; set
            {
                if (!_numberOfFrames.Equals(value))
                {
                    _numberOfFrames = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _frameTimer;
        public float FrameTimer
        {
            get => _frameTimer; set
            {
                if (!_frameTimer.Equals(value))
                {
                    _frameTimer = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _timePerFrame = 100f;
        public float TimePerFrame
        {
            get => _timePerFrame; set
            {
                if (!_timePerFrame.Equals(value))
                {
                    _timePerFrame = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "animator";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            
        }

        public void Read(System.IO.BinaryReader r)
        {
            
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("currentPose"))
            {
                CurrentPose = (SkeletonPose)Enum.Parse(typeof(SkeletonPose), values["currentPose"], true);
            }
            if (values.HasValueFor("type"))
            {
                Type = (ClipType)Enum.Parse(typeof(ClipType), values["type"], true);
            }
            if (values.HasValueFor("frame"))
            {
                Frame = int.Parse(values["frame"]);
            }
            if (values.HasValueFor("numberOfFrames"))
            {
                NumberOfFrames = int.Parse(values["numberOfFrames"]);
            }
            if (values.HasValueFor("frameTimer"))
            {
                FrameTimer = float.Parse(values["frameTimer"]);
            }
            if (values.HasValueFor("timePerFrame"))
            {
                TimePerFrame = float.Parse(values["timePerFrame"]);
            }
        }
    }
}
