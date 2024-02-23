using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Grecs;
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
            uint pixelResource = 0;
            var pixelSource = new Rectangle(943, 56, 1, 1);
            var pixelOrigin = Vector2.Zero;

            // Draw Position
            var position = (Movement)entity.GetComponent(typeof(Movement));
            var positionGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            positionGraphic.Resource = pixelResource;
            positionGraphic.Layer = 99;
            positionGraphic.SubLayer = 0;
            positionGraphic.SubSubLayer = 0;
            positionGraphic.Destination = new Rectangle((int)position.X, (int)position.Y, 2, 2);
            positionGraphic.Source = pixelSource;
            positionGraphic.Origin = pixelOrigin;
            positionGraphic.Angle = 0;
            positionGraphic.Colour = Color.Blue;

            if (entity.HasComponent(typeof(BoundingCircle)))
            {
                var circle = (BoundingCircle)entity.GetComponent(typeof(BoundingCircle));
                var circleLeftGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleLeftGraphic.Resource = pixelResource;
                circleLeftGraphic.Layer = 99;
                circleLeftGraphic.SubLayer = 0;
                circleLeftGraphic.SubSubLayer = 0;
                circleLeftGraphic.Destination = new Rectangle((int)(position.X-circle.Radius), (int)(position.Y), 2, 2);
                circleLeftGraphic.Source = pixelSource;
                circleLeftGraphic.Origin = pixelOrigin;
                circleLeftGraphic.Angle = 0;
                circleLeftGraphic.Colour = Color.Yellow;

                var circleRightGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleRightGraphic.Resource = pixelResource;
                circleRightGraphic.Layer = 99;
                circleRightGraphic.SubLayer = 0;
                circleRightGraphic.SubSubLayer = 0;
                circleRightGraphic.Destination = new Rectangle((int)(position.X + circle.Radius), (int)(position.Y), 2, 2);
                circleRightGraphic.Source = pixelSource;
                circleRightGraphic.Origin = pixelOrigin;
                circleRightGraphic.Angle = 0;
                circleRightGraphic.Colour = Color.Yellow;

                var circleTopGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleTopGraphic.Resource = pixelResource;
                circleTopGraphic.Layer = 99;
                circleTopGraphic.SubLayer = 0;
                circleTopGraphic.SubSubLayer = 0;
                circleTopGraphic.Destination = new Rectangle((int)(position.X), (int)(position.Y - circle.Radius), 2, 2);
                circleTopGraphic.Source = pixelSource;
                circleTopGraphic.Origin = pixelOrigin;
                circleTopGraphic.Angle = 0;
                circleTopGraphic.Colour = Color.Yellow;

                var circleBottomGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleBottomGraphic.Resource = pixelResource;
                circleBottomGraphic.Layer = 99;
                circleBottomGraphic.SubLayer = 0;
                circleBottomGraphic.SubSubLayer = 0;
                circleBottomGraphic.Destination = new Rectangle((int)(position.X), (int)(position.Y + circle.Radius), 2, 2);
                circleBottomGraphic.Source = pixelSource;
                circleBottomGraphic.Origin = pixelOrigin;
                circleBottomGraphic.Angle = 0;
                circleBottomGraphic.Colour = Color.Yellow;
            }

            // Draw Root
            var rootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            rootGraphic.Resource = pixelResource;
            rootGraphic.Layer = 99;
            rootGraphic.SubLayer = 0;
            rootGraphic.SubSubLayer = 0;
            rootGraphic.Destination = new Rectangle(skeleton.RootX, skeleton.RootY, 2, 2);
            rootGraphic.Source = pixelSource;
            rootGraphic.Origin = pixelOrigin;
            rootGraphic.Angle = 0;
            rootGraphic.Colour = Color.Green;

            // Draw Weapon Offset
            var weaponRootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            weaponRootGraphic.Resource = pixelResource;
            weaponRootGraphic.Layer = 99;
            weaponRootGraphic.SubLayer = 0;
            weaponRootGraphic.SubSubLayer = 0;
            weaponRootGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.WeaponOffsetX, skeleton.RootY + skeleton.WeaponOffsetY, 2, 2);
            weaponRootGraphic.Source = pixelSource;
            weaponRootGraphic.Origin = pixelOrigin;
            weaponRootGraphic.Angle = 0;
            weaponRootGraphic.Colour = Color.Magenta;
            var weaponAngleGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            weaponAngleGraphic.Resource = pixelResource;
            weaponAngleGraphic.Layer = 99;
            weaponAngleGraphic.SubLayer = 0;
            weaponAngleGraphic.SubSubLayer = 0;
            weaponAngleGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.WeaponOffsetX + (int)(Math.Cos(skeleton.WeaponOffsetAngle) * 10), skeleton.RootY + skeleton.WeaponOffsetY + (int)(Math.Sin(skeleton.WeaponOffsetAngle) * 10), 2, 2);
            weaponAngleGraphic.Source = pixelSource;
            weaponAngleGraphic.Origin = pixelOrigin;
            weaponAngleGraphic.Angle = 0;
            weaponAngleGraphic.Colour = Color.DarkMagenta;
        }
    }
}
