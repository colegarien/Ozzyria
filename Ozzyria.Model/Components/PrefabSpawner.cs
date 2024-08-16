using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class PrefabSpawner : Grecs.Component, ISerializable, IHydrateable
    {
        private string _prefabId = "slime";
        public string PrefabId
        {
            get => _prefabId; set
            {
                if (!_prefabId?.Equals(value) ?? (value != null))
                {
                    _prefabId = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private Type _entityTag = typeof(SlimeThought);
        public Type EntityTag
        {
            get => _entityTag; set
            {
                if (!_entityTag.Equals(value))
                {
                    _entityTag = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _entityLimit = 3;
        public int EntityLimit
        {
            get => _entityLimit; set
            {
                if (!_entityLimit.Equals(value))
                {
                    _entityLimit = value;
                    
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
        public string GetComponentIdentifier() {
            return "prefab_spawner";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(PrefabId);
            w.Write(EntityTag.ToString());
            w.Write(EntityLimit);
            w.Write(ThinkDelay.DelayInSeconds);
            w.Write(ThinkDelay.Timer);
        }

        public void Read(System.IO.BinaryReader r)
        {
            PrefabId = r.ReadString();
            EntityTag = Type.GetType(r.ReadString()) ?? typeof(object);
            EntityLimit = r.ReadInt32();
            ThinkDelay.DelayInSeconds = r.ReadSingle();
            ThinkDelay.Timer = r.ReadSingle();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("prefabId"))
            {
                PrefabId = values["prefabId"].Trim('"');
            }
            if (values.HasValueFor("entityTag"))
            {
                EntityTag = Type.GetType(values["entityTag"]) ?? typeof(object);
            }
            if (values.HasValueFor("entityLimit"))
            {
                EntityLimit = int.Parse(values["entityLimit"]);
            }
            if (values.HasValueFor("thinkDelay"))
            {
                var  values_thinkDelay = values.Extract("thinkDelay");
                if (values_thinkDelay.HasValueFor("delayInSeconds"))
            {
                ThinkDelay.DelayInSeconds = float.Parse(values_thinkDelay["delayInSeconds"].Trim('f'));
            }
                if (values_thinkDelay.HasValueFor("timer"))
            {
                ThinkDelay.Timer = float.Parse(values_thinkDelay["timer"].Trim('f'));
            }
                if (values_thinkDelay.HasValueFor("ready"))
            {
                ThinkDelay.Ready = bool.Parse(values_thinkDelay["ready"]);
            }
            }
        }
    }
}
