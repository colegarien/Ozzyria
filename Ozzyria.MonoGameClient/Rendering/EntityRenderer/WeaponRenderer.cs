﻿using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class WeaponRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(AttackIntent)) && entity.HasComponent(typeof(Weapon));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var intent = (AttackIntent)entity.GetComponent(typeof(AttackIntent));

            var weapon = (Weapon)entity.GetComponent(typeof(Weapon));
            if (weapon.WeaponType != WeaponType.Empty)
            {
                var weaponDrawable = ItemDrawableResolver.Get(weapon.WeaponId, skeleton.Direction, skeleton.Frame);
                weaponDrawable.FlipVertically = skeleton.Direction == Direction.Left || skeleton.Direction == Direction.Up;
                PushDrawable(graphicsPipeline, entity, skeleton, weaponDrawable, intent.Frame == intent.DamageFrame ? Color.Red : Color.White);
            }

            if (intent.Frame == intent.DamageFrame)
            {
                var effectDrawable = ItemDrawableResolver.Get("basic-trail", skeleton.Direction, skeleton.Frame);
                PushDrawable(graphicsPipeline, entity, skeleton, effectDrawable, Color.White);
            }
        }
    }
}
