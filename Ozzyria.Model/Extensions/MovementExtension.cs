using Ozzyria.Model.Components;
using Ozzyria.Model.Types;

namespace Ozzyria.Model.Extensions
{
    public static class MovementExtension
    {

        public static float DistanceTo(this Movement movement, Movement target)
        {
            return (float)Math.Sqrt(Math.Pow(target.X - movement.X, 2) + Math.Pow((target.Y + target.CollisionOffsetY) - (movement.Y + movement.CollisionOffsetY), 2));
        }

        public static Direction GetLookDirection(this Movement movement)
        {
            Direction direction;
            if (movement.LookDirection <= 0.78 || movement.LookDirection >= 5.48)
            {
                direction = Direction.Down;
            }
            else if (movement.LookDirection > 0.78 && movement.LookDirection < 2.36)
            {
                direction = Direction.Right;
            }
            else if (movement.LookDirection >= 2.36 && movement.LookDirection <= 3.94)
            {
                direction = Direction.Up;
            }
            else
            {
                direction = Direction.Left;
            }

            return direction;
        }

        public static void TurnToward(this Movement movement, float deltaTime, Movement otherMovement)
        {
            var x = movement.X;
            var y = movement.Y + movement.CollisionOffsetY;
            var otherX = otherMovement.X;
            var otherY = otherMovement.Y + otherMovement.CollisionOffsetY;

            var intent = MovementIntent.GetInstance();
            intent.MoveLeft = otherX + 1 < x;
            intent.MoveRight = otherX - 1 > x;
            intent.MoveUp = otherY + 1f < y;
            intent.MoveDown = otherY - 1f > y;
            movement.Owner.AddComponent(intent);
        }

        public static void MoveUp(this Movement movement, float deltaTime)
        {
            movement.LookDirection = AngleHelper.Pi;
            movement.MoveDirection = 0f;
        }

        public static void MoveUpRight(this Movement movement, float deltaTime)
        {
            movement.LookDirection = AngleHelper.Pi;
            movement.MoveDirection = AngleHelper.Clamp(-AngleHelper.PiOverFour);
        }

        public static void MoveRight(this Movement movement, float deltaTime)
        {
            movement.LookDirection = AngleHelper.PiOverTwo;
            movement.MoveDirection = 0f;
        }

        public static void MoveDownRight(this Movement movement, float deltaTime)
        {
            movement.LookDirection = 0;
            movement.MoveDirection = AngleHelper.PiOverFour;
        }

        public static void MoveDown(this Movement movement, float deltaTime)
        {
            movement.LookDirection = 0f;
            movement.MoveDirection = 0f;
        }

        public static void MoveDownLeft(this Movement movement, float deltaTime)
        {
            movement.LookDirection = 0;
            movement.MoveDirection = AngleHelper.Clamp(-AngleHelper.PiOverFour);
        }

        public static void MoveLeft(this Movement movement, float deltaTime)
        {
            movement.LookDirection = AngleHelper.Clamp(-AngleHelper.PiOverTwo);
            movement.MoveDirection = 0f;
        }

        public static void MoveUpLeft(this Movement movement, float deltaTime)
        {
            movement.LookDirection = AngleHelper.Pi;
            movement.MoveDirection = AngleHelper.PiOverFour;
        }

        public static bool IsMoving(this Movement movement)
        {
            return movement.Speed != 0;
        }

        public static void SlowDown(this Movement movement, float deltaTime)
        {
            if (movement.Speed == 0)
            {
                return;
            }

            movement.Speed -= movement.ACCELERATION * deltaTime;
            if (movement.Speed < 0.0f)
            {
                movement.Speed = 0.0f;
            }
        }

        public static void SpeedUp(this Movement movement, float deltaTime)
        {
            if (movement.Speed == movement.MAX_SPEED)
            {
                return;
            }

            movement.Speed += movement.ACCELERATION * deltaTime;
            if (movement.Speed > movement.MAX_SPEED)
            {
                movement.Speed = movement.MAX_SPEED;
            }
        }

        public static void Update(this Movement movement, float deltaTime)
        {
            movement.PreviousX = movement.X;
            movement.PreviousY = movement.Y;
            movement.X += movement.Speed * deltaTime * (float)Math.Sin(movement.LookDirection + movement.MoveDirection);
            movement.Y += movement.Speed * deltaTime * (float)Math.Cos(movement.LookDirection + movement.MoveDirection);
        }

        public static CollisionResult CheckCollision(this Movement movement, Movement otherMovement)
        {
            if (movement.CollisionShape == null || otherMovement.CollisionShape == null)
            {
                return new CollisionResult { Collided = false };
            }

            return movement.CollisionShape.CheckCollision(movement.X, movement.Y, movement.PreviousX, movement.PreviousY, otherMovement.CollisionShape, otherMovement.X, otherMovement.Y);
        }
    }
}
