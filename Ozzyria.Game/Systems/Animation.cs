using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Systems
{
    internal class Animation : TickSystem
    {
        protected EntityQuery query;
        public Animation()
        {
            query = new EntityQuery();
            query.And(typeof(AnimationState), typeof(Renderable));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var state = (AnimationState)entity.GetComponent(typeof(AnimationState));
                var renderable = (Renderable)entity.GetComponent(typeof(Renderable));

                // TODO OZ-23 : https://gamedev.stackexchange.com/questions/197372/how-to-do-state-based-animation-with-ecs
                renderable.timer += deltaTime;
                if (renderable.timer > 0.1f) // TODO OZ-23 around 10fps
                {
                    renderable.timer = 0;
                    renderable.CurrentFrame++;
                }

                // TODO OZ-23 : check transitions to new states and do transition
                if (state.State == "idle")
                {
                    if (state.GetBoolVariable("WantsToAttack"))
                    {
                        state.State = "attack"; // TODO OZ-23 tie state into Attack timer and stuff so combat is in-sync with animation (or vice-versa)?
                        renderable.timer = 0;
                        renderable.CurrentFrame = 0;
                    }
                }
                else if (state.State == "attack")
                {
                    if (!state.GetBoolVariable("WantsToAttack")) // TODO OZ-23 AND attack animation is done
                    {
                        state.State = "idle";
                        renderable.timer = 0;
                        renderable.CurrentFrame = 0;
                    }
                }
            }
        }
    }
}
