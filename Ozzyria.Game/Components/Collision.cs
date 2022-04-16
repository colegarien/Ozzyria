using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;
using System;
using System.Numerics;

namespace Ozzyria.Game.Components
{
    public class Collision : Component
    {
        [Savable]
        public bool IsDynamic { get; set; } = true;

        public static CollisionResult CircleIntersectsCircle(BoundingCircle circle1, BoundingCircle circle2)
        {
            var movement = (Movement)circle1.Owner.GetComponent(typeof(Movement));
            var otherMovement = (Movement)circle2.Owner.GetComponent(typeof(Movement));

            var direction = Vector2.Normalize(new Vector2(movement.X - otherMovement.X, movement.Y - otherMovement.Y));
            var collisionResult = new CollisionResult
            {
                Collided = movement.DistanceTo(otherMovement) < circle1.Radius + circle2.Radius,
                NormalX = direction.X,
                NormalY = direction.Y,
                Depth = Math.Abs(movement.DistanceTo(otherMovement) - circle2.Radius - circle1.Radius)
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
            var movement = (Movement)box1.Owner.GetComponent(typeof(Movement));
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

            collisionResult.Depth = Math.Abs(collisionResult.NormalX != 0 ? distanceH : distanceV);
            return collisionResult;
        }

        public static CollisionResult CircleIntersectsBox(BoundingCircle circle, BoundingBox box)
        {
            var movement = (Movement)circle.Owner.GetComponent(typeof(Movement));

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

            var direction = Vector2.Normalize(new Vector2(movement.X - testX, movement.Y - testY));
            var depth = (float)(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)) - circle.Radius);
            var collisionResult = new CollisionResult
            {
                Collided = depth < 0,
                NormalX = direction.X,
                NormalY = direction.Y,
                Depth = Math.Abs(depth)
            };

            return collisionResult;
        }

        public static CollisionResult BoxIntersectsCircle(BoundingBox box, BoundingCircle circle)
        {
            var result = CircleIntersectsBox(circle, box);
            result.NormalX *= -1f;
            result.NormalY *= -1f;
            return result;
        }
    }
}
