using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class ExperienceBoost : Grecs.Component, ISerializable, IHydrateable
    {
        private int _experience = 10;
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

        
        private bool _hasBeenAbsorbed = false;
        public bool HasBeenAbsorbed
        {
            get => _hasBeenAbsorbed; set
            {
                if (!_hasBeenAbsorbed.Equals(value))
                {
                    _hasBeenAbsorbed = value;
                    
                    TriggerChange();
                }
            }
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Experience);
            w.Write(HasBeenAbsorbed);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Experience = r.ReadInt32();
            HasBeenAbsorbed = r.ReadBoolean();
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
            if (values.HasValueFor("hasBeenAbsorbed"))
            {
                HasBeenAbsorbed = bool.Parse(values["hasBeenAbsorbed"]);
            }
        }
    }
}
