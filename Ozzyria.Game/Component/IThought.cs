using System.Collections.Generic;

namespace Ozzyria.Game.Component
{
    public abstract class IThought : IComponent
    {
        public abstract void Update(float deltaTime, Player[] players, Dictionary<int, Entity> entities);
    }
}
