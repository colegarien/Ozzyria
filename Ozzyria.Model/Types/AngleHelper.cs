using System;

namespace Ozzyria.Model.Types
{
    // TODO possibly convert to a new "Angle" type + Extension methods? maybe..
    public class AngleHelper
    {
        public static float PiOverFour = (float)(Math.PI / 4.0);
        public static float Pi = (float)(Math.PI);
        public static float PiOverTwo = (float)(Math.PI / 2.0);
        public static float ThreePiOverFour = (float)((3.0 * Math.PI) / 4.0);
        public static float TwoPi = (float)(Math.PI * 2f);
        private static float OneEightyOverPi = -(float)(180.0 / Math.PI);

        public static float Clamp(float angle)
        {
            while (angle < 0)
                angle += TwoPi;

            while(angle > TwoPi)
                angle -= TwoPi;

            return angle;
        }

        public static float AngleTo(float originX, float originY, float targetX, float targetY)
        {
            return (float)Math.Atan2(targetX - originX, targetY - originY);
        }

        public static bool IsInArc(float angle, float baseAngle, float thresholdAngle)
        {
            var min = Clamp(baseAngle - thresholdAngle);
            var max = Clamp(baseAngle + thresholdAngle);
            var clampedAngle = Clamp(angle);

            return (min <= clampedAngle && clampedAngle <= min + (thresholdAngle * 2))
                || (max - (thresholdAngle * 2) <= clampedAngle && clampedAngle <= max);
        }

        public static float RadiansToDegrees(float radians)
        {
            return OneEightyOverPi * radians;
        }
    }
}
