using System.Collections.Generic;

namespace Ozzyria.Game.ECS
{
    public class SystemCoordinator
    {
        // TODO OZ-14 Add function and basic life-cycle stuff for hitting trigger and tick systems
        protected List<TickSystem> tickSystems = new List<TickSystem>();

        public SystemCoordinator Add(TickSystem system)
        {
            tickSystems.Add(system);
            return this;
        }

        public void Execute(EntityContext context)
        {
            foreach(var system in tickSystems)
            {
                system.Execute(context);
            }
        }
    }
}
