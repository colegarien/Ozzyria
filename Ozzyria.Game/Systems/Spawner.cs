using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game.Systems
{
    internal class Spawner : TickSystem
    {
        protected EntityQuery query;
        public Spawner()
        {
            query = new EntityQuery();
            query.And(typeof(SlimeSpawner));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var spawner = (SlimeSpawner)entity.GetComponent(typeof(SlimeSpawner));
                spawner.ThinkDelay.Update(deltaTime);

                var numberOfSlimes = context.GetEntities(new EntityQuery().And(typeof(SlimeThought))).Count();
                if (numberOfSlimes < spawner.SLIME_LIMIT && spawner.ThinkDelay.IsReady())
                {
                    // OZ-22 : check if spawner is block before spawning things
                    EntityFactory.CreateSlime(context, spawner.X, spawner.Y);
                }
            }
        }
    }
}
