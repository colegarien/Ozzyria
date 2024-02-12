using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class ExperienceOrbRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(ExperienceOrbThought));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var drawable = ItemDrawableResolver.Get("exp_orb", Direction.None, 0);
            PushDrawable(graphicsPipeline, entity, skeleton, drawable, Color.Yellow);
        }
    }
}
