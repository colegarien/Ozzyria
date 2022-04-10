using System.Collections.Generic;

namespace Ozzyria.Game.ECS
{
    public class QueryListener // TODO OZ-14 right now only listeners for changes in entities that match the query, do I need more?
    {
        protected EntityQuery _query;

        protected List<Entity> _entities;

        public QueryListener(EntityQuery query)
        {
            _query = query;
            _entities = new List<Entity>();
        }

        public Entity[] Gather()
        {
            var result = _entities.ToArray();
            _entities.Clear();

            return result;
        }

        public void HandleEntityChangeEvent(Entity entity)
        {
            if (_query.Matches(entity))
            {
                if(!_entities.Contains(entity))
                    _entities.Add(entity);
            }
            else
            {
                if(_entities.Contains(entity))
                    _entities.Remove(entity);
            }
        }
    }
}
