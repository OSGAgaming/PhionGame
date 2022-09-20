using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using QueefCord.Content.UI;

namespace QueefCord.Content.Entities
{
    public class PlayerMovement : IEntityModifier
    {
        public readonly float MovementSpeed = 0.12f;
        public readonly float JumpForce = 1.5f;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(in Entity entity, GameTime gameTime)
        {
            Player player = entity as Player;
            float mag = player.Velocity.Length();

            bool anyColliding = false;

            foreach (Collideable2D c in player.Get<EntityCollision>().CollisionBoxes)
                if (c.Colliding)  
                    anyColliding = true;
                


            if (GameInput.Instance["D"].IsDown())
                player.Velocity.X += MovementSpeed;       

            if (GameInput.Instance["A"].IsDown())
                player.Velocity.X -= MovementSpeed;          

            if (GameInput.Instance["W"].IsDown() && anyColliding)  
                player.Velocity.Y -= JumpForce;
            
        }
    }
}
