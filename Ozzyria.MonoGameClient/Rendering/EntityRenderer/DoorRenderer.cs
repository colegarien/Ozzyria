using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class DoorRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Door));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            // TODO OZ-22 make doors have their own graphic
            var drawable = ItemDrawableResolver.Get("exp_orb", Direction.None, 0);
            PushDrawable(graphicsPipeline, entity, skeleton, drawable, Color.White);
        }
    }
}
