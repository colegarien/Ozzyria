using Ozzyria.Game.Component;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Systems
{
    internal class Thought : TickSystem
    {
        protected EntityQuery query;
        public Thought()
        {
            query = new EntityQuery();
            query.Or(typeof(PlayerThought), typeof(SlimeThought), typeof(SlimeSpawner), typeof(ExperienceOrbThought));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach(var entity in entities)
            {
                // Death Check
                if (entity.HasComponent(typeof(Stats)) && ((Stats)entity.GetComponent(typeof(Stats))).IsDead())
                {
                    continue;
                }

                var thought = (Component.Thought)(entity.GetComponent(typeof(PlayerThought)) ?? entity.GetComponent(typeof(SlimeThought)) ?? entity.GetComponent(typeof(SlimeSpawner)) ?? entity.GetComponent(typeof(ExperienceOrbThought)));
                thought.Update(deltaTime, context);


                // TODO OZ-14 : likey move this to collision thang (when it exists)
                if (entity.HasComponent(typeof(ExperienceBoost)) && ((ExperienceBoost)entity.GetComponent(typeof(ExperienceBoost))).HasBeenAbsorbed)
                {
                    context.DestroyEntity(entity);
                }
            }
        }
    }
}
