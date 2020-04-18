using Ozzyria.Game.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzyria.Game
{
    public class Entity
    {
        public int Id { get; set; } = -1;
        public Dictionary<ComponentType, Component.Component> Components { get; set; } = new Dictionary<ComponentType, Component.Component>();

        public void AttachComponent(Component.Component component)
        {
            component.Owner = this;
            Components[component.Type()] = component;
        }

        public bool HasComponent(ComponentType type)
        {
            return Components.ContainsKey(type);
        }
    }
}
