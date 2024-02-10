using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

namespace Ozzyria.MonoGameClient.Rendering
{
    internal abstract class EntityRenderPipeline
    {
        private EntityRenderPipeline _next = null;

        protected abstract bool CanRender(Entity entity);
        protected abstract void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton);

        protected void PushDrawable(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton, Drawable drawable, Color color)
        {
            var x = skeleton.RootX;
            var y = skeleton.RootY;
            var angle = drawable.BaseAngle;
            switch (drawable.SkeletonOffset)
            {
                case DrawableOffsetType.Weapon:
                    x += skeleton.WeaponOffsetX;
                    y += skeleton.WeaponOffsetY;
                    angle += skeleton.WeaponOffsetAngle;
                    break;
                case DrawableOffsetType.Armor:
                    x += skeleton.ArmorOffsetX;
                    y += skeleton.ArmorOffsetY;
                    angle += skeleton.ArmorOffsetAngle;
                    break;
                case DrawableOffsetType.Mask:
                    x += skeleton.MaskOffsetX;
                    y += skeleton.MaskOffsetY;
                    angle += skeleton.MaskOffsetAngle;
                    break;
                case DrawableOffsetType.Hat:
                    x += skeleton.HatOffsetX;
                    y += skeleton.HatOffsetY;
                    angle += skeleton.HatOffsetAngle;
                    break;
            }

            var graphic = graphicsPipeline.GetEntityGraphic(entity.id);
            graphic.Resource = drawable.Resource;
            graphic.Layer = skeleton.Layer;
            graphic.SubLayer = skeleton.SubLayer;
            graphic.SubSubLayer = drawable.Subspace;
            graphic.Destination = new Rectangle(x, y, drawable.Source.Width, drawable.Source.Height);
            graphic.Source = drawable.Source;
            graphic.Angle = angle;
            graphic.Origin = drawable.Origin;
            graphic.Colour = color;
            graphic.FlipVertically = drawable.FlipVertically;
            graphic.FlipHorizontally = drawable.FlipHorizontally;
        }

        public void Render(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            if (CanRender(entity))
                DoRender(graphicsPipeline, entity, skeleton);

            _next?.Render(graphicsPipeline, entity, skeleton);
        }

        public EntityRenderPipeline Chain(EntityRenderPipeline nextPipeline)
        {
            if (_next == null)
                _next = nextPipeline;
            else
                _next.Chain(nextPipeline);

            return this;
        }
    }
}
