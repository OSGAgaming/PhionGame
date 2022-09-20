using Microsoft.Xna.Framework;
using System;

namespace QueefCord.Core.Helpers
{
    public static partial class Utils
    {
        public static string AssetDirectory => Environment.ExpandEnvironmentVariables($@"{MainDirectory}\Content\Textures");
        public static string MainDirectory => Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory}");
        public static string LocalDirectory => Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory.Split(@"\bin")[0]}");
        public static string WorldPath => MainDirectory + $@"\Content\Levels\";
        public static string LocalWorldPath => LocalDirectory + $@"\Content\Levels\";
    }
}
