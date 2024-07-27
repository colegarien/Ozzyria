using Grecs;

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


                // TODO OZ-22 consider if we want this or not, its cool for doors to be able to move them around the same area
                //if (areaChange.NewArea != location.Area)
                {
                    var entityLeaveEvent = new EntityLeaveAreaEvent
                    {
                        SourceArea = location.Area,
                        EntityId = location.Owner.id,
                        NewArea = areaChange.NewArea,
                    };

                    // detach from current area
                    _world.WorldState.Areas[location.Area]._context.DetachEntity(entity);

                    // update location
                    location.Area = "test_m";

                    // update player tracking if player
                    if (entity.HasComponent(typeof(Components.Player)))
                    {
                        var playerId = ((Components.Player)entity.GetComponent(typeof(Components.Player)))?.PlayerId ?? -1;
                        _world.WorldState.PlayerAreaTracker[playerId] = areaChange.NewArea;
                        entityLeaveEvent.PlayerId = playerId;
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
                    _world.WorldState.AreaEvents.Add(entityLeaveEvent);
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
