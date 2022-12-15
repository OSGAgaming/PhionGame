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
using QueefCord.Core.DataStructures;

namespace QueefCord.Content.Entities
{
    internal abstract class Tool : Item
    {
        public override int MaxStack => 1;

        public virtual int Range => 20;
        public virtual int Size => 20;
        public virtual int HitLag => 16;
        public virtual string HitboxID => "Tool";

        public virtual PlayerState Anim { get; }
        public virtual bool HasHitbox { get; set; } = true;

        public virtual void ActiveAI() { }
        public virtual void UseAI() { }

        private int HitLagCounter;
        private bool Hitting;

        public override void OnActive()
        {
            ActiveAI();

            if (!HasHitbox)
                return;

            if (Hitting && HitLagCounter > 0)
                HitLagCounter--;

            if (HitLagCounter == 0 && Hitting)
            {
                Hitting = false;
                Vector2 NormalizedDist = Vector2.Normalize(Utils.DefaultMouseWorld - Player.LocalPlayer.Center);
                Vector2 Pos = Player.LocalPlayer.Center + NormalizedDist * Range;

                Collideable2D toolHitbox = new Collideable2D(null, new RectangleF(Pos - new Vector2(Size) / 2, new Vector2(Size)));
                toolHitbox.id = HitboxID;

                SceneHolder.CurrentScene.GetSystem<AABBCollisionSystem>().GenerateHitbox(toolHitbox);
            }
        }

        public override void OnUse()
        {
            UseAI();

            if (!HasHitbox)
                return;

            Hitting = true;
            HitLagCounter = HitLag;

            Player.LocalPlayer.Get<PlayerAnimation>().TriggerAnimation(Player.LocalPlayer, Anim);
        }
    }
}
