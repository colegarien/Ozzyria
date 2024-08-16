using Grecs;
using System.Linq;
using Ozzyria.Model.Components;
using Ozzyria.Model.Extensions;
using Ozzyria.Model.Types;

namespace Ozzyria.Game.Systems
{
    internal class AttackSystem : TickSystem
    {
        protected EntityQuery attackerQuery;
        protected EntityQuery targetQuery;
        public AttackSystem()
        {
            attackerQuery = new EntityQuery();
            attackerQuery.And(typeof(AttackIntent), typeof(Movement), typeof(Stats));

            targetQuery = new EntityQuery();
            targetQuery.And(typeof(Movement), typeof(Stats));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var attackerEntities = context.GetEntities(attackerQuery);
            if(attackerEntities.Length == 0)
                return;

            var targetEntities = context.GetEntities(targetQuery);
            foreach (var entity in attackerEntities)
            {
                var movement = entity.GetComponent<Movement>();
                var intent = entity.GetComponent<AttackIntent>();

                intent.FrameTimer += deltaTime;
                if (intent.FrameTimer >= intent.TimePerFrame)
                {
                    intent.FrameTimer -= intent.TimePerFrame;
                    intent.Frame++;
                    if (intent.Frame >= intent.DecayFrame)
                    {
                        entity.RemoveComponent(intent);
                    }
                    else if (intent.Frame == intent.DamageFrame && entity.HasComponent(typeof(Weapon)))
                    {
                        // TODO OZ-55 consider all equipment for damage formula
                        var weapon = entity.GetComponent<Weapon>();
                        var targetsInRange = targetEntities.Where(e => entity.id != e.id && e.GetComponent<Movement>().DistanceTo(movement) <= weapon.AttackRange);
                        foreach (var target in targetsInRange)
                        {
                            var targetMovement = target.GetComponent<Movement>();
                            var targetStats = target.GetComponent<Stats>();

                            var angleToTarget = AngleHelper.AngleTo(movement.X, movement.Y, targetMovement.X, targetMovement.Y);
                            if (AngleHelper.IsInArc(angleToTarget, movement.LookDirection, weapon.AttackAngle))
                            {
                                targetStats.Damage(weapon.AttackDamage);
                                if (targetStats.IsDead())
                                {
                                    target.AddComponent(target.CreateComponent(typeof(Dead)));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
