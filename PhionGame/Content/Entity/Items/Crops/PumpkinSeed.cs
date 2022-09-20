using QueefCord.Content.UI;
using QueefCord.Core.Entities;
using QueefCord.Core.Entities.EntitySystems;
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
    public class PumpkinSeed : Item
    {
        public override Texture2D Icon => Assets<Texture2D>.Get("Textures/Items/PumpkinSeed");
        public override int MaxStack => 16;
        public override string Id => "PumpkinSeed";
        public override string Tooltip => "Use these to plant Pumpkins";

        public override void OnUse()
        {
            Vector2 mP = Utils.DefaultMouseWorld.Snap(TileManager.frameResolution);

            Prop Crop = new Prop("PumpkinCrop", "Default");
            Crop.Transform.Position = mP;
            Crop.Size = new Vector2(16, 16);

            SceneHolder.CurrentScene.AddEntity(Crop);

            Inventory.RemoveItems<PumpkinSeed>(1);
        }
    }
}