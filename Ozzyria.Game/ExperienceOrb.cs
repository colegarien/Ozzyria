using Ozzyria.Game.Component;
using System;
using System.Linq;

namespace Ozzyria.Game
{
    public class ExperienceOrb
    {
        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 4;

        public Movement Movement { get; set; } = new Movement { ACCELERATION = 200f, MAX_SPEED = 300f };

        public int Experience { get; set; } = 10;
        public bool HasBeenAbsorbed { get; set; } = false;

        public void Update(float deltaTime, Player[] players)
        {
            if (HasBeenAbsorbed)
            {
                return;
            }

            var closestPlayer = players.OrderBy(p => Math.Pow(p.Movement.X - Movement.X, 2) + Math.Pow(p.Movement.Y - Movement.Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                Movement.SlowDown(deltaTime);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.Movement.X - Movement.X, 2) + Math.Pow(closestPlayer.Movement.Y - Movement.Y, 2));
            if(distance > MAX_FOLLOW_DISTANCE)
            {
                Movement.SlowDown(deltaTime);
                return;
            }

            Movement.SpeedUp(deltaTime);

            if (distance <= ABSORBTION_DISTANCE)
            {
                closestPlayer.AddExperience(Experience);
                HasBeenAbsorbed = true;
                return;
            }

            Movement.TurnToward(deltaTime, closestPlayer.Movement.X, closestPlayer.Movement.Y);
            Movement.Update(deltaTime);
        }

    }
}
