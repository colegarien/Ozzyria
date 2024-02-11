using Microsoft.Xna.Framework;
using Ozzyria.Game.Animation;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using Ozzyria.MonoGameClient.Rendering.Resolver;

namespace Ozzyria.MonoGameClient.Rendering.EntityRenderer
{
    internal class WeaponRenderer : EntityRenderPipeline
    {
        protected override bool CanRender(Entity entity)
        {
            return entity.HasComponent(typeof(Combat)) && entity.HasComponent(typeof(Weapon));
        }

        protected override void DoRender(GraphicsPipeline graphicsPipeline, Entity entity, Skeleton skeleton)
        {
            var combat = (Combat)entity.GetComponent(typeof(Combat));
            if (!combat.Attacking)
                return;

            var weapon = (Weapon)entity.GetComponent(typeof(Weapon));
            var weaponDrawable = WeaponDrawableResolver.GetWeapon(weapon.WeaponType, weapon.WeaponId);
            weaponDrawable.FlipVertically = skeleton.Direction == Direction.Left || skeleton.Direction == Direction.Up;
            PushDrawable(graphicsPipeline, entity, skeleton, weaponDrawable, combat.Frame == combat.DamageFrame ? Color.Red : Color.White);

            if (combat.Frame == combat.DamageFrame)
            {
                var effectDrawable = WeaponDrawableResolver.GetWeaponTrail(weapon.WeaponType, weapon.WeaponId);
                PushDrawable(graphicsPipeline, entity, skeleton, effectDrawable, Color.White);
            }
        }
    }
}
