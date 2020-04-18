using System.Collections.Generic;

namespace Ozzyria.Game.Component
{
    public abstract class IThought : Component
    {
        public override ComponentType Type() => ComponentType.Thought;
        public abstract void Update(float deltaTime, Player[] players, Dictionary<int, Entity> entities);
    }
}
