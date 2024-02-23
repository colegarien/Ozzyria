using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Game.Utility;

namespace Ozzyria.Game.Systems
{
    internal class Death : TriggerSystem
    {
        public Death(EntityContext context) : base(context)
        {
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));

                EntityFactory.CreateExperienceOrb(context, movement.X, movement.Y, 10);
                if (entity.HasComponent(typeof(Components.Player)))
                {
                    // TODO OZ-30 : create Graveyard component and add a AssignedGraveyard to players, then just revive them there
                    // TODO OZ-30 : probably just remove thought component, change renderable to corpse then make a "respawn" system to handle player respawn at a graveyard

                    var playerId = ((Components.Player)entity.GetComponent(typeof(Components.Player))).PlayerId;

                    // reset player
                    context.DestroyEntity(entity);
                    EntityFactory.CreatePlayer(context, playerId);
                }
                else
                {
                    context.DestroyEntity(entity);
                }
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().And(typeof(Dead), typeof(Movement));
            return context.CreateListener(query);
        }
    }
}
