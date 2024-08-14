using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Bag : Grecs.Component, ISerializable, IHydrateable
    {
        private string _name = "Inventory";
        public string Name
        {
            get => _name; set
            {
                if (!_name.Equals(value))
                {
                    _name = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private int _capacity = 25;
        public int Capacity
        {
            get => _capacity; set
            {
                if (!_capacity.Equals(value))
                {
                    _capacity = value;
                    
                    TriggerChange();
                }
            }
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Name);
            w.Write(Capacity);
        }

        public void Read(System.IO.BinaryReader r)
        {
            Name = r.ReadString();
            Capacity = r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("name"))
            {
                Name = values["name"].Trim('"');
            }
            if (values.HasValueFor("capacity"))
            {
                Capacity = int.Parse(values["capacity"]);
            }
        }
    }
}
