using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System;
using System.Resources;

namespace Ozzyria.Game.Systems
{
    internal class Animation : TickSystem
    {
        protected EntityQuery query;
        public Animation()
        {
            query = new EntityQuery();
            query.And(typeof(AnimationState), typeof(Movement), typeof(Renderable));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var state = (AnimationState)entity.GetComponent(typeof(AnimationState));
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var renderable = (Renderable)entity.GetComponent(typeof(Renderable));
                var combat = (Components.Combat)entity.GetComponent(typeof(Components.Combat));


                // TODO OZ-23 : https://gamedev.stackexchange.com/questions/197372/how-to-do-state-based-animation-with-ecs
                renderable.timer += deltaTime;
                if (renderable.timer > 0.1f)
                {
                    renderable.timer = 0;
                    renderable.CurrentFrame++;
                }

                // TODO OZ-23 : check transitions to new states or clips/directions
                if(movement.LookDirection <= 0.78 || movement.LookDirection >= 5.48)
                {
                    state.Direction = "south";
                }
                else if (movement.LookDirection > 0.78 && movement.LookDirection < 2.36)
                {
                    state.Direction = "east";
                }
                else if (movement.LookDirection >= 2.36 && movement.LookDirection <= 3.94)
                {
                    state.Direction = "north";
                }
                else
                {
                    state.Direction = "west";
                }

                // TODO OZ-23 this can be better, more data-driven 
                if (state.State == "idle")
                {
                    if (combat.Attacking)
                    {
                        state.State = "attack";
                    }
                }
                else if (state.State == "attack")
                {
                    if (!combat.Attacking)
                    {
                        state.State = "idle";
                    }
                }

                // TODO OZ-23 don't hard code this... make this more data-driven!
                renderable.CurrentClip = $"body_white_{state.State}_{state.Direction}";
            }
        }
    }
}
