using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class EntityManager
    {
        private int maxEntityId = 1000; // TODO think again... this is to account for player entities
        private Dictionary<int, Entity> entities;

        public EntityManager()
        {
            entities = new Dictionary<int, Entity>();
        }

        public Entity[] GetEntities()
        {
            return entities.Values.ToArray();
        }

        public Entity GetEntity(int id)
        {
            return entities[id];
        }

        public int Register(Entity entity)
        {
            if(entity.Id < 0)
            {
                entity.Id = maxEntityId++;
            }

            entities[entity.Id] = entity;
            return entity.Id;
        }

        public void DeRegister(int id)
        {
            if (entities.ContainsKey(id))
            {
                entities.Remove(id);
            }
        }
    }
}
