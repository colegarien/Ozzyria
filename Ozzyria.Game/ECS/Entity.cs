﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game.ECS
{

    public delegate void ComponentEvent(Entity entity, IComponent component);

    public class Entity
    {
        protected IDictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();

        // TODO OZ-27 implement pooling have the context initialize/manage the pool?

        public event ComponentEvent OnComponentAdded;
        public event ComponentEvent OnComponentChanged;
        public event ComponentEvent OnComponentRemoved;

        public uint id;

        public void AddComponent(IComponent component)
        {
            _components[component.GetType()] = component;
            component.Owner = this;

            if (OnComponentAdded != null)
                OnComponentAdded(this, component);
        }

        public Entity RemoveComponent(IComponent component)
        {
            var type = component.GetType();
            if (HasComponent(type))
            {
                component.OnComponentChanged = null;
                _components.Remove(type);
            }

            if(OnComponentRemoved != null)
                OnComponentRemoved(this, component);

            return this;
        }

        public void RemoveAllComponents()
        {
            var c = _components.Values;
            foreach(var component in c)
                RemoveComponent(component);
        }

        public IComponent[] GetComponents()
        {
            return _components.Values.ToArray();
        }

        public bool HasComponent(Type type)
        {
            return _components.ContainsKey(type);
        }

        public IComponent GetComponent(Type type)
        {
            return HasComponent(type)
                ? _components[type]
                : null;
        }

        public IComponent CreateComponent<T>() where T : new()
        {
            IComponent c = (IComponent)new T();
            c.OnComponentChanged = OnComponentChanged;
            return c;
        }

        public IComponent CreateComponent(Type type)
        {
            IComponent c = (IComponent)Activator.CreateInstance(type);
            c.OnComponentChanged = OnComponentChanged;
            return c;
        }
    }
}
