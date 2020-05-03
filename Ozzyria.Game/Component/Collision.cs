using System;
using System.Numerics;

namespace Ozzyria.Game.Component
{
    public class Collision : Component
    {
        public override ComponentType Type() => ComponentType.Collision;

        public static CollisionResult CircleIntersectsCircle(BoundingCircle circle1, BoundingCircle circle2)
        {
            var movement = circle1.Owner.GetComponent<Movement>(ComponentType.Movement);
            var otherMovement = circle2.Owner.GetComponent<Movement>(ComponentType.Movement);

            var direction = Vector2.Normalize(new Vector2(movement.PreviousX - otherMovement.X, movement.PreviousY - otherMovement.Y));
            var collisionResult = new CollisionResult
            {
                Collided = movement.DistanceTo(otherMovement) < circle1.Radius + circle2.Radius,
                NormalX = direction.X,
                NormalY = direction.Y
            };

            return collisionResult;
        }

        public static CollisionResult BoxIntersectsBox(BoundingBox box1, BoundingBox box2)
        {
            var collisionOnRight = (box1.GetLeft() <= box2.GetLeft() && box2.GetLeft() <= box1.GetRight());
            var collisionOnLeft = (box1.GetLeft() <= box2.GetRight() && box2.GetRight() <= box1.GetRight());
            var collisionOnBottom = (box1.GetTop() <= box2.GetTop() && box2.GetTop() <= box1.GetBottom());
            var collisionOnTop = (box1.GetTop() <= box2.GetBottom() && box2.GetBottom() <= box1.GetBottom());

            var collisionResult = new CollisionResult
            {
                Collided = (collisionOnRight || collisionOnLeft) && (collisionOnTop || collisionOnBottom),
                NormalX = 0,
                NormalY = 0
            };

            // Determine Correct Normal
            var movement = box1.Owner.GetComponent<Movement>(ComponentType.Movement);
            var dx = movement.X - movement.PreviousX;
            var dy = movement.Y - movement.PreviousY;

            var movingTowardH = (collisionOnRight && dx > 0) || (collisionOnLeft && dx < 0);
            var movingTowardV = (collisionOnBottom && dy > 0) || (collisionOnTop && dy < 0);

            var distanceH = collisionOnRight ? box1.GetRight() - box2.GetLeft() : box2.GetRight() - box1.GetLeft();
            var distanceV = collisionOnBottom ? box1.GetBottom() - box2.GetTop() : box2.GetBottom() - box1.GetTop();
            if(Math.Abs(distanceH) < Math.Abs(distanceV) && movingTowardH)
            {
                collisionResult.NormalX = collisionOnLeft ? 1 : -1;
                collisionResult.NormalY = 0;
            }
            else if(movingTowardV)
            {
                collisionResult.NormalX = 0;
                collisionResult.NormalY = collisionOnTop ? 1 : -1;
            }

            return collisionResult;
        }

        public static CollisionResult CircleIntersectsBox(BoundingCircle circle, BoundingBox box)
        {
            var movement = circle.Owner.GetComponent<Movement>(ComponentType.Movement);

            var testX = movement.X;
            var testY = movement.Y;

            if (movement.X < box.GetLeft())
                testX = box.GetLeft();
            else if (movement.X > box.GetRight())
                testX = box.GetRight();

            if (movement.Y < box.GetTop())
                testY = box.GetTop();
            else if (movement.Y > box.GetBottom())
                testY = box.GetBottom();

            float deltaX = movement.X - testX;
            float deltaY = movement.Y - testY;

            var direction = Vector2.Normalize(new Vector2(movement.PreviousX - testX, movement.PreviousY - testY));
            var collisionResult = new CollisionResult
            {
                Collided = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)) <= circle.Radius,
                NormalX = direction.X,
                NormalY = direction.Y
            };

            return collisionResult;
        }
        public static CollisionResult BoxIntersectsCircle(BoundingBox box, BoundingCircle circle)
        {
            var circleMovement = circle.Owner.GetComponent<Movement>(ComponentType.Movement);

            var testX = circleMovement.X;
            var testY = circleMovement.Y;

            if (circleMovement.X < box.GetLeft())
                testX = box.GetLeft();
            else if (circleMovement.X > box.GetRight())
                testX = box.GetRight();

            if (circleMovement.Y < box.GetTop())
                testY = box.GetTop();
            else if (circleMovement.Y > box.GetBottom())
                testY = box.GetBottom();

            float deltaX = circleMovement.X - testX;
            float deltaY = circleMovement.Y - testY;


            var movement = box.Owner.GetComponent<Movement>(ComponentType.Movement);
            var direction = Vector2.Normalize(new Vector2(movement.PreviousX - testX, movement.PreviousY - testY));
            var collisionResult = new CollisionResult
            {
                Collided = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)) <= circle.Radius,
                NormalX = direction.X,
                NormalY = direction.Y
            };

            return collisionResult;
        }
    }
}
