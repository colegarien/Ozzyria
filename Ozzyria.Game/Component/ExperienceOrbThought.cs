using Ozzyria.Game.Component.Attribute;
using Ozzyria.Game.ECS;
using System.Linq;

namespace Ozzyria.Game.Component
{
    [Options(Name = "ExperienceOrbThought")]
    public class ExperienceOrbThought : Thought
    {
        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 4;

        public override void Update(float deltaTime, EntityContext context)
        {
            var movement = (Movement)Owner.GetComponent(typeof(Movement));
            var boost = (ExperienceBoost)Owner.GetComponent(typeof(ExperienceBoost));

            if (boost.HasBeenAbsorbed)
            {
                return;
            }

            var closestPlayer = context.GetEntities()
                .Where(e => e.id != Owner.id && e.HasComponent(typeof(Movement)) && e.HasComponent(typeof(Stats)) && e.HasComponent(typeof(PlayerThought)))
                .OrderBy(e => movement.DistanceTo((Movement)e.GetComponent(typeof(Movement))))
                .FirstOrDefault();
            if (closestPlayer == null)
            {
                movement.SlowDown(deltaTime);
                return;
            }

            var playerMovement = (Movement)closestPlayer.GetComponent(typeof(Movement));
            var playerStats = (Stats)closestPlayer.GetComponent(typeof(Stats));

            var distance = movement.DistanceTo(playerMovement);
            if (distance > MAX_FOLLOW_DISTANCE)
            {
                movement.SlowDown(deltaTime);
                return;
            }

            movement.SpeedUp(deltaTime);

            if (distance <= ABSORBTION_DISTANCE)
            {
                boost.AbsorbInto(playerStats);
                return;
            }

            movement.TurnToward(deltaTime, playerMovement.X, playerMovement.Y);
            movement.Update(deltaTime);
        }
    }
}
