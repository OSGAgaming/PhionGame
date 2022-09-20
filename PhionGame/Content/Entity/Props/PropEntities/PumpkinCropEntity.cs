using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using QueefCord.Core.Tiles;
using QueefCord.Core.Resources;
using QueefCord.Core.Entities;
using QueefCord.Core.Interfaces;
using QueefCord.Core.UI;
using QueefCord.Content.UI;
using QueefCord.Core.Scenes;
using QueefCord.Content.Entities;
using QueefCord.Core.DataStructures;
using QueefCord.Content.Maths;

namespace QueefCord.Core.Tiles
{
    public class PumpkinCropEntity : CropEntity<Pumpkin>
    {
        public override string Prop => "PumpkinCrop";
    }
}
