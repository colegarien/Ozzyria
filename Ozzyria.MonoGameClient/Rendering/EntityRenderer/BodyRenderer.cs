using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;
using Skeleton = Ozzyria.Model.Components.Skeleton;
using Body = Ozzyria.Model.Components.Body;

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
            if (body.BodyId == "")
                return;

            var bodyDrawable = BodyDrawableResolver.Get(body.BodyId, skeleton.Pose, skeleton.Direction, skeleton.Frame);
            PushDrawable(graphicsPipeline, entity, skeleton, bodyDrawable);
        }
    }
}
