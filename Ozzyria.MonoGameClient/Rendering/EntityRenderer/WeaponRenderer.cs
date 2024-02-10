using Microsoft.Xna.Framework;
using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;

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
            switch (weapon.WeaponType)
            {
                case WeaponType.Sword:
                    var drawable = Drawable.WEAPON_DAGGER;
                    drawable.FlipVertically = skeleton.Direction == Direction.Left || skeleton.Direction == Direction.Up;
                    PushDrawable(graphicsPipeline, entity, skeleton, drawable, combat.Frame == combat.DamageFrame ? Color.Red : Color.White);

                    if (combat.Frame == combat.DamageFrame) {
                        var effectDrawable = Drawable.WEAPON_TRAIL;
                        PushDrawable(graphicsPipeline, entity, skeleton, effectDrawable, Color.White);
                    }
                    break;
            }
        }
    }
}
