using Ozzyria.Game.Components.Attribute;
using Grecs;
using System;
using System.Numerics;

namespace Ozzyria.Game.Components
{
    public class Collision : Component
    {
        private Movement _ownerMovement = null;
        protected Movement OwnerMovement {
            get {
                if(_ownerMovement == null || Owner.id != _ownerMovement.Owner.id)
                {
                    _ownerMovement = Owner.GetComponent<Movement>();
                }

                return _ownerMovement;
            }
            set {
                // noop
            }
        }

        protected float X
        {
            get
            {
                return (OwnerMovement?.X ?? 0);
            }
            set
            {
                // noop
            }
        }
        protected float Y
        {
            get
            {
                return (OwnerMovement?.Y ?? 0) + (OwnerMovement?.CollisionOffsetY ?? 0);
            }
            set
            {
                // noop
            }
        }

        protected bool _isDynamic = true;
        [Savable]
        public bool IsDynamic { get => _isDynamic; set
            {
                if (_isDynamic != value)
                {
                    _isDynamic = value;
                    Owner?.TriggerComponentChanged(this);
                }
            }
        }

        public static CollisionResult CircleIntersectsCircle(BoundingCircle circle1, BoundingCircle circle2)
        {
            var direction = Vector2.Normalize(new Vector2(circle1.X - circle2.X, circle1.Y - circle2.Y));
            var distance = MathF.Sqrt(((circle2.X - circle1.X) * (circle2.X - circle1.X)) + ((circle2.Y - circle1.Y) * (circle2.Y - circle1.Y)));
            var collisionResult = new CollisionResult
            {
                Collided = distance < circle1.Radius + circle2.Radius,
                NormalX = direction.X,
                NormalY = direction.Y,
                Depth = Math.Abs(distance - circle2.Radius - circle1.Radius)
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
            var movement = box1.OwnerMovement; 
            var dx = movement.X - movement.PreviousX;
            var dy = movement.Y - movement.PreviousY;

            var movingTowardH = (collisionOnRight && dx > 0) || (collisionOnLeft && dx < 0);
            var movingTowardV = (collisionOnBottom && dy > 0) || (collisionOnTop && dy < 0);

            var distanceH = collisionOnRight ? box1.GetRight() - box2.GetLeft() : box2.GetRight() - box1.GetLeft();
            var distanceV = collisionOnBottom ? box1.GetBottom() - box2.GetTop() : box2.GetBottom() - box1.GetTop();
            if (Math.Abs(distanceH) < Math.Abs(distanceV) && movingTowardH)
            {
                collisionResult.NormalX = collisionOnLeft ? 1 : -1;
                collisionResult.NormalY = 0;
            }
            else if (movingTowardV)
            {
                collisionResult.NormalX = 0;
                collisionResult.NormalY = collisionOnTop ? 1 : -1;
            }

            collisionResult.Depth = Math.Abs(collisionResult.NormalX != 0 ? distanceH : distanceV);
            return collisionResult;
        }

        public static CollisionResult CircleIntersectsBox(BoundingCircle circle, BoundingBox box)
        {
            var testX = circle.X;
            var testY = circle.Y;

            if (circle.X < box.GetLeft())
                testX = box.GetLeft();
            else if (circle.X > box.GetRight())
                testX = box.GetRight();

            if (circle.Y < box.GetTop())
                testY = box.GetTop();
            else if (circle.Y > box.GetBottom())
                testY = box.GetBottom();

            float deltaX = circle.X - testX;
            float deltaY = circle.Y - testY;

            var direction = Vector2.Normalize(new Vector2(deltaX, deltaY));
            var depth = MathF.Sqrt((deltaX * deltaX) + (deltaY * deltaY)) - circle.Radius;
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
