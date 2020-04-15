using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzyria.Game.Component
{
    public class ExperienceOrbThought : IThought
    {
        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 4;

        public override void Update(float deltaTime, Player[] players, Dictionary<int, Entity> entities)
        {
            var movement = (Movement)Owner.Components[typeof(Movement)];
            var boost = (ExperienceBoost)Owner.Components[typeof(ExperienceBoost)];

            if (boost.HasBeenAbsorbed)
            {
                return;
            }

            var closestPlayer = players.OrderBy(p => Math.Pow(p.Movement.X - movement.X, 2) + Math.Pow(p.Movement.Y - movement.Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                movement.SlowDown(deltaTime);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.Movement.X - movement.X, 2) + Math.Pow(closestPlayer.Movement.Y - movement.Y, 2));
            if (distance > MAX_FOLLOW_DISTANCE)
            {
                movement.SlowDown(deltaTime);
                return;
            }

            movement.SpeedUp(deltaTime);

            if (distance <= ABSORBTION_DISTANCE)
            {
                boost.AbsorbInto(closestPlayer);
                return;
            }

            movement.TurnToward(deltaTime, closestPlayer.Movement.X, closestPlayer.Movement.Y);
            movement.Update(deltaTime);
        }
    }
}
