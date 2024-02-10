using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class DebugRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Movement));
            //return false;
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var pixel = Drawable.PIXEL;

            // Draw Position
            var position = (Movement)entity.GetComponent(typeof(Movement));
            var positionGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            positionGraphic.Layer = 99;
            positionGraphic.SubLayer = 0;
            positionGraphic.SubSubLayer = 0;
            positionGraphic.Destination = new Rectangle((int)position.X, (int)position.Y, 2, 2);
            positionGraphic.Source = pixel.Source;
            positionGraphic.Angle = 0;
            positionGraphic.Origin = pixel.Origin;
            positionGraphic.Colour = Color.Blue;

            if (entity.HasComponent(typeof(BoundingCircle)))
            {
                var circle = (BoundingCircle)entity.GetComponent(typeof(BoundingCircle));
                var circleLeftGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleLeftGraphic.Layer = 99;
                circleLeftGraphic.SubLayer = 0;
                circleLeftGraphic.SubSubLayer = 0;
                circleLeftGraphic.Destination = new Rectangle((int)(position.X-circle.Radius), (int)(position.Y), 2, 2);
                circleLeftGraphic.Source = pixel.Source;
                circleLeftGraphic.Angle = 0;
                circleLeftGraphic.Origin = pixel.Origin;
                circleLeftGraphic.Colour = Color.Yellow;

                var circleRightGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleRightGraphic.Layer = 99;
                circleRightGraphic.SubLayer = 0;
                circleRightGraphic.SubSubLayer = 0;
                circleRightGraphic.Destination = new Rectangle((int)(position.X + circle.Radius), (int)(position.Y), 2, 2);
                circleRightGraphic.Source = pixel.Source;
                circleRightGraphic.Angle = 0;
                circleRightGraphic.Origin = pixel.Origin;
                circleRightGraphic.Colour = Color.Yellow;

                var circleTopGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleTopGraphic.Layer = 99;
                circleTopGraphic.SubLayer = 0;
                circleTopGraphic.SubSubLayer = 0;
                circleTopGraphic.Destination = new Rectangle((int)(position.X), (int)(position.Y - circle.Radius), 2, 2);
                circleTopGraphic.Source = pixel.Source;
                circleTopGraphic.Angle = 0;
                circleTopGraphic.Origin = pixel.Origin;
                circleTopGraphic.Colour = Color.Yellow;

                var circleBottomGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleBottomGraphic.Layer = 99;
                circleBottomGraphic.SubLayer = 0;
                circleBottomGraphic.SubSubLayer = 0;
                circleBottomGraphic.Destination = new Rectangle((int)(position.X), (int)(position.Y + circle.Radius), 2, 2);
                circleBottomGraphic.Source = pixel.Source;
                circleBottomGraphic.Angle = 0;
                circleBottomGraphic.Origin = pixel.Origin;
                circleBottomGraphic.Colour = Color.Yellow;
            }

            // Draw Root
            var rootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            rootGraphic.Layer = 99;
            rootGraphic.SubLayer = 0;
            rootGraphic.SubSubLayer = 0;
            rootGraphic.Destination = new Rectangle(skeleton.RootX, skeleton.RootY, 2, 2);
            rootGraphic.Source = pixel.Source;
            rootGraphic.Angle = 0;
            rootGraphic.Origin = pixel.Origin;
            rootGraphic.Colour = Color.Green;

            // Draw Weapon Offset
            var weaponRootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            weaponRootGraphic.Layer = 99;
            weaponRootGraphic.SubLayer = 0;
            weaponRootGraphic.SubSubLayer = 0;
            weaponRootGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.WeaponOffsetX, skeleton.RootY + skeleton.WeaponOffsetY, 2, 2);
            weaponRootGraphic.Source = pixel.Source;
            weaponRootGraphic.Angle = 0;
            weaponRootGraphic.Origin = pixel.Origin;
            weaponRootGraphic.Colour = Color.Magenta;
            var weaponAngleGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            weaponAngleGraphic.Layer = 99;
            weaponAngleGraphic.SubLayer = 0;
            weaponAngleGraphic.SubSubLayer = 0;
            weaponAngleGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.WeaponOffsetX + (int)(Math.Cos(skeleton.WeaponOffsetAngle) * 10), skeleton.RootY + skeleton.WeaponOffsetY + (int)(Math.Sin(skeleton.WeaponOffsetAngle) * 10), 2, 2);
            weaponAngleGraphic.Source = pixel.Source;
            weaponAngleGraphic.Angle = 0;
            weaponAngleGraphic.Origin = pixel.Origin;
            weaponAngleGraphic.Colour = Color.DarkMagenta;
        }
    }
}
