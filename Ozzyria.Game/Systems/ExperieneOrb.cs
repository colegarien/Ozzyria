using Ozzyria.Game.Components;
using Grecs;
using System.Linq;

namespace Ozzyria.Game.Systems
{
    internal class ExperieneOrb : TickSystem
    {
        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 6;

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
                var movement = entity.GetComponent<Movement>();
                var boost = entity.GetComponent<ExperienceBoost>();

                if (boost.HasBeenAbsorbed)
                {
                    context.DestroyEntity(entity);
                    continue;
                }

                var closestPlayer = players
                    .OrderBy(e => movement.DistanceTo(e.GetComponent<Movement>()))
                    .FirstOrDefault();
                if (closestPlayer == null)
                {
                    if (movement.IsMoving())
                    {
                        // slow down
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = false;
                        intent.MoveRight = false;
                        intent.MoveUp = false;
                        intent.MoveDown = false;
                        entity.AddComponent(intent);
                    }
                    continue;
                }

                var playerMovement = closestPlayer.GetComponent<Movement>();
                var playerStats = closestPlayer.GetComponent<Stats>();

                var distance = movement.DistanceTo(playerMovement);
                if (distance > MAX_FOLLOW_DISTANCE)
                {
                    if (movement.IsMoving())
                    {
                        // slow down
                        var intent = MovementIntent.GetInstance();
                        intent.MoveLeft = false;
                        intent.MoveRight = false;
                        intent.MoveUp = false;
                        intent.MoveDown = false;
                        entity.AddComponent(intent);
                    }
                    continue;
                }

                if (distance <= ABSORBTION_DISTANCE)
                {
                    boost.AbsorbInto(playerStats);
                    continue;
                }

                movement.TurnToward(deltaTime, playerMovement);
            }
        }
    }
}
