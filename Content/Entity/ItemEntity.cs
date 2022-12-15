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
    internal class ItemEntity<T> : KinematicEntity2D, IUpdate, IDraw where T : IStoreable, new()
    {
        public string Layer => "Default";

        public virtual IStoreable AssociatedItem => new T();

        private readonly int GracePeriod = 30;
        private int GraceTimer;

        public ItemEntity(Vector2 size = default, Vector2 pos = default)
        {
            if (AssociatedItem != null && size == default)
                Size = AssociatedItem.Icon.Bounds.Size.ToVector2();

            if (size != default) Size = size;

            AddMechanic(new EntityCollision(this, Size, true, true));
            GraceTimer = GracePeriod;
            Transform.Position = pos;
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (GraceTimer > 0) GraceTimer--;
        }

        public override void OnCollide(Entity2D entity)
        {
            if (entity is Player && GraceTimer == 0)
            {
                if (Inventory.AddItem(AssociatedItem))
                    SceneHolder.CurrentScene.RemoveEntity(this);
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(AssociatedItem.Icon, new RectangleF(Transform.Position, Size), Color.White);
        }
    }
}
