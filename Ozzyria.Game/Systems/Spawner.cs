using Ozzyria.Model.Components;
using Ozzyria.Model.Extensions;
using Grecs;
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
            query.And(typeof(PrefabSpawner));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {

            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var spawner = (PrefabSpawner)entity.GetComponent(typeof(PrefabSpawner));
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                spawner.ThinkDelay.Update(deltaTime);

                // OZ-22 : check if spawner is block before spawning things
                var numberOfPrefabs = context.GetEntities(new EntityQuery().And(spawner.EntityTag)).Count();
                if (numberOfPrefabs < spawner.EntityLimit && spawner.ThinkDelay.IsReady())
                {
                    var prefab = prefabPackage.GetDefinition(spawner.PrefabId);
                    Model.Utility.PrefabHydrator.HydrateDefinitionAtLocation(context, prefab, movement.X, movement.Y, movement.Layer);
                }
            }
        }
    }
}
