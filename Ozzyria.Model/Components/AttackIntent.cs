using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class AttackIntent : Grecs.PooledComponent<AttackIntent>, ISerializable, IHydrateable
    {
        private int _frame = 0;
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

        
        private int _decayFrame = 3;
        public int DecayFrame
        {
            get => _decayFrame; set
            {
                if (!_decayFrame.Equals(value))
                {
                    _decayFrame = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _damageFrame = 1;
        public int DamageFrame
        {
            get => _damageFrame; set
            {
                if (!_damageFrame.Equals(value))
                {
                    _damageFrame = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _frameTimer = 0f;
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

        
        private float _timePerFrame = 0.100f;
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
            return "attack_intent";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Frame);
            w.Write(DecayFrame);
            w.Write(DamageFrame);
            w.Write(FrameTimer);
            w.Write(TimePerFrame);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Frame = r.ReadInt32();
            DecayFrame = r.ReadInt32();
            DamageFrame = r.ReadInt32();
            FrameTimer = r.ReadSingle();
            TimePerFrame = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("frame"))
            {
                Frame = int.Parse(values["frame"]);
            }
            if (values.HasValueFor("decayFrame"))
            {
                DecayFrame = int.Parse(values["decayFrame"]);
            }
            if (values.HasValueFor("damageFrame"))
            {
                DamageFrame = int.Parse(values["damageFrame"]);
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
