using Grecs;
using Ozzyria.Model.Types;
using Grynt.Model.Definitions;

namespace Ozzyria.Model.Utility
{
    public class EntityFactory
    {
        public static void HydrateDefinition(EntityContext context, PrefabDefinition prefab, ValuePacket values)
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
        }
    }
}
