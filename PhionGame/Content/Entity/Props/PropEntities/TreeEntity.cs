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
using QueefCord.Core.Helpers;

namespace QueefCord.Core.Tiles
{
    public class OrangeTreeEntity : PropEntity
    {
        public override string Prop => "OrangeTree";

        public int AxeResistance => 3;
        public int TreeStrength;

        public bool IsBeingMined;

        public OrangeTreeEntity()
        {
            TreeStrength = AxeResistance;
            AddMechanic(new Animation(12, 5, 48, 48, 3));
        }

        public override bool Draw(SpriteBatch sb, Prop prop)
        {
            sb.Draw(Texture, prop.Transform.Position, Get<Animation>().Frame, Color.White, 1 - prop.Center.Y / 100000f);

            return true;
        }

        public override void Update(Prop prop)
        {
            if (IsBeingMined)
            {
                Get<Animation>().FrameX++;
                Get<Animation>().FrameY = 2;
                if (Get<Animation>().FrameX == 5)
                {
                    IsBeingMined = false;
                    Get<Animation>().FrameY = 0;
                    Get<Animation>().FrameX = 0;
                }
            }
        }

        public override void OnCollide(Prop prop, Entity2D col)
        {
            if (col.id == "Axe" && !IsBeingMined)
            {
                TreeStrength--;
                IsBeingMined = true;
                //IStoreable storeable = UIScreenManager.Instance.GetScreen<Inventory>().ActiveHotbarItem;
                if (TreeStrength != 0)
                    return;

                SceneHolder.CurrentScene.RemoveEntity(prop);

                for (int i = 0; i < 3; i++)
                    SceneHolder.CurrentScene.AddEntity(new ItemEntity<LogWoodItem>()
                    {
                        Transform = new Transform(prop.Center),
                        Size = new Vector2(20),
                        Velocity = new Vector2((float)Rand.random.NextDouble() * 2 - 1, (float)Rand.random.NextDouble() * 2 - 1)
                    });
            }
        }
    }
}
