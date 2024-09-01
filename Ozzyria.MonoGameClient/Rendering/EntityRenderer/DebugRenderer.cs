using Microsoft.Xna.Framework;
using Grecs;
using System;
using Ozzyria.Model.Components;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class DebugRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return Settings.DebugRendering && entity.HasComponent(typeof(Movement));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            uint pixelResource = 0;
            var pixelSource = new Rectangle(943, 56, 1, 1);
            var pixelOrigin = Vector2.Zero;

            // Draw Position
            var movement = (Movement)entity.GetComponent(typeof(Movement));
            var positionGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            positionGraphic.Resource = pixelResource;
            positionGraphic.Layer = 99;
            positionGraphic.SubLayer = 0;
            positionGraphic.SubSubLayer = 0;
            positionGraphic.Destination = new Rectangle((int)movement.X, (int)movement.Y, 2, 2);
            positionGraphic.Source = pixelSource;
            positionGraphic.Origin = pixelOrigin;
            positionGraphic.Angle = 0;
            positionGraphic.Colour = Color.Blue;

            var collisionOffsetY = movement.CollisionOffsetY;
            if (movement.CollisionShape?.BoundingCircle?.Radius > 0)
            {
                var radius = movement.CollisionShape.BoundingCircle.Radius;
                var circleLeftGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleLeftGraphic.Resource = pixelResource;
                circleLeftGraphic.Layer = 99;
                circleLeftGraphic.SubLayer = 0;
                circleLeftGraphic.SubSubLayer = 0;
                circleLeftGraphic.Destination = new Rectangle((int)(movement.X- radius), (int)(movement.Y + collisionOffsetY), 2, 2);
                circleLeftGraphic.Source = pixelSource;
                circleLeftGraphic.Origin = pixelOrigin;
                circleLeftGraphic.Angle = 0;
                circleLeftGraphic.Colour = Color.Yellow;

                var circleRightGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleRightGraphic.Resource = pixelResource;
                circleRightGraphic.Layer = 99;
                circleRightGraphic.SubLayer = 0;
                circleRightGraphic.SubSubLayer = 0;
                circleRightGraphic.Destination = new Rectangle((int)(movement.X + radius), (int)(movement.Y + collisionOffsetY), 2, 2);
                circleRightGraphic.Source = pixelSource;
                circleRightGraphic.Origin = pixelOrigin;
                circleRightGraphic.Angle = 0;
                circleRightGraphic.Colour = Color.Yellow;

                var circleTopGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleTopGraphic.Resource = pixelResource;
                circleTopGraphic.Layer = 99;
                circleTopGraphic.SubLayer = 0;
                circleTopGraphic.SubSubLayer = 0;
                circleTopGraphic.Destination = new Rectangle((int)(movement.X), (int)(movement.Y + collisionOffsetY - radius), 2, 2);
                circleTopGraphic.Source = pixelSource;
                circleTopGraphic.Origin = pixelOrigin;
                circleTopGraphic.Angle = 0;
                circleTopGraphic.Colour = Color.Yellow;

                var circleBottomGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleBottomGraphic.Resource = pixelResource;
                circleBottomGraphic.Layer = 99;
                circleBottomGraphic.SubLayer = 0;
                circleBottomGraphic.SubSubLayer = 0;
                circleBottomGraphic.Destination = new Rectangle((int)(movement.X), (int)(movement.Y + collisionOffsetY + radius), 2, 2);
                circleBottomGraphic.Source = pixelSource;
                circleBottomGraphic.Origin = pixelOrigin;
                circleBottomGraphic.Angle = 0;
                circleBottomGraphic.Colour = Color.Yellow;

                var circleCenterGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                circleCenterGraphic.Resource = pixelResource;
                circleCenterGraphic.Layer = 99;
                circleCenterGraphic.SubLayer = 0;
                circleCenterGraphic.SubSubLayer = 0;
                circleCenterGraphic.Destination = new Rectangle((int)(movement.X), (int)(movement.Y + collisionOffsetY), 2, 2);
                circleCenterGraphic.Source = pixelSource;
                circleCenterGraphic.Origin = pixelOrigin;
                circleCenterGraphic.Angle = 0;
                circleCenterGraphic.Colour = Color.Orange;
            }
            
            if (movement.CollisionShape.BoundingBox?.Width > 0 && movement.CollisionShape.BoundingBox?.Height > 0)
            {
                var boxWidth = movement.CollisionShape.BoundingBox.Width;
                var boxHeight = movement.CollisionShape.BoundingBox.Height;
                var boxLeft = movement.X - (boxWidth / 2f);
                var boxRight = movement.X + (boxWidth / 2f);
                var boxTop = movement.Y - (boxHeight / 2f);
                var boxBottom = movement.Y + (boxHeight / 2f);

                var boxLeftGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                boxLeftGraphic.Resource = pixelResource;
                boxLeftGraphic.Layer = 99;
                boxLeftGraphic.SubLayer = 0;
                boxLeftGraphic.SubSubLayer = 0;
                boxLeftGraphic.Destination = new Rectangle((int)boxLeft, (int)boxTop, 2, (int)(boxHeight));
                boxLeftGraphic.Source = pixelSource;
                boxLeftGraphic.Origin = pixelOrigin;
                boxLeftGraphic.Angle = 0;
                boxLeftGraphic.Colour = Color.Yellow;

                var boxRightGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                boxRightGraphic.Resource = pixelResource;
                boxRightGraphic.Layer = 99;
                boxRightGraphic.SubLayer = 0;
                boxRightGraphic.SubSubLayer = 0;
                boxRightGraphic.Destination = new Rectangle((int)boxRight - 2, (int)boxTop, 2, (int)(boxHeight));
                boxRightGraphic.Source = pixelSource;
                boxRightGraphic.Origin = pixelOrigin;
                boxRightGraphic.Angle = 0;
                boxRightGraphic.Colour = Color.Yellow;

                var boxTopGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                boxTopGraphic.Resource = pixelResource;
                boxTopGraphic.Layer = 99;
                boxTopGraphic.SubLayer = 0;
                boxTopGraphic.SubSubLayer = 0;
                boxTopGraphic.Destination = new Rectangle((int)boxLeft, (int)boxTop, (int)(boxWidth), 2);
                boxTopGraphic.Source = pixelSource;
                boxTopGraphic.Origin = pixelOrigin;
                boxTopGraphic.Angle = 0;
                boxTopGraphic.Colour = Color.Yellow;

                var boxBottomGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                boxBottomGraphic.Resource = pixelResource;
                boxBottomGraphic.Layer = 99;
                boxBottomGraphic.SubLayer = 0;
                boxBottomGraphic.SubSubLayer = 0;
                boxBottomGraphic.Destination = new Rectangle((int)boxLeft, (int)boxBottom - 2, (int)(boxWidth), 2);
                boxBottomGraphic.Source = pixelSource;
                boxBottomGraphic.Origin = pixelOrigin;
                boxBottomGraphic.Angle = 0;
                boxBottomGraphic.Colour = Color.Yellow;

                var boxCenterGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
                boxCenterGraphic.Resource = pixelResource;
                boxCenterGraphic.Layer = 99;
                boxCenterGraphic.SubLayer = 0;
                boxCenterGraphic.SubSubLayer = 0;
                boxCenterGraphic.Destination = new Rectangle((int)(movement.X), (int)(movement.Y + collisionOffsetY), 2, 2);
                boxCenterGraphic.Source = pixelSource;
                boxCenterGraphic.Origin = pixelOrigin;
                boxCenterGraphic.Angle = 0;
                boxCenterGraphic.Colour = Color.Orange;
            }

            if (skeleton == null) {
                return;
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

            // Draw Hat Offset
            var hatRootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            hatRootGraphic.Resource = pixelResource;
            hatRootGraphic.Layer = 99;
            hatRootGraphic.SubLayer = 0;
            hatRootGraphic.SubSubLayer = 0;
            hatRootGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.HatOffsetX, skeleton.RootY + skeleton.HatOffsetY, 2, 2);
            hatRootGraphic.Source = pixelSource;
            hatRootGraphic.Origin = pixelOrigin;
            hatRootGraphic.Angle = 0;
            hatRootGraphic.Colour = Color.BlueViolet;

            // Draw Mask Offset
            var maskRootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            maskRootGraphic.Resource = pixelResource;
            maskRootGraphic.Layer = 99;
            maskRootGraphic.SubLayer = 0;
            maskRootGraphic.SubSubLayer = 0;
            maskRootGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.MaskOffsetX, skeleton.RootY + skeleton.MaskOffsetY, 2, 2);
            maskRootGraphic.Source = pixelSource;
            maskRootGraphic.Origin = pixelOrigin;
            maskRootGraphic.Angle = 0;
            maskRootGraphic.Colour = Color.Beige;

            // Armor Mask Offset
            var armorRootGraphic = graphicsPipeline.GetEntityGraphic(entity.id);
            armorRootGraphic.Resource = pixelResource;
            armorRootGraphic.Layer = 99;
            armorRootGraphic.SubLayer = 0;
            armorRootGraphic.SubSubLayer = 0;
            armorRootGraphic.Destination = new Rectangle(skeleton.RootX + skeleton.ArmorOffsetX, skeleton.RootY + skeleton.ArmorOffsetY, 2, 2);
            armorRootGraphic.Source = pixelSource;
            armorRootGraphic.Origin = pixelOrigin;
            armorRootGraphic.Angle = 0;
            armorRootGraphic.Colour = Color.Chartreuse;

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
