using Ozzyria.Game.Components;
using Ozzyria.Game.ECS;
using System.Linq;

namespace Ozzyria.Game.Systems
{
    internal class ExperieneOrb : TickSystem
    {
        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 4;

        protected EntityQuery query;
        protected EntityQuery playerQuery;
        public ExperieneOrb()
        {
            query = new EntityQuery();
            query.And(typeof(ExperienceOrbThought));

            playerQuery = new EntityQuery().And(typeof(Movement), typeof(Stats), typeof(PlayerThought));
        }

        public override void Execute(float deltaTime, EntityContext context)
        {
            var entities = context.GetEntities(query);
            var players = context.GetEntities(playerQuery);
            foreach (var entity in entities)
            {
                var movement = (Movement)entity.GetComponent(typeof(Movement));
                var boost = (ExperienceBoost)entity.GetComponent(typeof(ExperienceBoost));

                if (boost.HasBeenAbsorbed)
                {
                    context.DestroyEntity(entity);
                    continue;
                }

                var closestPlayer = players
                    .OrderBy(e => movement.DistanceTo((Movement)e.GetComponent(typeof(Movement))))
                    .FirstOrDefault();
                if (closestPlayer == null)
                {
                    movement.SlowDown(deltaTime);
                    continue;
                }

                var playerMovement = (Movement)closestPlayer.GetComponent(typeof(Movement));
                var playerStats = (Stats)closestPlayer.GetComponent(typeof(Stats));

                var distance = movement.DistanceTo(playerMovement);
                if (distance > MAX_FOLLOW_DISTANCE)
                {
                    movement.SlowDown(deltaTime);
                    continue;
                }

                movement.SpeedUp(deltaTime);

                if (distance <= ABSORBTION_DISTANCE)
                {
                    boost.AbsorbInto(playerStats);
                    continue;
                }

                movement.TurnToward(deltaTime, playerMovement.X, playerMovement.Y);
                movement.Update(deltaTime);
            }
        }
    }
}
