using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.Utility;
using System;
using Grecs;

namespace Ozzyria.Game.Components
{
    public class Movement : Component
    {
        private float _previousX = 0f;
        private float _previousY = 0f;
        private int _layer = 1;
        private float _x = 0f;
        private float _y = 0f;
        private float _collisionOffsetY = 0f;
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
        public float CollisionOffsetY
        {
            get => _collisionOffsetY; set
            {
                if (_collisionOffsetY != value)
                {
                    _collisionOffsetY = value;
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
            return (float)Math.Sqrt(Math.Pow(target.X - X, 2) + Math.Pow((target.Y + target.CollisionOffsetY) - (Y + CollisionOffsetY), 2));
        }

        public Direction GetLookDirection()
        {
            Direction direction;
            if (LookDirection <= 0.78 || LookDirection >= 5.48)
            {
                direction = Direction.Down;
            }
            else if (LookDirection > 0.78 && LookDirection < 2.36)
            {
                direction = Direction.Right;
            }
            else if (LookDirection >= 2.36 && LookDirection <= 3.94)
            {
                direction = Direction.Up;
            }
            else
            {
                direction = Direction.Left;
            }

            return direction;
        }

        public void TurnToward(float deltaTime, Movement otherMovement)
        {
            var x = X;
            var y = Y + CollisionOffsetY;
            var otherX = otherMovement.X;
            var otherY = otherMovement.Y + otherMovement.CollisionOffsetY;

            var intent = MovementIntent.GetInstance();
            intent.MoveLeft = otherX + 1 < x;
            intent.MoveRight = otherX - 1 > x;
            intent.MoveUp = otherY + 1f < y;
            intent.MoveDown = otherY - 1f > y;
            Owner.AddComponent(intent);

        }

        public void MoveUp(float deltaTime)
        {
            LookDirection = AngleHelper.Pi;
            MoveDirection = 0f;
        }

        public void MoveUpRight(float deltaTime)
        {
            LookDirection = AngleHelper.Pi;
            MoveDirection = -AngleHelper.PiOverFour;
        }

        public void MoveRight(float deltaTime)
        {
            LookDirection = AngleHelper.PiOverTwo;
            MoveDirection = 0f;
        }

        public void MoveDownRight(float deltaTime)
        {
            LookDirection = 0;
            MoveDirection = AngleHelper.PiOverFour;
        }

        public void MoveDown(float deltaTime)
        {
            LookDirection = 0f;
            MoveDirection = 0f;
        }

        public void MoveDownLeft(float deltaTime)
        {
            LookDirection = 0;
            MoveDirection = -AngleHelper.PiOverFour;
        }

        public void MoveLeft(float deltaTime)
        {
            LookDirection = -AngleHelper.PiOverTwo;
            MoveDirection = 0f;
        }

        public void MoveUpLeft(float deltaTime)
        {
            LookDirection = AngleHelper.Pi;
            MoveDirection = AngleHelper.PiOverFour;
        }

        public bool IsMoving()
        {
            return Speed != 0;
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
