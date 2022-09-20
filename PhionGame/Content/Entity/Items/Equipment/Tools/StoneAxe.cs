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
    internal class StoneAxe : Tool
    {
        public override Texture2D Icon => Assets<Texture2D>.Get("Textures/Equipment/StoneAxe");
        public override int MaxStack => 1;
        public override string Id => "StoneAxe";
        public override string Tooltip => "An axe?";
        public override string HitboxID => "Axe";
        public override PlayerState Anim => PlayerState.Mining;
    }
}
