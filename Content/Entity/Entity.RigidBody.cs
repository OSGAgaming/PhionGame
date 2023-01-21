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
    public class RigidBody : IEntityModifier
    {
        public float Gravity { get; set; }
        public Vector2 Drag { get; set; }
        public float Friction { get; set; }

        public Vector2 Velocity;
        public RigidBody(float Gravity, Vector2 Drag = default, float Friction = 1)
        {
            this.Gravity = Gravity;
            if (Drag == default) this.Drag = Vector2.One;
            else this.Drag = Drag;

            this.Friction = Friction;
        }

        public void Update(in EntityCore entity, GameTime gameTime)
        {
            Velocity.Y += Gravity;
            entity.Transform.Position += Velocity;

            if (entity.Has(out EntityCollision col))
            {
                if (col.IsColliding())
                {
                    Velocity *= Friction;
                    return;
                }
            }

            Velocity *= Drag;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
