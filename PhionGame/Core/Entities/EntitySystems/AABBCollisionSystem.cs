using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using QueefCord.Core.Entities;
using System.Diagnostics;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Helpers;
using QueefCord.Content.UI;
using QueefCord.Content.Entities;

namespace QueefCord.Core.Entities.EntitySystems
{
    public class AABBCollisionSystem : EntitySystem<Collideable2D>
    {
        public List<Collideable2D> RuntimeHitboxes = new List<Collideable2D>();
        public List<Collideable2D> AllHitboxes = new List<Collideable2D>();

        public void GenerateHitbox(Collideable2D e) => RuntimeHitboxes.Add(e);

        public override void Update(GameTime gameTime)
        {
            List<Collideable2D> Concat = new List<Collideable2D>(Entities);
            Concat.AddRange(RuntimeHitboxes);

            foreach (Collideable2D e in AllHitboxes.ToArray())
                e.Colliding = false;

            //for now n^2 cause too lazy for octree rn
            //just wanna test
            foreach (Collideable2D e in Concat.ToArray())
            {
                foreach (Collideable2D e2 in Concat.ToArray())
                {
                    if (!e.Equals(e2))
                        if (e.CanCollide && e2.CanCollide)
                            Compare(e, e2);

                }
            }

            AllHitboxes = Concat;
            RuntimeHitboxes.Clear();
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Collideable2D e in AllHitboxes.ToArray())
                Utils.DrawRectangle(e.CollisionFrame, Color.Red, 2);            
        }

        public void Compare(Collideable2D Ke, Collideable2D Ke2)
        {
            if (Ke.Compare(Ke2))
            {
                Ke.Collide(Ke2);
            }
        }
    }
}
