using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Stats : Grecs.Component, ISerializable, IHydrateable
    {
        private int _experience = 0;
        public int Experience
        {
            get => _experience; set
            {
                if (!_experience.Equals(value))
                {
                    _experience = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _maxExperience = 100;
        public int MaxExperience
        {
            get => _maxExperience; set
            {
                if (!_maxExperience.Equals(value))
                {
                    _maxExperience = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _health = 100;
        public int Health
        {
            get => _health; set
            {
                if (!_health.Equals(value))
                {
                    _health = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _maxHealth = 100;
        public int MaxHealth
        {
            get => _maxHealth; set
            {
                if (!_maxHealth.Equals(value))
                {
                    _maxHealth = value;
                    
                    TriggerChange();
                }
            }
        }
        public string GetComponentIdentifier() {
            return "Stats";
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Experience);
            w.Write(MaxExperience);
            w.Write(Health);
            w.Write(MaxHealth);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Experience = r.ReadInt32();
            MaxExperience = r.ReadInt32();
            Health = r.ReadInt32();
            MaxHealth = r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("experience"))
            {
                Experience = int.Parse(values["experience"]);
            }
            if (values.HasValueFor("maxExperience"))
            {
                MaxExperience = int.Parse(values["maxExperience"]);
            }
            if (values.HasValueFor("health"))
            {
                Health = int.Parse(values["health"]);
            }
            if (values.HasValueFor("maxHealth"))
            {
                MaxHealth = int.Parse(values["maxHealth"]);
            }
        }
    }
}
