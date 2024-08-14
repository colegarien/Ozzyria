using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;
using Skeleton = Ozzyria.Model.Components.Skeleton;
using Hat = Ozzyria.Model.Components.Hat;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class HatRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Hat));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var hat = (Hat)entity.GetComponent(typeof(Hat));
            if (hat.HatId == "")
                return;

            PushDrawable(graphicsPipeline, entity, skeleton, ItemDrawableResolver.Get(hat.HatId, skeleton.Direction, skeleton.Frame));
        }
    }
}
