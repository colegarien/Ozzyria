using Ozzyria.Model.Types;

namespace Ozzyria.Model.Components
{
    public class Bag : Grecs.Component, ISerializable, IHydrateable
    {
        private string _container_id = "";
        public string ContainerId
        {
            get => _container_id; set
            {
                if (!_container_id?.Equals(value) ?? (value != null))
                {
                    _container_id = value;
                    
                    TriggerChange();
                }
            }
        }

        
        private string _name = "Inventory";
        public string Name
        {
            get => _name; set
            {
                if (!_name?.Equals(value) ?? (value != null))
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
            w.Write(ContainerId);
            w.Write(Name);
            w.Write(Capacity);
        }

        public void Read(System.IO.BinaryReader r)
        {
            ContainerId = r.ReadString();
            Name = r.ReadString();
            Capacity = r.ReadInt32();
        }
        public void Hydrate(ValuePacket values)
        {
            if (values == null || values.Count <= 0)
            {
                return;
            }

            if (values.HasValueFor("container_id"))
            {
                ContainerId = values["container_id"].Trim('"');
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
