﻿using System;

namespace Ozzyria.Game.Utility
{
    public class RandomHelper
    {
        private static Random random;

        /// <summary>
        /// Generate Random float greater than or equal to min and less than max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Random(float min, float max)
        {
            LazyInitializeRandom();
            return (float)(random.NextDouble() * (max - min)) + min;
        }

        /// <summary>
        /// Generate Random int greater than or equal to min and less than max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            LazyInitializeRandom();
            return (int)Math.Round(Random((float)min, (float)max));
        }

        private static void LazyInitializeRandom()
        {
            if(random == null)
            {
                random = new Random();
            }
        }
    }
}
