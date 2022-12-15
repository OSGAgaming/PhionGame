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

namespace QueefCord.Content.Entities
{
    public class NPC : KinematicEntity2D, IUpdate, IDraw
    {
        public string Layer => "Default";

        public virtual int MaxHealth 
        { 
            get => Get<BaseNPCStats>().MaxHealth; 
            set => Get<BaseNPCStats>().MaxHealth = value; 
        }

        public virtual int Health
        {
            get => Get<BaseNPCStats>().Health;
            set => Get<BaseNPCStats>().Health = value;
        }

        public virtual bool Friendly { get; set; }
        public virtual int Damage { get; set; }


        public NPC()
        {
            AddMechanic(new BaseNPCStats(0));
            SetDefaults();
            AddMechanic(new EntityCollision(this, Size, true, true));
        }

        public virtual void SetDefaults() { }
        public virtual void AI() { }
        public virtual void OnDeath(GameTime gameTime) { }
        public virtual void CollideAI(Entity2D entity) { }
        public virtual void OnDraw(SpriteBatch sb) { }

        public override void OnCollide(Entity2D entity)
        {
            if(entity.id == "Weapon")
            {
                IStoreable storeable = UIScreenManager.Instance.GetScreen<Inventory>().ActiveHotbarItem;

                if (storeable is Weapon weapon)
                {
                    Vector2 NormalizedDist = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - Player.LocalPlayer.Center);
                    Velocity += NormalizedDist * weapon.KnockBack;

                    Get<BaseNPCStats>().TakeDamage(weapon.Damage);
                }
            }

            if (!Friendly)
            {
                if (entity is Player p)
                {
                    CollideAI(entity);

                    p.Get<BaseNPCStats>().TakeDamage(Damage);
                }
            }
        }

        public override void OnUpdate(GameTime gameTime)
        {
            AI();
        }

        public virtual void Draw(SpriteBatch sb)
        {
            OnDraw(sb);

            int outerPad = 8;
            int height = 5;
            float perc = Health / (float)MaxHealth;

            int width = (int)((Box.width + outerPad * 2) * (perc));

            Rectangle health = new Rectangle((int)Box.x - outerPad, (int)Box.bottom + 1, width, height);
            Color color = Color.Lerp(Color.IndianRed, Color.White, 1 - Get<BaseNPCStats>().ImmunityFrames / 30f);

            Utils.DrawBoxFill(health, Color.IndianRed, 0);
        }
    }
}
