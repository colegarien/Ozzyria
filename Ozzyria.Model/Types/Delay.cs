namespace Ozzyria.Model.Types
{
    public class Delay : ISerializable, IHydrateable
    {
        private System.Action? _triggerChange;
        public System.Action? TriggerChange { get => _triggerChange; set
            {
                _triggerChange = value;
                
            }
        }
        
        
        private float _delayInSeconds = 0.5f;
        public float DelayInSeconds
        {
            get => _delayInSeconds; set
            {
                if (!_delayInSeconds.Equals(value))
                {
                    _delayInSeconds = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private float _timer = 0.5f;
        public float Timer
        {
            get => _timer; set
            {
                if (!_timer.Equals(value))
                {
                    _timer = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }

        
        private bool _ready = false;
        public bool Ready
        {
            get => _ready; set
            {
                if (!_ready.Equals(value))
                {
                    _ready = value;
                    
                    TriggerChange?.Invoke();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "Delay";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(DelayInSeconds);
            w.Write(Timer);
        }

        public void Read(System.IO.BinaryReader r)
        {
            DelayInSeconds = r.ReadSingle();
            Timer = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("delayInSeconds"))
            {
                DelayInSeconds = float.Parse(values["delayInSeconds"]);
            }
            if (values.HasValueFor("timer"))
            {
                Timer = float.Parse(values["timer"]);
            }
            if (values.HasValueFor("ready"))
            {
                Ready = bool.Parse(values["ready"]);
            }
        }
    }
}
