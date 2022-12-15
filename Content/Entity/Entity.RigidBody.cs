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
        private float Gravity { get; set; }
        private float Resistance { get; set; }

        public Vector2 Velocity;
        public RigidBody(float Gravity, float Resistance = 1)
        {
            this.Gravity = Gravity;
            this.Resistance = Resistance;
        }

        public void Update(in Entity entity, GameTime gameTime)
        {
            Velocity.Y += Gravity;
            entity.Transform.Position += Velocity;

            Velocity *= Resistance;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
