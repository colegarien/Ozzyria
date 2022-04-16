using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.Utility;
using System;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    [Options(Name = "Movement")]
    public class Movement : Component
    {

        public float ACCELERATION { get; set; } = 200f;
        public float MAX_SPEED { get; set; } = 100f;
        public float TURN_SPEED { get; set; } = 5f;

        [Savable]
        public float PreviousX { get; set; } = 0f;
        [Savable]
        public float PreviousY { get; set; } = 0f;
        [Savable]
        public int Layer { get; set; } = 1; // TODO OZ-24 experiment with things on multiple layers
        [Savable]
        public float X { get; set; } = 0f;
        [Savable]
        public float Y { get; set; } = 0f;
        [Savable]
        public float Speed { get; set; } = 0f;
        [Savable]
        public float MoveDirection { get; set; } = 0f;
        [Savable]
        public float LookDirection { get; set; } = 0f;
        

        public float DistanceTo(Movement target)
        {
            return (float)Math.Sqrt(Math.Pow(target.X - X, 2) + Math.Pow(target.Y - Y, 2));
        }


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
            PreviousX = X;
            PreviousY = Y;
            X += Speed * deltaTime * (float)Math.Sin(LookDirection + MoveDirection);
            Y += Speed * deltaTime * (float)Math.Cos(LookDirection + MoveDirection);
        }
    }
}
