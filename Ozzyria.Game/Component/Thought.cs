using System.Collections.Generic;

namespace Ozzyria.Game.Component
{
    public abstract class Thought : Component
    {
        public override ComponentType Type() => ComponentType.Thought;
        public abstract void Update(float deltaTime, EntityManager entityManager);
    }
}
