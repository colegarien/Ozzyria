using Grecs;
using Ozzyria.Model.Types;
using Ozzyria.Model.CodeGen.Definitions;

namespace Ozzyria.Model.Utility
{
    public class PrefabHydrator
    {
        public static Entity HydrateDefinition(EntityContext context, PrefabDefinition prefab, ValuePacket values = null)
        {
            var entity = context.CreateEntity();

            var prefabValues = ValuePacket.Combine(prefab.Defaults, values);
            foreach (var componentId in prefab.Components)
            {
                if (EntitySerializer.ComponentIdToTypeMap.ContainsKey(componentId))
                {
                    var component = entity.CreateComponent(EntitySerializer.ComponentIdToTypeMap[componentId]);
                    if (component is IHydrateable)
                    {
                        ((IHydrateable)component).Hydrate(prefabValues.Extract(componentId));
                    }
                    entity.AddComponent(component);
                }
            }

            return entity;
        }

        public static Entity HydrateDefinitionAtLocation(EntityContext context, PrefabDefinition prefab, float x, float y, int layer, ValuePacket values = null)
        {
            var prefabValues = ValuePacket.Combine(values, new ValuePacket
            {
                    { "movement::x", x.ToString() },
                    { "movement::y", y.ToString() },
                    { "movement::previousX", x.ToString() },
                    { "movement::previousY", y.ToString() },
                    { "movement::layer", layer.ToString() },
            });

            return HydrateDefinition(context, prefab, prefabValues);
        }
    }
}
