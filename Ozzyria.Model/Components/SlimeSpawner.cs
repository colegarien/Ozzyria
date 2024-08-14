using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class SlimeSpawner : Grecs.Component, ISerializable, IHydrateable
    {
        private float _x = 500f;
        public float X
        {
            get => _x; set
            {
                if (!_x.Equals(value))
                {
                    _x = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private float _y = 400f;
        public float Y
        {
            get => _y; set
            {
                if (!_y.Equals(value))
                {
                    _y = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _SLIME_LIMIT = 3;
        public int SLIME_LIMIT
        {
            get => _SLIME_LIMIT; set
            {
                if (!_SLIME_LIMIT.Equals(value))
                {
                    _SLIME_LIMIT = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private Delay _thinkDelay = new Delay{ DelayInSeconds = 5f,  };
        public Delay ThinkDelay
        {
            get => _thinkDelay; set
            {
                if (!_thinkDelay?.Equals(value) ?? (value != null))
                {
                    _thinkDelay = value;
                    if (value != null) { _thinkDelay.TriggerChange = TriggerChange; }
                    TriggerChange();
                }
            }
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(SLIME_LIMIT);
            w.Write(ThinkDelay.DelayInSeconds);
            w.Write(ThinkDelay.Timer);
        }

        public void Read(System.IO.BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            SLIME_LIMIT = r.ReadInt32();
            ThinkDelay.DelayInSeconds = r.ReadSingle();
            ThinkDelay.Timer = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("x"))
            {
                X = float.Parse(values["x"]);
            }
            if (values.HasValueFor("y"))
            {
                Y = float.Parse(values["y"]);
            }
            if (values.HasValueFor("SLIME_LIMIT"))
            {
                SLIME_LIMIT = int.Parse(values["SLIME_LIMIT"]);
            }
            if (values.HasValueFor("thinkDelay"))
            {
                var  values_thinkDelay = values.Extract("thinkDelay");
                if (values_thinkDelay.HasValueFor("delayInSeconds"))
            {
                ThinkDelay.DelayInSeconds = float.Parse(values_thinkDelay["delayInSeconds"]);
            }
                if (values_thinkDelay.HasValueFor("timer"))
            {
                ThinkDelay.Timer = float.Parse(values_thinkDelay["timer"]);
            }
                if (values_thinkDelay.HasValueFor("ready"))
            {
                ThinkDelay.Ready = bool.Parse(values_thinkDelay["ready"]);
            }
            }
        }
    }
}
