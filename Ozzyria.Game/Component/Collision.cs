using System;

namespace Ozzyria.Game.Component
{
    public class Collision : Component
    {
        public override ComponentType Type() => ComponentType.Collision;

        public static bool CircleIntersectsCircle(BoundingCircle circle1, BoundingCircle circle2)
        {
            var movement = circle1.Owner.GetComponent<Movement>(ComponentType.Movement);
            var otherMovement = circle2.Owner.GetComponent<Movement>(ComponentType.Movement);

            return movement.DistanceTo(otherMovement) < circle1.Radius + circle2.Radius;
        } 

        public static bool BoxIntersectsBox(BoundingBox box1, BoundingBox box2)
        {
            var horizontalIntersection = (box1.GetLeft() <= box2.GetLeft() && box2.GetLeft() <= box1.GetRight())
                || (box1.GetLeft() <= box2.GetRight() && box2.GetRight() <= box1.GetRight());
            var verticalIntersection = (box1.GetTop() <= box2.GetTop() && box2.GetTop() <= box1.GetBottom())
                || (box1.GetTop() <= box2.GetBottom() && box2.GetBottom() <= box1.GetBottom());

            return horizontalIntersection && verticalIntersection;
        }

        public static bool CircleIntersectsBox(BoundingCircle circle, BoundingBox box)
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
            return Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)) <= circle.Radius;
        }
    }
}
