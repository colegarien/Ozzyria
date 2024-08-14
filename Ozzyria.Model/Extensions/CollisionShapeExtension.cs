using Ozzyria.Model.Types;
using System.Numerics;

namespace Ozzyria.Model.Extensions
{
    public static class CollisionShapeExtension
    {
        public static CollisionResult CheckCollision(this CollisionShape shape, float shapeX, float shapeY, float shapePrevX, float shapePrevY, CollisionShape other, float otherX, float otherY)
        {
            var shapeHasBox = shape.BoundingBox != null && shape.BoundingBox.Width > 0 && shape.BoundingBox.Height > 0;
            var shapeHasCircle = shape.BoundingCircle != null && shape.BoundingCircle.Radius > 0;
            var otherHasBox = other.BoundingBox != null && other.BoundingBox.Width > 0 && other.BoundingBox.Height > 0;
            var otherHasCircle = other.BoundingCircle != null && other.BoundingCircle.Radius > 0;

            if ((!shapeHasBox && !shapeHasCircle) || (!otherHasBox && !otherHasCircle))
            {
                // one or both are missing boudning shapes
                return new CollisionResult { Collided = false };
            }

            if (shapeHasCircle && otherHasCircle)
            {
                return shape.BoundingCircle.Intersects(shapeX, shapeY, other.BoundingCircle, otherX, otherY);
            }
            else if (shapeHasBox && otherHasBox)
            {
                return shape.BoundingBox.Intersects(shapeX, shapeY, shapePrevX, shapePrevY, other.BoundingBox, otherX, otherY);
            }
            else if (shapeHasCircle && otherHasBox)
            {
                return shape.BoundingCircle.Intersects(shapeX, shapeY, other.BoundingBox, otherX, otherY);
            }
            else if (shapeHasBox && otherHasCircle)
            {
                return shape.BoundingBox.Intersects(shapeX, shapeY, other.BoundingCircle, otherX, otherY);
            }

            return new CollisionResult { Collided = false };
        }
        public static CollisionResult Intersects(this BoundingCircle circle, float circleX, float circleY, BoundingCircle other, float otherX, float otherY)
        {
            var direction = Vector2.Normalize(new Vector2(circleX - otherX, circleY - otherY));
            var distance = MathF.Sqrt(((otherX - circleX) * (otherX - circleX)) + ((otherY - circleY) * (otherY - circleY)));
            var collisionResult = new CollisionResult
            {
                Collided = distance < circle.Radius + other.Radius,
                NormalX = direction.X,
                NormalY = direction.Y,
                Depth = Math.Abs(distance - other.Radius - circle.Radius)
            };

            return collisionResult;
        }

        public static CollisionResult Intersects(this BoundingCircle circle, float circleX, float circleY, BoundingBox other, float otherX, float otherY)
        {
            var boxLeft = otherX - (other.Width / 2f);
            var boxRight = otherX + (other.Width / 2f);
            var boxTop = otherY - (other.Height / 2f);
            var boxBottom = otherY + (other.Height / 2f);

            var testX = circleX;
            var testY = circleY;

            if (circleX < boxLeft)
                testX = boxLeft;
            else if (circleX > boxRight)
                testX = boxRight;

            if (circleY < boxTop)
                testY = boxTop;
            else if (circleY > boxBottom)
                testY = boxBottom;

            float deltaX = circleX - testX;
            float deltaY = circleY - testY;

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

        public static CollisionResult Intersects(this BoundingBox box, float boxX, float boxY, float boxPrevX, float boxPrevY, BoundingBox other, float otherX, float otherY)
        {
            var boxLeft = boxX - (box.Width / 2f);
            var boxRight = boxX + (box.Width / 2f);
            var boxTop = boxY - (box.Height / 2f);
            var boxBottom = boxY + (box.Height / 2f);

            var otherLeft = otherX - (other.Width / 2f);
            var otherRight = otherX + (other.Width / 2f);
            var otherTop = otherY - (other.Height / 2f);
            var otherBottom = otherY + (other.Height / 2f);

            var collisionOnRight = (boxLeft <= otherLeft && otherLeft <= boxRight);
            var collisionOnLeft = (boxLeft <= otherRight && otherRight <= boxRight);
            var collisionOnBottom = (boxTop <= otherTop && otherTop <= boxBottom);
            var collisionOnTop = (boxTop <= otherBottom && otherBottom <= boxBottom);

            var collisionResult = new CollisionResult
            {
                Collided = (collisionOnRight || collisionOnLeft) && (collisionOnTop || collisionOnBottom),
                NormalX = 0,
                NormalY = 0
            };

            // Determine Correct Normal
            var dx = boxX - boxPrevX;
            var dy = boxY - boxPrevY;

            var movingTowardH = (collisionOnRight && dx > 0) || (collisionOnLeft && dx < 0);
            var movingTowardV = (collisionOnBottom && dy > 0) || (collisionOnTop && dy < 0);

            var distanceH = collisionOnRight ? boxRight - otherLeft : otherRight - boxLeft;
            var distanceV = collisionOnBottom ? boxBottom - otherTop : otherBottom - boxTop;
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

        public static CollisionResult Intersects(this BoundingBox box, float boxX, float boxY, BoundingCircle other, float otherX, float otherY)
        {
            var result = other.Intersects(otherX, otherY, box, boxX, boxY);
            result.NormalX *= -1f;
            result.NormalY *= -1f;
            return result;
        }
    }
}
