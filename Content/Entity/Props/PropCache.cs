using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using QueefCord.Core.Tiles;
using QueefCord.Core.Resources;

namespace QueefCord.Core.Tiles
{
    public partial class PropManager
    {
        public void LoadProps()
        {
            AddPropType("Rock", Assets<Texture2D>.Get("Textures/Props/ExampleProp"));
            AddPropType("OrangeTree", Assets<Texture2D>.Get("Textures/Props/OrangeTree"));
            AddPropType("OrangeCrop", Assets<Texture2D>.Get("Textures/Props/OrangeCrop"));
            AddPropType("PumpkinCrop", Assets<Texture2D>.Get("Textures/Props/PumpkinCrop"));
        }
    }
}
