using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public abstract class Thought : Component
    {
        public abstract void Update(float deltaTime, EntityContext context);
    }
}
