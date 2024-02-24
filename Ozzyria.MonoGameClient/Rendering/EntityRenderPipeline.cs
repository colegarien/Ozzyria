using Microsoft.Xna.Framework;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using Grecs;

namespace Ozzyria.MonoGameClient.Rendering
{
    internal abstract class EntityRenderPipeline
    {
        private EntityRenderPipeline _next = null;

        protected abstract bool CanRender(Entity entity);
        protected abstract void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton);

        protected void PushDrawable(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton, Drawable drawable)
        {
            var x = skeleton.RootX;
            var y = skeleton.RootY;
            var angle = drawable.BaseAngle;
            switch (drawable.AttachmentType)
            {
                case DrawableAttachmentType.Weapon:
                    x += skeleton.WeaponOffsetX;
                    y += skeleton.WeaponOffsetY;
                    angle += skeleton.WeaponOffsetAngle;
                    break;
                case DrawableAttachmentType.Armor:
                    x += skeleton.ArmorOffsetX;
                    y += skeleton.ArmorOffsetY;
                    angle += skeleton.ArmorOffsetAngle;
                    break;
                case DrawableAttachmentType.Mask:
                    x += skeleton.MaskOffsetX;
                    y += skeleton.MaskOffsetY;
                    angle += skeleton.MaskOffsetAngle;
                    break;
                case DrawableAttachmentType.Hat:
                    x += skeleton.HatOffsetX;
                    y += skeleton.HatOffsetY;
                    angle += skeleton.HatOffsetAngle;
                    break;
            }

            var color = Color.White;
            switch (drawable.ColorType)
            {
                case DrawableColorType.Yellow:
                    color = Color.Yellow;
                    break;
                case DrawableColorType.White:
                    break;
            }

            var graphic = graphicsPipeline.GetEntityGraphic(entity.id);
            graphic.Resource = drawable.Resource;
            graphic.Layer = skeleton.Layer;
            graphic.SubLayer = skeleton.SubLayer;
            graphic.SubSubLayer = drawable.Subspace;
            graphic.Destination = new Rectangle(x, y, drawable.Width, drawable.Height);
            graphic.Source = new Rectangle(drawable.Left, drawable.Top, drawable.Width, drawable.Height);
            graphic.Angle = angle;
            graphic.Origin = new Vector2(drawable.OriginX, drawable.OriginY);
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
