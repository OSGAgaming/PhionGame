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
    public class CollisionSystem : EntitySystem<Collideable2D>
    {
        public HashSet<Collideable2D> StaticRuntimeHitboxes = new HashSet<Collideable2D>();
        public HashSet<Collideable2D> KinematicRuntimeHitboxes = new HashSet<Collideable2D>();

        public List<Collideable2D> AllHitboxes = new List<Collideable2D>();

        public void GenerateStaticHitbox(Collideable2D e)
        {
            StaticRuntimeHitboxes.Add(e);
        }
        public void GenerateKinematicHitbox(Collideable2D e) => KinematicRuntimeHitboxes.Add(e);

        public override void Update(GameTime gameTime)
        {
            List<Collideable2D> Concat = new List<Collideable2D>(Entities);

            Concat.AddRange(StaticRuntimeHitboxes);
            Concat.AddRange(KinematicRuntimeHitboxes);

            foreach (Collideable2D e in AllHitboxes.ToArray())
            {
                e.Colliding = false;
                e.CollisionInfo = new CollisionInfo(Vector2.Zero);
            }

            //for now n^2 cause too lazy for octree rn
            //just wanna test
            foreach (Collideable2D e in KinematicRuntimeHitboxes)
            {
                foreach (Collideable2D e2 in Concat.ToArray())
                {
                    if (!e.Equals(e2))
                        if (e.CanCollide && e2.CanCollide)
                            Compare(e, e2);

                }
            }

            AllHitboxes = Concat;

            foreach(Collideable2D c in AllHitboxes)
            {
                if(c.Has(out ChunkAllocator allocator))
                {
                    allocator.Dispose();
                }
            }

            StaticRuntimeHitboxes.Clear();
            KinematicRuntimeHitboxes.Clear();
        }

        public override void Draw(SpriteBatch sb)
        {
            //foreach (Collideable2D e in AllHitboxes.ToArray())
            //    Utils.DrawRectangle(e.CollisionFrame, Color.Red, 2);            
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
