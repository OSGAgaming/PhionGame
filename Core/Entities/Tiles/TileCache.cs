using QueefCord.Content.Entities;
using QueefCord.Content.Maths;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.IO;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace QueefCord.Core.Tiles
{
    public static class TileID
    {
        public const short TestTile = 0;
        public const short RockTile = 1;
    }

    public partial class TileManager : IUpdate
    {
        public static Dictionary<int, string> TileTexturePaths = new Dictionary<int, string>();
        public static Dictionary<int, Texture2D> TileTextures = new Dictionary<int, Texture2D>();
        public static Dictionary<int, Texture2D> TileOutlines = new Dictionary<int, Texture2D>();
        public static Dictionary<int, Texture2D> TileTop = new Dictionary<int, Texture2D>();
        public static Dictionary<int, Color> TileColors = new Dictionary<int, Color>();

        public static void AddTileSets()
        {
            FieldInfo[] info = typeof(TileID).GetFields();

            foreach (FieldInfo field in info)
            {
                short id = (short)field.GetValue(null);
                if (!TileTexturePaths.ContainsKey(id))
                {
                    TileTexturePaths.Add(id, field.Name);
                    TileTextures.Add(id, Assets<Texture2D>.Get($"Textures/Tiles/{TileTexturePaths[id]}Loop").GetValue());

                    if (Assets<Texture2D>.Has($"Textures/Tiles/{TileTexturePaths[id]}Outline"))
                        TileOutlines.Add(id, Assets<Texture2D>.Get($"Textures/Tiles/{TileTexturePaths[id]}Outline").GetValue());
                    if (Assets<Texture2D>.Has($"Textures/Tiles/{TileTexturePaths[id]}Top"))
                        TileTop.Add(id, Assets<Texture2D>.Get($"Textures/Tiles/{TileTexturePaths[id]}Top").GetValue());

                    TileColors.Add(id, Assets<Texture2D>.Get($"Textures/Tiles/{TileTexturePaths[id]}").GetValue().GetDominantColor());
                }
            }
        }
    }
}
