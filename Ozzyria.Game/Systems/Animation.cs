using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System;

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


                // TODO OZ-23 : https://gamedev.stackexchange.com/questions/197372/how-to-do-state-based-animation-with-ecs
                renderable.timer += deltaTime;
                if (renderable.timer > 0.1f)
                {
                    renderable.timer = 0;
                    renderable.CurrentFrame++;
                }

                // TODO OZ-23 : check transitions to new states or clips/directions
                if(movement.LookDirection > 0.3 && movement.LookDirection > 0.9)
                {
                    renderable.CurrentClip = "body_white_idle_south";
                }
                else
                {
                    renderable.CurrentClip = "body_white_idle_east";
                }
            }
        }
    }
}
