using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;
using System.Collections.Generic;

namespace Ozzyria.Game.Components
{
    public class Bag : Component
    {
        private string _name = "Iventory";
        private int _capacity = 25;

        // Not synced to clients automatically with normal entity updates
        private List<Entity> _contents = new List<Entity>();

        [Savable]
        public string Name
        {
            get => _name; set
            {
                if (_name != value)
                {
                    _name = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        [Savable]
        public int Capacity
        {
            get => _capacity; set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public bool AddItem(Entity entity)
        {
            if(_contents.Count >= _capacity)
            {
                return false;
            }

            _contents.Add(entity);
            Owner?.TriggerComponentChanged(this);
            return true;
        }

        public List<Entity> Contents
        {
            get => _contents; set
            {
                if (_contents != value)
                {
                    _contents = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
    }
}
