using Ozzyria.Game.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzyria.Game
{
    public class Entity
    {
        public int Id { get; set; } = -1;
        public Dictionary<Type, IComponent> Components { get; set; } = new Dictionary<Type, IComponent>();

        public void AttachComponent(IComponent component)
        {
            Components[component.GetType()] = component;
        }

        public bool HasComponent(Type type)
        {
            return Components.ContainsKey(type);
        }
    }
}
