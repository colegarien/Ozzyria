using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Content.Models;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Systems.Rendering
{
    internal class SkeletonSystem : TriggerSystem
    {
        public SkeletonSystem(EntityContext context) : base(context)
        {
        }

        public override void Execute(EntityContext context, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var animator = (Animator)entity.GetComponent(typeof(Animator));
                var skeleton = (Skeleton)entity.GetComponent(typeof(Skeleton));

                Direction direction = movement.GetLookDirection();

                // Calculate New/Current Offsets
                SkeletonOffsets offsets = SkeletonOffsetResolver.Get(skeleton.Type, animator.CurrentPose, direction, animator.Frame);

                // Apply Offsets to skeleton
                skeleton.Pose = animator.CurrentPose;
                skeleton.Frame = animator.Frame;
                skeleton.Layer = movement.Layer;
                skeleton.SubLayer = (int)(movement.Y + offsets.RootOffsetY + offsets.RenderOffsetY);
                skeleton.Direction = direction;
                skeleton.RootX = (int)(movement.X + offsets.RootOffsetX);
                skeleton.RootY = (int)(movement.Y + offsets.RootOffsetY);
                skeleton.RenderOffsetY = offsets.RenderOffsetY;
                skeleton.WeaponOffsetX = offsets.WeaponOffsetX;
                skeleton.WeaponOffsetY = offsets.WeaponOffsetY;
                skeleton.WeaponOffsetAngle = offsets.WeaponOffsetAngle;
                skeleton.ArmorOffsetX =     offsets.ArmorOffsetX;
                skeleton.ArmorOffsetY =     offsets.ArmorOffsetY;
                skeleton.ArmorOffsetAngle = offsets.ArmorOffsetAngle;
                skeleton.HatOffsetX =     offsets.HatOffsetX;
                skeleton.HatOffsetY =     offsets.HatOffsetY;
                skeleton.HatOffsetAngle = offsets.HatOffsetAngle;
                skeleton.MaskOffsetX =     offsets.MaskOffsetX;
                skeleton.MaskOffsetY =     offsets.MaskOffsetY;
                skeleton.MaskOffsetAngle = offsets.MaskOffsetAngle;
            }
        }

        protected override bool Filter(Entity entity)
        {
            return true;
        }

        protected override QueryListener GetListener(EntityContext context)
        {
            var listener = context.CreateListener(new EntityQuery().And(typeof(Movement), typeof(Animator), typeof(Skeleton)));
            listener.ListenToAdded = true;
            listener.ListenToChanged = true;

            return listener;
        }
    }
}
