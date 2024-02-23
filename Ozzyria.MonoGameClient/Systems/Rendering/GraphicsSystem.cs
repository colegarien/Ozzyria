﻿using Grecs;
using Ozzyria.MonoGameClient.Rendering.EntityRenderer;
using Ozzyria.MonoGameClient.Rendering;
using Ozzyria.Game.Components;

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
                .Chain(new ExperienceOrbRenderer())
                .Chain(new DoorRenderer())
                .Chain(new BagRenderer())

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
            entityPipeline.Render(graphicsPipeline, entity, (Skeleton)entity.GetComponent(typeof(Skeleton)));
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var listener = context.CreateListener(new EntityQuery().And(typeof(Skeleton)));
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;
            listener.ListenToRemoved = true;

            return listener;
        }
    }
}