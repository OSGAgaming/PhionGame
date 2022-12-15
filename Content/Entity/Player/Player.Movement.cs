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
using QueefCord.Content.Maths;

namespace QueefCord.Content.Entities
{
    public class PlayerMovement : IEntityModifier
    {
        public readonly float MovementSpeed = 0.15f;
        public readonly float JumpForce = 2.5f;

        private readonly int JumpMaxCooldown = 2;
        private int JumpCooldown;

        public PlayerMovement(float movementSpeed, float jumpForce)
        {
            MovementSpeed = movementSpeed;
            JumpForce = jumpForce;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(in Entity entity, GameTime gameTime)
        {
            Player player = entity as Player;

            bool collidingDown = false;

            foreach (Collideable2D c in player.Get<EntityCollision>().CollisionBoxes)
            {
                Logger.NewText(c.CollisionInfo.DirectionMask);

                if (c.Colliding && (c.CollisionInfo.DirectionMask & (byte)Core.Entities.Direction.Down) != 0)
                {
                    collidingDown = true;
                    break;
                }
            }


            if (GameInput.Instance["D"].IsDown())
                player.Velocity.X += MovementSpeed;       

            if (GameInput.Instance["A"].IsDown())
                player.Velocity.X -= MovementSpeed;

            if (GameInput.Instance["S"].IsDown())
                player.Velocity.Y += MovementSpeed;

            if (GameInput.Instance["W"].IsDown() && collidingDown && JumpCooldown == 0)
            {
                JumpCooldown = JumpMaxCooldown;
                player.Velocity.Y -= JumpForce;
            }

            if (JumpCooldown > 0) JumpCooldown--;
        }
    }
}
