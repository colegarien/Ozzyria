using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.Utility;
using System;
using Ozzyria.Game.ECS;

namespace Ozzyria.Game.Components
{
    public class Movement : Component
    {
        private float _previousX = 0f;
        private float _previousY = 0f;
        private int _layer = 1;
        private float _x = 0f;
        private float _y = 0f;
        private float _speed = 0f;
        private float _moveDirection = 0f;
        private float _lookDirection = 0f;

        public float ACCELERATION { get; set; } = 200f;
        public float MAX_SPEED { get; set; } = 100f;
        public float TURN_SPEED { get; set; } = 5f;

        [Savable]
        public float PreviousX { get => _previousX; set
            {
                if (_previousX != value)
                {
                    _previousX = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float PreviousY { get => _previousY; set
            {
                if (_previousY != value)
                {
                    _previousY = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public int Layer { get => _layer; set
            {
                if (_layer != value)
                {
                    _layer = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float X { get => _x; set
            {
                if (_x != value)
                {
                    _x = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float Y { get => _y; set
            {
                if (_y != value)
                {
                    _y = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float Speed { get => _speed; set
            {
                if (_speed != value)
                {
                    _speed = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float MoveDirection { get => _moveDirection; set
            {
                if (_moveDirection != value)
                {
                    _moveDirection = AngleHelper.Clamp(value);
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }
        [Savable]
        public float LookDirection { get => _lookDirection; set
            {
                if (_lookDirection != value)
                {
                    _lookDirection = AngleHelper.Clamp(value);
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

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
            LookDirection += (TURN_SPEED * deltaTime);
        }

        public void TurnRight(float deltaTime)
        {
            LookDirection -= (TURN_SPEED * deltaTime);
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
