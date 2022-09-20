using Microsoft.Xna.Framework;
using System;

namespace QueefCord.Core.Helpers
{
    public static class Time
    {
        public static GameTime Current;
        public static double DeltaTime(this GameTime gt) => gt.ElapsedGameTime.TotalSeconds;
    }
}
