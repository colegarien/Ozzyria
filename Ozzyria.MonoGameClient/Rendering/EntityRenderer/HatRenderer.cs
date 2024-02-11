using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.MonoGameClient.Rendering.Resolver;

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

            PushDrawable(graphicsPipeline, entity, skeleton, ItemDrawableResolver.Get(hat.HatId, skeleton.Direction, skeleton.Frame), Color.White);
        }
    }
}
