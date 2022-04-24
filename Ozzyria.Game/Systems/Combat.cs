using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
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
                if (combat.Attacking)
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
