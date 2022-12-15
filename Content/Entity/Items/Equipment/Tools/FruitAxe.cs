using QueefCord.Content.UI;
using QueefCord.Core.Entities;
using QueefCord.Core.Entities.EntitySystems;
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

namespace QueefCord.Content.Entities
{
    internal class FruitAxe : Tool
    {
        public override Texture2D Icon => Assets<Texture2D>.Get("Textures/Equipment/FruitAxe");
        public override int MaxStack => 1;
        public override string Id => "FruitAxe";
        public override string Tooltip => "An fruity axe?";
        public override string HitboxID => "Axe";
        public override PlayerState Anim => PlayerState.Mining;

        public override void SetDefaults()
        {
            Recipie = new SlotInfo[2];

            AddRecepie<StoneAxe>(1);
            AddRecepie<Orange>(3);
        }
    }
}
