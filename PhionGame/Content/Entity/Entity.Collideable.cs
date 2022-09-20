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
    public class EntityCollision : IEntityModifier
    {
        public List<Collideable2D> CollisionBoxes = new List<Collideable2D>();
        public Entity2D Parent;

        public void AddCollisionBox(Rectangle rect, bool Static = false, bool Trigger = false) => CollisionBoxes.Add(new Collideable2D(Parent, rect, Static, Trigger));

        public EntityCollision(Entity2D e)
        {
            Parent = e;
        }

        public EntityCollision(Entity2D e, Rectangle r, bool Static = false, bool Trigger = false)
        {
            Parent = e;
            AddCollisionBox(r, Static, Trigger);
        }

        public EntityCollision(Entity2D e, Vector2 size, bool Static = false, bool Trigger = false)
        {
            Parent = e;
            AddCollisionBox(new RectangleF(0, 0, size.X, size.Y), Static, Trigger);
        }

        public void Update(in Entity entity, GameTime gameTime)
        {
            foreach (Collideable2D c in CollisionBoxes)
            {
                c.Update(gameTime);
                SceneHolder.CurrentScene.GetSystem<AABBCollisionSystem>().GenerateHitbox(c);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
