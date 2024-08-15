using Ozzyria.Model.Components;
using Ozzyria.Model.Extensions;
using Grecs;
using Ozzyria.Game.Utility;
using System.Linq;
using Ozzyria.Model.CodeGen.Packages;
using Ozzyria.Content;

namespace Ozzyria.Game.Systems
{
    internal class Spawner : TickSystem
    {
        private PrefabPackage prefabPackage;
        protected EntityQuery query;
        public Spawner()
        {
            prefabPackage = Packages.GetInstance().PrefabPackage;

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

                // OZ-22 : check if spawner is block before spawning things
                var numberOfSlimes = context.GetEntities(new EntityQuery().And(typeof(SlimeThought))).Count();
                if (numberOfSlimes < spawner.SLIME_LIMIT && spawner.ThinkDelay.IsReady())
                {
                    // TOOD probably should make some helper utility for doing these on-the-fly hydrations!
                    var slimePrefab = prefabPackage.GetDefinition("slime");
                    Model.Utility.EntityFactory.HydrateDefinition(context, slimePrefab, new Model.Types.ValuePacket
                    {
                        { "movement->x", spawner.X.ToString() },
                        { "movement->y", spawner.Y.ToString() },
                        { "movement->previousX", spawner.X.ToString() },
                        { "movement->previousY", spawner.Y.ToString() }
                    });
                }
            }
        }
    }
}
