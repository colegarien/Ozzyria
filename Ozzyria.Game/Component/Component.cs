using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Component
{
    public abstract class Component : IComponent
    {
        public Entity Owner { get; set; }
    }
}
