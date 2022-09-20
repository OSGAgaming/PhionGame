using QueefCord.Content.Maths;
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
    public class ExampleEnemy : NPC
    {
        public Entity2D Target;
        public float KnockBack;

        private int ArrowTimer;

        public override void SetDefaults()
        {
            Size = new Vector2(32);
            MaxHealth = 12;
            Health = 12;
            Friendly = false;
            Damage = 1;
            KnockBack = 2f;

            AddMechanic(new Animation(8, 8, 32, 32, 1));
        }
        public override void OnDeath(GameTime gameTime)
        {
            for (int i = 0; i < 3; i++)
                SceneHolder.CurrentScene.AddEntity(new ItemEntity<MilkItem>()
                {
                    Transform = new Transform(Center),
                    Size = new Vector2(20),
                    Velocity = new Vector2((float)Rand.random.NextDouble() * 2 - 1, (float)Rand.random.NextDouble() * 2 - 1)
                });
        }

        public override void OnDraw(SpriteBatch sb)
        {
            Color color = Color.Lerp(Color.IndianRed, Color.White, 1 - Get<BaseNPCStats>().ImmunityFrames / 30f);

            sb.Draw(Assets<Texture2D>.Get("Textures/NPCs/GreenCow"), 
                Transform.Position, Get<Animation>().Frame, color, 0f,
                Vector2.Zero, Vector2.One, Velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }

        public override void AI()
        {
            IList<IUpdate> updateables = SceneHolder.CurrentScene.Updateables;
            Animation a = Get<Animation>();

            if (Target == null)
            {
                foreach (IUpdate update in updateables)
                    if (update is Player p)
                    {
                        if (Vector2.DistanceSquared(p.Center, Center) < 100 * 100)
                            Target = p;
                    }

                //TODO: Helper Method
                a.Animate(4, 8, 3);
            }
            else
            {
                Vector2 lookAt = Vector2.Normalize(Target.Center - Center);
                Velocity += lookAt * 0.05f;

                a.Animate(1, 8, 7);


                ArrowTimer++;

                if (ArrowTimer % 120 == 0)
                {
                    Vector2 NormalizedDist = Vector2.Normalize(Target.Center - Center);

                    HostileArrow arrow = new HostileArrow();
                    arrow.Velocity = NormalizedDist * 3;
                    arrow.Size = new Vector2(10);
                    arrow.Center = Center + NormalizedDist * (15 + 2);
                    arrow.Friction = Vector2.One;

                    Projectile.SpawnProjectile(arrow);
                }
            }

            
        }

        public override void CollideAI(Entity2D entity)
        {
            Vector2 lookAt = Vector2.Normalize(Target.Center - Center);

            if (entity is KinematicEntity2D k) k.Velocity += lookAt * KnockBack;
        }
    }
}
