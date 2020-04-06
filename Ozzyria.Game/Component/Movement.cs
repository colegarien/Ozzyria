using Ozzyria.Game.Utility;
using System;

namespace Ozzyria.Game.Component
{
    public class Movement
    {
        public float ACCELERATION = 200f;
        public float MAX_SPEED = 100f;
        public float TURN_SPEED = 5f;

        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Speed { get; set; } = 0f;
        public float MoveDirection { get; set; } = 0f;
        public float LookDirection { get; set; } = 0f;
        
        public void TurnToward(float deltaTime, float targetX, float targetY)
        {
            LookDirection = AngleHelper.AngleTo(X, Y, targetX, targetY);
        }

        public void TurnLeft(float deltaTime)
        {
            LookDirection = AngleHelper.Clamp(LookDirection + (TURN_SPEED * deltaTime));
        }

        public void TurnRight(float deltaTime)
        {
            LookDirection = AngleHelper.Clamp(LookDirection - (TURN_SPEED * deltaTime));
        }

        public void MoveForward(float deltaTime)
        {
            MoveDirection = 0f;
        }

        public void MoveForwardRight(float deltaTime)
        { 
            MoveDirection = -AngleHelper.PiOverFour;
        }

        public void MoveRight(float deltaTime)
        {
            MoveDirection = -AngleHelper.PiOverTwo;
        }

        public void MoveBackwardRight(float deltaTime)
        {
            MoveDirection = -AngleHelper.ThreePiOverFour;
        }

        public void MoveBackward(float deltaTime)
        {
            MoveDirection = AngleHelper.Pi;
        }

        public void MoveBackwardLeft(float deltaTime)
        {
            MoveDirection = AngleHelper.ThreePiOverFour;
        }

        public void MoveLeft(float deltaTime)
        {
            MoveDirection = AngleHelper.PiOverTwo;
        }

        public void MoveForwardLeft(float deltaTime)
        {
            MoveDirection = AngleHelper.PiOverFour;
        }

        public void SlowDown(float deltaTime)
        {
            if (Speed == 0)
            {
                return;
            }

            Speed -= ACCELERATION * deltaTime;
            if (Speed < 0.0f)
            {
                Speed = 0.0f;
            }
        }

        public void SpeedUp(float deltaTime)
        {
            if (Speed == MAX_SPEED)
            {
                return;
            }

            Speed += ACCELERATION * deltaTime;
            if (Speed > MAX_SPEED)
            {
                Speed = MAX_SPEED;
            }
        }

        public void Update(float deltaTime)
        {
            X += Speed * deltaTime * (float)Math.Sin(LookDirection + MoveDirection);
            Y += Speed * deltaTime * (float)Math.Cos(LookDirection + MoveDirection);
        }
    }
}
