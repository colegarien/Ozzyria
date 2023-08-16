using System.Collections.Generic;

namespace Ozzyria.Game.ECS
{
    public enum QueryEventType
    {
        Added,
        Changed,
        Removed
    }

    public class QueryListener
    {
        public bool ListenToAdded { get; set; } = true;
        public bool ListenToChanged { get; set; } = false;
        public bool ListenToRemoved { get; set; } = false;

        protected EntityQuery _query;
        protected List<Entity> _entities;


        public QueryListener(EntityQuery query)
        {
            _query = query;
            _entities = new List<Entity>();
        }

        public void Detach(Entity entity)
        {
            _entities.Remove(entity);
        }

        public Entity[] Gather()
        {
            var result = _entities.ToArray();
            _entities.Clear();

            return result;
        }

        public void HandleEntityChangeEvent(QueryEventType type, Entity entity, IComponent component)
        {
            // Check if component change is even relevant
            if (!_query.Uses(component.GetType())
                || (type == QueryEventType.Added && !ListenToAdded)
                || (type == QueryEventType.Changed && !ListenToChanged))
            {
                return;
            }

            if (_query.Matches(entity))
            {
                if (!_entities.Contains(entity))
                    _entities.Add(entity);
            }
            else if (type == QueryEventType.Removed && ListenToRemoved && !_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
            else if (!ListenToRemoved && _entities.Contains(entity)) {
                _entities.Remove(entity);
            }
        }
    }
}
