using QueefCord.Content.UI;
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

namespace QueefCord.Content.Entities
{
    public enum PlayerState
    {
        Running,
        Mining,
        Attacking,
        Watering
    }

    public enum Direction
    {
        Down,
        Up,
        Left,
        Right
    }

    public class PlayerAnimation : Animation
    {
        public bool IsAttacking;
        public PlayerState PlayerAnimationState;
        public Direction Direction;

        public PlayerAnimation(int X, int Y, int W, int H, int S) : base(X, Y, W, H, S)
        {
        }

        public void HandleRunning(Entity entity)
        {
            Player player = entity as Player;

            //TODO: Not Hardcode
            float mag = player.rigidBody.Velocity.Length();
            int speed = AnimationSpeed + 3 - (int)(mag * 3);
            speed = Math.Max(3, speed);

            player.Get<PlayerAnimation>().FrameY = mag > 0.1f ? 4 + (int)Direction : (int)Direction;

            if (FrameCounterX % speed == 0)
                FrameX++;
        }

        public void HandleAttacking(Entity entity)
        {
            if (FrameCounterX % AnimationSpeed == 0)
                FrameX++;

            Player player = entity as Player;

            player.Get<PlayerAnimation>().FrameY = 12 + (int)Direction;

            if (FrameX % FrameCountX == 0 && FrameX != 0)
                PlayerAnimationState = PlayerState.Running;
        }

        public void HandleMining(Entity entity)
        {
            if (FrameCounterX % AnimationSpeed == 0)
                FrameX++;

            Player player = entity as Player;

            player.Get<PlayerAnimation>().FrameY = 16 + (int)Direction;

            if (FrameX % FrameCountX == 0 && FrameX != 0)
                PlayerAnimationState = PlayerState.Running;
        }

        public void HandleWatering(Entity entity)
        {
            if (FrameCounterX % AnimationSpeed == 0)
                FrameX++;

            Player player = entity as Player;

            player.Get<PlayerAnimation>().FrameY = 20 + (int)Direction;

            if (FrameX % FrameCountX == 0 && FrameX != 0)
                PlayerAnimationState = PlayerState.Running;
        }

        public Direction CalculateDirection(Vector2 v)
        {
            if (Math.Abs(v.X) > Math.Abs(v.Y))
            {
                if (v.X > 0)
                    return Direction = Direction.Right;
                else
                    return Direction = Direction.Left;
            }
            else
            {
                if (v.Y > 0)
                    return Direction = Direction.Down;
                else
                    return Direction = Direction.Up;
            }
        }

        public void TriggerAnimation(in Entity entity, PlayerState state)
        {
            if (PlayerAnimationState != PlayerState.Running) return;

            Player player = entity as Player;

            Vector2 off = Mouse.GetState().Position.ToVector2() - player.Transform.Position;

            Direction = CalculateDirection(off);
            PlayerAnimationState = state;

            FrameX = 0;
        }

        public override void Update(in EntityCore entity, GameTime gameTime)
        {
            Player player = entity as Player;

            if (PlayerAnimationState == PlayerState.Running)
                Direction = CalculateDirection(player.rigidBody.Velocity);

            switch (PlayerAnimationState)
            {
                case PlayerState.Attacking:
                    HandleAttacking(entity);
                    break;

                case PlayerState.Running:
                    HandleRunning(entity);
                    break;

                case PlayerState.Mining:
                    HandleMining(entity);
                    break;

                case PlayerState.Watering:
                    HandleWatering(entity);
                    break;
            }

            FrameCounterX++;
            FrameX %= FrameCountX;
        }
    }
}
