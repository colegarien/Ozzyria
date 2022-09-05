using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Systems
{
    internal class AnimationStateSync : TriggerSystem
    {
        public AnimationStateSync(EntityContext context) : base(context)
        {
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var state = (AnimationState)entity.GetComponent(typeof(AnimationState));

                if (entity.HasComponent(typeof(Movement)))
                {
                    var movement = (Movement)entity.GetComponent(typeof(Movement));
                    if (movement.LookDirection <= 0.78 || movement.LookDirection >= 5.48)
                    {
                        state.SetVariable("Direction", "south");
                    }
                    else if (movement.LookDirection > 0.78 && movement.LookDirection < 2.36)
                    {
                        state.SetVariable("Direction", "east");
                    }
                    else if (movement.LookDirection >= 2.36 && movement.LookDirection <= 3.94)
                    {
                        state.SetVariable("Direction", "north");
                    }
                    else
                    {
                        state.SetVariable("Direction", "west");
                    }
                }
                else
                {
                    // Direction default is east
                    state.SetVariable("Direction", "east");
                }

                if (entity.HasComponent(typeof(Components.Combat)))
                {
                    var combat = (Components.Combat)entity.GetComponent(typeof(Components.Combat));
                    state.SetVariable("WantsToAttack", combat.WantToAttack ? "true" : "false");
                    state.SetVariable("IsAttacking", combat.Attacking ? "true" : "false");
                }
                else
                {
                    // IsAttacking default is false
                    state.SetVariable("WantsToAttack", "false");
                    state.SetVariable("IsAttacking", "false");
                }
            }
        }

        protected override bool Filter(Entity entity)
        {
            return entity.HasComponent(typeof(AnimationState));
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var query = new EntityQuery().Or(typeof(Movement), typeof(Components.Combat));
            var listener = context.CreateListener(query);

            listener.ListenToAdded = true;
            listener.ListenToChanged = true;
            listener.ListenToRemoved = false;

            return listener;
        }
    }
}
