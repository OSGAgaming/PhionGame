using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Core.Tiles
{
    public struct Tile
    {
        public short id;
        public bool solid;
        public bool Active;
        public byte Outline;
        public byte Top;
    }

    public struct Space
    {
        public Color TileColor;
        public Color? LightSources;
    }
}
