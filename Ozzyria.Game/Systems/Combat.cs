using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Game.Utility;
using System.Linq;

namespace Ozzyria.Game.Systems
{
    internal class Combat : TickSystem
    {
        protected EntityQuery query;
        public Combat()
        {
            query = new EntityQuery();
            query.And(typeof(Components.Combat), typeof(Movement), typeof(Stats));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var combat = (Components.Combat)entity.GetComponent(typeof(Components.Combat));
                if (!combat.Attacking && combat.WantToAttack)
                {
                    // starting attack
                    combat.Attacking = true;
                    combat.StartedAttack = true;
                }
                else
                {
                    // not starting
                    combat.StartedAttack = false;
                }

                if (combat.Attacking)
                {
                    combat.FrameTimer += deltaTime;
                    if (combat.FrameTimer >= combat.TimePerFrame)
                    {
                        combat.FrameTimer -= combat.TimePerFrame;
                        combat.Frame++;
                        if (combat.Frame >= combat.DecayFrame)
                        {
                            combat.Frame = 0;
                            combat.FrameTimer = 0;
                            combat.Attacking = false;
                        }
                        else if (combat.Frame == combat.DamageFrame)
                        {
                            var entitiesInRange = entities.Where(e => entity.id != e.id && ((Movement)e.GetComponent(typeof(Movement))).DistanceTo(movement) <= combat.AttackRange);
                            foreach (var target in entitiesInRange)
                            {
                                var targetMovement = (Movement)target.GetComponent(typeof(Movement));
                                var targetStats = (Stats)target.GetComponent(typeof(Stats));

                                var angleToTarget = AngleHelper.AngleTo(movement.X, movement.Y, targetMovement.X, targetMovement.Y);
                                if (AngleHelper.IsInArc(angleToTarget, movement.LookDirection, combat.AttackAngle))
                                {
                                    targetStats.Damage(combat.AttackDamage);
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
}
