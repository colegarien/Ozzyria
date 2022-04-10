using System.Collections.Generic;

namespace Ozzyria.Game.ECS
{
    public class SystemCoordinator
    {
        protected List<TickSystem> tickSystems = new List<TickSystem>();
        protected List<TriggerSystem> triggerSystems = new List<TriggerSystem>();

        public SystemCoordinator Add(TickSystem system)
        {
            tickSystems.Add(system);
            return this;
        }

        public SystemCoordinator Add(TriggerSystem system)
        {
            triggerSystems.Add(system);
            return this;
        }

        public void Execute(EntityContext context)
        {
            foreach(var system in tickSystems)
            {
                system.Execute(context);
            }

            foreach (var system in triggerSystems)
            {
                system.Execute(context);
            }
        }
    }
}
