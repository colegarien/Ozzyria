﻿using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;
using Ozzyria.Model.Types;
using Skeleton = Ozzyria.Model.Components.Skeleton;
using Weapon = Ozzyria.Model.Components.Weapon;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class WeaponRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Ozzyria.Model.Components.AttackIntent)) && entity.HasComponent(typeof(Weapon));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var intent = (Ozzyria.Model.Components.AttackIntent)entity.GetComponent(typeof(Ozzyria.Model.Components.AttackIntent));

            var weapon = (Weapon)entity.GetComponent(typeof(Weapon));
            if (weapon.WeaponType != WeaponType.Empty)
            {
                var weaponDrawable = ItemDrawableResolver.Get(weapon.WeaponId, skeleton.Direction, skeleton.Frame);
                weaponDrawable.FlipVertically = skeleton.Direction == Direction.Left || skeleton.Direction == Direction.Up;
                PushDrawable(graphicsPipeline, entity, skeleton, weaponDrawable);
            }

            if (intent.Frame == intent.DamageFrame)
            {
                var effectDrawable = ItemDrawableResolver.Get("basic-trail", skeleton.Direction, skeleton.Frame);
                PushDrawable(graphicsPipeline, entity, skeleton, effectDrawable);
            }
        }
    }
}
