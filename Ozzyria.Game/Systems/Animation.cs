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
                if (renderable.timer > 0.1f)
                {
                    renderable.timer = 0;
                    renderable.CurrentFrame++;
                }

                // TODO OZ-23 : check transitions to new states or clips/directions
                if (state.State == "idle")
                {
                    if (state.GetVariable("IsAttacking").Equals("true"))
                    {
                        state.State = "attack";
                    }
                }
                else if (state.State == "attack")
                {
                    if (!state.GetVariable("IsAttacking").Equals("true"))
                    {
                        state.State = "idle";
                    }
                }

                // TODO OZ-23 don't hard code this... make this more data-driven!
                renderable.CurrentClip = $"body_white_{state.State}_{state.GetVariable("Direction")}";
            }
        }
    }
}
