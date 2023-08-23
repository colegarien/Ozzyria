using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Systems
{
    internal class AreaChange : TriggerSystem
    {
        private World _world;

        public AreaChange(World world, EntityContext context) : base(context)
        {
            _world = world;
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach(var entity in entities)
            {
                var location = (Components.Location)entity.GetComponent(typeof(Components.Location));
                var areaChange = (Components.AreaChange)entity.GetComponent(typeof(Components.AreaChange));

                if (areaChange.NewArea != location.Area)
                {
                    // detach from current area
                    _world.WorldState.Areas[location.Area]._context.DetachEntity(entity);

                    // update location
                    location.Area = "test_b";

                    // update player tracking if player
                    if (entity.HasComponent(typeof(Components.Player)))
                    {
                        var playerId = ((Components.Player)entity.GetComponent(typeof(Components.Player)))?.PlayerId ?? -1;
                        _world.WorldState.PlayerAreaTracker[playerId] = areaChange.NewArea;
                    }

                    // update movement if has component
                    if (entity.HasComponent(typeof(Components.Movement)))
                    {
                        var movement = (Components.Movement)entity.GetComponent(typeof(Components.Movement));
                        movement.X = areaChange.NewX;
                        movement.Y = areaChange.NewY;
                        movement.PreviousX = areaChange.NewX;
                        movement.PreviousY = areaChange.NewY;
                    }

                    // reattach to new area
                    _world.WorldState.Areas[areaChange.NewArea]._context.AttachEntity(entity);

                    // TODO OZ-27 trigger the AreaChange networking packet
                }

                entity.RemoveComponent(areaChange);
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Components.Location), typeof(Components.AreaChange));
            var listener = context.CreateListener(query);
            listener.ListenToAdded = true;

            return listener;
        }
    }
}
