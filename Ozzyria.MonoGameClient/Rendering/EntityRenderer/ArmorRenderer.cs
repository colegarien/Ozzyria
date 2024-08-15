using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;
using Ozzyria.Model.Components;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class ArmorRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Ozzyria.Model.Components.Armor));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var armor = (Ozzyria.Model.Components.Armor)entity.GetComponent(typeof(Ozzyria.Model.Components.Armor));
            if (armor.ArmorId == "")
                return;

            PushDrawable(graphicsPipeline, entity, skeleton, ItemDrawableResolver.Get(armor.ArmorId, skeleton.Direction, skeleton.Frame));
        }
    }
}
