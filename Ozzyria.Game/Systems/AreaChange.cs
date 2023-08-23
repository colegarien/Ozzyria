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
                var player = (Components.Player)entity.GetComponent(typeof(Components.Player));
                var areaChange = (Components.AreaChange)entity.GetComponent(typeof(Components.AreaChange));
                var movement = (Components.Movement)entity.GetComponent(typeof(Components.Movement));

                if (areaChange.NewArea != player.Map)
                {
                    // detach from current area
                    _world.WorldState.Areas[player.Map]._context.DetachEntity(entity);

                    // make component updates
                    player.Map = "test_b";
                    _world.WorldState.PlayerAreaTracker[player.PlayerId] = areaChange.NewArea;

                    movement.X = areaChange.NewX;
                    movement.Y = areaChange.NewY;
                    movement.PreviousX = areaChange.NewX;
                    movement.PreviousY = areaChange.NewY;

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
            // TODO OZ-27 move "map" off of player and into a Location component or something
            var query = new EntityQuery().And(typeof(Components.Player), typeof(Components.AreaChange));
            var listener = context.CreateListener(query);
            listener.ListenToAdded = true;

            return listener;
        }
    }
}
