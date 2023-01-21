using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace QueefCord.Content.Maths
{
    public static class Rand
    {
        public static Random random;

        static Rand() => random = new Random();

        //insert utils or something
        public static float NextFloat(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("min cannot be greater than max.");
            }

            return min + (float)random.NextDouble() * (max - min);
        }
        public static float NextFloat(float max = 1)
        {
            return (float)(random.NextDouble() * max);
        }

        public static Vector2 NextVec2(float min = 1, float max = 1) => new Vector2(NextFloat(min, max), NextFloat(min, max));


        
    }
}