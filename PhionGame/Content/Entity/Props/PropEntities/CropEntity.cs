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
    public abstract class CropEntity<T> : PropEntity where T : IStoreable, new()
    {
        public float TicksNeededToGrow = 1000;
        public float Growth;
        public float GrowthRate = 0.5f;
        public float GrowthAccel = 0f;
        public float GrowthDeccel = 0.999f;

        public CropEntity()
        {
            AddMechanic(new Animation(5, 1, 16, 16, 3));
        }

        public override bool Draw(SpriteBatch sb, Prop prop)
        {
            sb.Draw(Texture, prop.Transform.Position, Get<Animation>().Frame, Color.White);

            return true;
        }

        public override void Update(Prop prop)
        {
            if (Growth < TicksNeededToGrow)
                Growth += GrowthRate * (0.1f + GrowthAccel);

            float perc = Growth / TicksNeededToGrow;

            if (perc < 1 / 4f)
                Get<Animation>().FrameX = 0;
            else if (perc < 2 / 4f)
                Get<Animation>().FrameX = 1;
            else if (perc < 3 / 4f)
                Get<Animation>().FrameX = 2;
            else if (perc >= 1)
                Get<Animation>().FrameX = 3;

            GrowthAccel *= GrowthDeccel;
        }

        public override void OnCollide(Prop prop, Entity2D col)
        {
            if (col.id == "Hoe")
            {
                if (Growth >= TicksNeededToGrow)
                {

                    for (int i = 0; i < 3; i++)
                        SceneHolder.CurrentScene.AddEntity(new ItemEntity<T>()
                        {
                            Transform = new Transform(prop.Center),
                            Size = new Vector2(20),
                            Velocity = new Vector2((float)Rand.random.NextDouble() * 2 - 1, (float)Rand.random.NextDouble() * 2 - 1)
                        });
                }

                Growth = 0;
                GrowthAccel = 0;
            }

            if (col.id == "WateringCan")
            {
                GrowthAccel = 1;
            }
        }
    }
}
