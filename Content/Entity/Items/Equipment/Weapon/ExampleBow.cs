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
    internal class ExampleBow : Weapon
    {
        public override Texture2D Icon => Assets<Texture2D>.Get("Textures/Equipment/Bow");
        public override string Id => "ExampleBow";
        public override string Tooltip => "Any arrows?";
        public override string HitboxID => "Weapon";
        public override int Damage => 7;
        public override bool HasHitbox => false;
        public override int UseTime => 20;

        public float animScale = 0;
        public float animScaleVel = 0;
        public float dist = 10;

        public override void OnDraw(SpriteBatch sb)
        {
            Vector2 NormalizedDist = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - Player.LocalPlayer.Center);

            sb.Draw(
                Icon, Player.LocalPlayer.Center + NormalizedDist * dist, Icon.Bounds,
                Color.White, NormalizedDist.ToRotation(), Icon.TextureCenter(), animScale, SpriteEffects.None, 0f);


            if (UseTimer > 0)
            {
                UseTimer--;
            }
        }

        public override void OnPassive()
        {
            animScaleVel += (0 - animScale) / 32f - animScaleVel / 7f;

            animScale += animScaleVel;
        }

        public override void ActiveAI()
        {
            animScaleVel += (1 - animScale) / 32f - animScaleVel / 7f;

            animScale += animScaleVel;
        }

        public override void UseAI()
        {
            Vector2 NormalizedDist = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - Player.LocalPlayer.Center);

            if (UseTimer == 0)
            {
                Arrow arrow = new Arrow();
                arrow.rigidBody.Velocity = NormalizedDist * 4;
                arrow.Size = new Vector2(10);
                arrow.Center = Player.LocalPlayer.Center + NormalizedDist * (dist + 2);
                arrow.rigidBody.Drag = Vector2.One;

                Projectile.SpawnProjectile(arrow);

                UseTimer = UseTime;
            }
        }
    }
}
