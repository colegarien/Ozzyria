using Grecs;
using Ozzyria.Model.Components;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ozzyria.Game.Storage
{
    public class Container
    {
        public string Id { get; set; }
        public int Capacity { get; set; }
        public List<Entity> Contents { get; set; }

        public bool IsFull()
        {
            return Contents?.Count >= Capacity;
        }

        public bool IsEmpty()
        {
            return Contents?.Count <= 0;
        }
    }

    public class ContainerStorage
    {
        private ConcurrentDictionary<string, Container> _containers = new ConcurrentDictionary<string, Container>();

        private void ReserveContainerForBag(Bag bag)
        {
            if(bag.ContainerId == "")
            {
                var newContainerId = System.Guid.NewGuid().ToString();
                while (_containers.ContainsKey(newContainerId))
                    newContainerId = System.Guid.NewGuid().ToString();
                bag.ContainerId = newContainerId;
            }

            if (!_containers.ContainsKey(bag.ContainerId))
            {
                _containers[bag.ContainerId] = new Container
                {
                    Id = bag.ContainerId,
                    Capacity = bag.Capacity,
                    Contents = new List<Entity>()
                };
            }

            if (_containers[bag.ContainerId].Capacity != bag.Capacity)
            {
                _containers[bag.ContainerId].Capacity = bag.Capacity;
            }
        }

        public Entity GetItemFromBag(Bag bag, int slot)
        {
            var containerId = bag.ContainerId;
            if (containerId == "" || slot < 0 || !_containers.ContainsKey(containerId) || _containers[containerId].Contents.Count <= slot)
            {
                return null;
            }

            return _containers[containerId].Contents[slot];
        }

        public bool AddItemToBag(Bag bag, Entity entity)
        {
            ReserveContainerForBag(bag);
            var containerId = bag.ContainerId;
            if (containerId == "" || !_containers.ContainsKey(containerId) || _containers[containerId].IsFull() || !entity.HasComponent(typeof(Item)))
            {
                return false;
            }

            var item = entity.GetComponent(typeof(Item)) as Item;
            item.Slot = _containers[containerId].Contents.Count;
            _containers[containerId].Contents.Add(entity);

            // trigger bag change event (this initiates syncs to client)
            bag.Owner?.TriggerComponentChanged(bag);
            return true;
        }

        public Entity RemoveItemFromBag(Bag bag, int slot)
        {
            var containerId = bag.ContainerId;
            if (containerId == "" || slot < 0 || !_containers.ContainsKey(containerId) || _containers[containerId].Contents.Count <= slot)
            {
                return null;
            }

            var entity = _containers[containerId].Contents[slot];
            _containers[containerId].Contents.RemoveAt(slot);

            if (slot < _containers[containerId].Contents.Count)
            {
                // adjust slot numbers since items are shifted
                for (var i = slot; i < _containers[containerId].Contents.Count; i++)
                {
                    ((Item)_containers[containerId].Contents[i].GetComponent(typeof(Item))).Slot = i;
                }
            }

            // trigger bag change event (this initiates syncs to client)
            bag.Owner?.TriggerComponentChanged(bag);
            return entity;
        }

        public List<Entity> GetBagContents(Bag bag)
        {
            var containerId = bag.ContainerId;
            if (containerId == "" || !_containers.ContainsKey(containerId) || _containers[containerId].IsEmpty())
            {
                return new List<Entity>();
            }

            return _containers[containerId].Contents;
        }

        public void ReplaceBagContents(Bag bag, List<Entity> entities)
        {
            ReserveContainerForBag(bag);
            var containerId = bag.ContainerId;
            if (containerId == "" || !_containers.ContainsKey(containerId))
            {
                return;
            }

            _containers[containerId].Contents = entities;
            // trigger bag change event (this initiates syncs to client)
            bag.Owner?.TriggerComponentChanged(bag);
        }
    }
}
