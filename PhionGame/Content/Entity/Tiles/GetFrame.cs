using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.Tiles;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Content.Entities
{
    internal static class Framing
    {
        //TODO: abstract
        public static Rectangle TileFrame(TileSet manager, int i, int j)
        {
            Tile?[,] tiles = manager.Tiles;
            int res = TileManager.frameResolution;

            Rectangle r(int a, int b) => new Rectangle(a * res, b * res, res, res);

            return r(0, 5);
        }
    }
}
