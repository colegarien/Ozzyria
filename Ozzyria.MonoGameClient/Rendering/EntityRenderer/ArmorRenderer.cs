using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;
using Ozzyria.Model.Components;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class ArmorRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Armor));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var armor = (Armor)entity.GetComponent(typeof(Armor));
            if (armor.ArmorId == "")
                return;

            PushDrawable(graphicsPipeline, entity, skeleton, ItemDrawableResolver.Get(armor.ArmorId, skeleton.Direction, skeleton.Frame));
        }
    }
}
