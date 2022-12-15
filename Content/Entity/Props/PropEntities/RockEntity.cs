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

namespace QueefCord.Core.Tiles
{
    public class RockEntity : PropEntity
    {
        public override string Prop => "Rock";
        
        public override void OnCollide(Prop prop, Entity2D col)
        {
            if (col.id == "Pickaxe")
            {
                SceneHolder.CurrentScene.RemoveEntity(prop);
                SceneHolder.CurrentScene.AddEntity(new ItemEntity<RockItem>()
                {
                    Transform = new Transform(prop.Transform.Position)
                });
            }
        }
    }
}
