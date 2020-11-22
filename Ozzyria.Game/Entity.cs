using Ozzyria.Game.Component;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game
{
    public class Entity
    {
        public int Id { get; set; } = -1;
        private Dictionary<ComponentType, Component.Component> Components { get; set; } = new Dictionary<ComponentType, Component.Component>();

        public void AttachComponent(Component.Component component)
        {
            if (component == null)
                return;

            component.Owner = this;
            Components[component.Type()] = component;
        }

        public bool HasComponent(ComponentType type)
        {
            return Components.ContainsKey(type);
        }

        public T GetComponent<T>(ComponentType type) where T : Component.Component
        {
            if (HasComponent(type))
            {
                return (T)Components[type];
            }

            return null;
        }

        public Component.Component[] GetAllComponents()
        {
            return Components.Values.ToArray();
        }
    }
}
