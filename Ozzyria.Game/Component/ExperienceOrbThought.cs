using Ozzyria.Game.Component.Attribute;
using System.Linq;

namespace Ozzyria.Game.Component
{
    [Options(Name = "ExperienceOrbThought")]
    public class ExperienceOrbThought : Thought
    {
        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 4;

        public override void Update(float deltaTime, EntityManager entityManager)
        {
            var movement = Owner.GetComponent<Movement>(ComponentType.Movement);
            var boost = Owner.GetComponent<ExperienceBoost>(ComponentType.ExperienceBoost);

            if (boost.HasBeenAbsorbed)
            {
                return;
            }

            var closestPlayer = entityManager.GetEntities()
                .Where(e => e.Id != Owner.Id && e.HasComponent(ComponentType.Movement) && e.HasComponent(ComponentType.Stats) && e.HasComponent(ComponentType.Thought) && e.GetComponent<Thought>(ComponentType.Thought) is PlayerThought)
                .OrderBy(e => movement.DistanceTo(e.GetComponent<Movement>(ComponentType.Movement)))
                .FirstOrDefault();
            if (closestPlayer == null)
            {
                movement.SlowDown(deltaTime);
                return;
            }

            var playerMovement = closestPlayer.GetComponent<Movement>(ComponentType.Movement);
            var playerStats = closestPlayer.GetComponent<Stats>(ComponentType.Stats);

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
