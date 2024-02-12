using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class BagRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Bag));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            // TODO get rid of this, just make like a "static renderable" or something instead of this
            var drawable = ItemDrawableResolver.Get("simple_bag", Direction.None, 0);
            PushDrawable(graphicsPipeline, entity, skeleton, drawable, Color.White);
        }
    }
}
