using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class BodyRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Body));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var body = (Body)entity.GetComponent(typeof(Body));
            var bodyDrawable = BodyDrawableResolver.Get(body.BodyType, skeleton.Pose, skeleton.Direction, skeleton.Frame);
            PushDrawable(graphicsPipeline, entity, skeleton, bodyDrawable, Color.White);
        }
    }
}
