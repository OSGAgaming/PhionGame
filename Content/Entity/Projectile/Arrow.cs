using QueefCord.Content.UI;
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
    public class Arrow : Projectile, IDraw
    {
        public override int Damage => 4;
        public override float KnockBack => 2;

        public override int TimeAlive => 120;
        public string Layer => "Default";

        public RigidBody rigidBody;

        public override void OnSpawn()
        {
            AddMechanic(new RigidBody(0));
            rigidBody = Get<RigidBody>();
        }

        public void Draw(SpriteBatch sb)
        {
            float rot = rigidBody.Velocity.ToRotation();

            sb.Draw(Assets<Texture2D>.Get("Textures/Projectiles/Arrow"), Center, Color.White, rot);
        }
        public override void OnUpdate(GameTime gameTime)
        {
            TimeLeft++;

            if (TimeLeft >= TimeAlive)
                SceneHolder.CurrentScene.RemoveEntity(this);
        }
        public override void OnCollide(Entity2D entity)
        {
            if (entity is NPC n)
            {
                Vector2 lookAt = Vector2.Normalize(entity.Center - Center);
                n.Get<RigidBody>().Velocity += lookAt * KnockBack;

                entity.Get<BaseNPCStats>().TakeDamage(Damage);
                SceneHolder.CurrentScene.RemoveEntity(this);
            }
        }
    }
}
