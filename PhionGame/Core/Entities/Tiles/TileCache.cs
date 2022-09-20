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
    }

    public partial class TileManager : IUpdate, ISerializable
    {
        public static void AddTileSets()
        {
            FieldInfo[] info = typeof(TileID).GetFields();

            foreach (FieldInfo field in info)
            {
                short id = (short)field.GetValue(null);
                if (!TileTextures.ContainsKey(id))
                {
                    TileTextures.Add(id, field.Name);
                    TileColors.Add(id, Assets<Texture2D>.Get($"Textures/Tiles/{TileTextures[id]}").GetValue().GetDominantColor());
                }
            }
        }
    }
}
