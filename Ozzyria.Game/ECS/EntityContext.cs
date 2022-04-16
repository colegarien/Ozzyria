﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game.ECS
{
    public class EntityContext
    {
        protected IDictionary<uint, Entity> entities = new Dictionary<uint, Entity>();
        protected IDictionary<Type, List<uint>> entityComponents = new Dictionary<Type, List<uint>>();
        protected List<uint> recentlyRemoved = new List<uint>();

        protected List<QueryListener> _queryListeners = new List<QueryListener>();

        protected readonly ComponentEvent OnComponentAdded;
        protected readonly ComponentEvent OnComponentChanged;
        protected readonly ComponentEvent OnComponentRemoved;

        public EntityContext()
        {
            OnComponentAdded = HandleOnComponentAdded;
            OnComponentChanged = HandleOnComponentChanged;
            OnComponentRemoved = HandleOnComponentRemoved;
        }

        public Entity[] GetEntities()
        {
            return entities.Values.ToArray();
        }

        public Entity[] GetEntities(EntityQuery query)
        {
            if (query.IsEmpty())
                return Array.Empty<Entity>();

            bool applyAnd = query.ands.Count > 0;
            var andEntityIds = entities.Keys.ToList();
            foreach (var andType in query.ands)
            {
                var entityIdsWithType = entityComponents.ContainsKey(andType)
                        ? entityComponents[andType]
                        : new List<uint>();
                andEntityIds = andEntityIds.Intersect(entityIdsWithType).ToList();

                if (andEntityIds.Count == 0)
                    break;
            }

            bool applyOr = query.ors.Count > 0;
            var orEntityIds = new List<uint>();
            foreach (var orType in query.ors)
            {
                if (entityComponents.ContainsKey(orType))
                    orEntityIds.AddRange(entityComponents[orType]);
            }

            bool applyNone = query.nones.Count > 0;
            var noneEntityIds = new List<uint>();
            foreach(var noneType in query.nones)
            {
                if (entityComponents.ContainsKey(noneType))
                    noneEntityIds.AddRange(entityComponents[noneType]);
            }

            return entities
                .Where(kv => (!applyAnd || andEntityIds.Contains(kv.Key)) &&
                    (!applyOr || orEntityIds.Contains(kv.Key)) &&
                    (!applyNone || !noneEntityIds.Contains(kv.Key))
                ).Select(kv => kv.Value)
                .ToArray();
        }

        public Entity CreateEntity(uint? id = null)
        {
            uint newId = id == null
                ? (entities.Keys.Count > 0 ? entities.Keys.Max() : 0) + 1
                : (uint)id;

            if(entities.ContainsKey(newId))
                return entities[newId];

            entities[newId] = new Entity();
            entities[newId].id = newId;
            entities[newId].OnComponentAdded += OnComponentAdded;
            entities[newId].OnComponentChanged += OnComponentChanged;
            entities[newId].OnComponentRemoved += OnComponentRemoved;

            return entities[newId];
        }

        public void DestroyEntity(uint id)
        {
            if (entities.ContainsKey(id))
            {
                entities[id].RemoveAllComponents();
                entities.Remove(id);
            }
        }

        public void DestroyEntity(Entity entity)
        {
            recentlyRemoved.Add(entity.id);

            entity.RemoveAllComponents();
            entities.Remove(entity.id);
        }

        public uint[] GetRecentlyDestroyed()
        {
            var ids = recentlyRemoved.ToArray();
            recentlyRemoved.Clear();
            return ids;
        }

        public QueryListener CreateListener(EntityQuery query)
        {
            var listener = new QueryListener(query);
            _queryListeners.Add(listener);
            return listener;
        }

        public void HandleOnComponentAdded(Entity entity, IComponent component)
        {
            if(!entityComponents.ContainsKey(component.GetType()))
                entityComponents[component.GetType()] = new List<uint>();
            entityComponents[component.GetType()].Add(entity.id);

            UpdateListeners(QueryEventType.Added, entity, component);
        }

        public void HandleOnComponentChanged(Entity entity, IComponent component)
        {
            UpdateListeners(QueryEventType.Changed, entity, component);
        }

        public void HandleOnComponentRemoved(Entity entity, IComponent component)
        {
            if(entityComponents.ContainsKey(component.GetType()))
                entityComponents[component.GetType()].Remove(entity.id);

            UpdateListeners(QueryEventType.Removed, entity, component);
        }

        protected void UpdateListeners(QueryEventType type, Entity entity, IComponent component)
        {
            foreach (var listener in _queryListeners)
                listener.HandleEntityChangeEvent(type, entity, component);
        }
    }
}
