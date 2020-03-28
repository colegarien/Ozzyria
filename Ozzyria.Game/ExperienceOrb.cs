using System;
using System.Linq;

namespace Ozzyria.Game
{
    public class ExperienceOrb
    {
        const float ACCELERATION = 200f;
        const float MAX_SPEED = 300f;

        const float MAX_FOLLOW_DISTANCE = 200;
        const float ABSORBTION_DISTANCE = 4;


        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;

        public int Experience { get; set; } = 10;
        public bool HasBeenAbsorbed { get; set; } = false;

        public void Update(float deltaTime, Player[] players)
        {
            if (HasBeenAbsorbed)
            {
                return;
            }

            var closestPlayer = players.OrderBy(p => Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2)).FirstOrDefault();
            if (closestPlayer == null)
            {
                SlowDown(deltaTime);
                return;
            }

            var distance = Math.Sqrt(Math.Pow(closestPlayer.X - X, 2) + Math.Pow(closestPlayer.Y - Y, 2));
            if(distance > MAX_FOLLOW_DISTANCE)
            {
                SlowDown(deltaTime);
                return;
            }

            SpeedUp(deltaTime);

            if (distance <= ABSORBTION_DISTANCE)
            {
                closestPlayer.AddExperience(Experience);
                HasBeenAbsorbed = true;
                return;
            }

            // Move toward player
            X += Speed * deltaTime * (float)((closestPlayer.X - X) / distance);
            Y += Speed * deltaTime * (float)((closestPlayer.Y - Y) / distance);
        }

        private void SlowDown(float deltaTime)
        {
            if(Speed == 0)
            {
                return;
            }

            Speed -= ACCELERATION * deltaTime;
            if (Speed < 0.0f)
            {
                Speed = 0.0f;
            }
        }

        private void SpeedUp(float deltaTime)
        {
            if(Speed == MAX_SPEED)
            {
                return;
            }

            Speed += ACCELERATION * deltaTime;
            if (Speed > MAX_SPEED)
            {
                Speed = MAX_SPEED;
            }
        }
    }
}
