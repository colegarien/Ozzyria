using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class SlimeThought : Grecs.Component, ISerializable, IHydrateable
    {
        private Delay _thinkDelay = new Delay{  };
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

        
        private int _thinkAction = 0;
        public int ThinkAction
        {
            get => _thinkAction; set
            {
                if (!_thinkAction.Equals(value))
                {
                    _thinkAction = value;
                    
                    TriggerChange();
                }
            }
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
            if (values.HasValueFor("thinkAction"))
            {
                ThinkAction = int.Parse(values["thinkAction"]);
            }
        }
    }
}
