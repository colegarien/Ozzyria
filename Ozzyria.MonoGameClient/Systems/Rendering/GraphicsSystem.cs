using Grecs;
using Ozzyria.MonoGameClient.Rendering.EntityRenderer;
using Ozzyria.MonoGameClient.Rendering;
using Ozzyria.Model.Components;

namespace Ozzyria.MonoGameClient.Systems.Rendering
{
    internal class GraphicsSystem : TriggerSystem
    {

        GraphicsPipeline graphicsPipeline;
        EntityRenderPipeline entityPipeline;
        public GraphicsSystem(EntityContext context) : base(context)
        {
            graphicsPipeline = GraphicsPipeline.Get();
            entityPipeline = new BodyRenderer()
                .Chain(new WeaponRenderer())
                .Chain(new ArmorRenderer())
                .Chain(new HatRenderer())
                .Chain(new MaskRenderer())

                .Chain(new DebugRenderer());
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entityId in context.GetRecentlyDestroyed())
            {
                graphicsPipeline.ClearEntityGraphics(entityId);
            }

            foreach (var entity in entities)
            {
                RenderEntity(entity);
            }
        }

        public void RenderEntity(Entity entity)
        {
            entityPipeline.Render(graphicsPipeline, entity, entity.GetComponent<Skeleton>());
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var listener = context.CreateListener(new EntityQuery().Or(typeof(Movement), typeof(Skeleton)));
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;
            listener.ListenToRemoved = true;

            return listener;
        }
    }
}
