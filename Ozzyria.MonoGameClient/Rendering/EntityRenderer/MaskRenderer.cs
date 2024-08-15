using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;
using Ozzyria.Model.Components;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class MaskRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Mask));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var mask = (Mask)entity.GetComponent(typeof(Mask));
            if (mask.MaskId == "")
                return;

            PushDrawable(graphicsPipeline, entity, skeleton, ItemDrawableResolver.Get(mask.MaskId, skeleton.Direction, skeleton.Frame));
        }
    }
}
