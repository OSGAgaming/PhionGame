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
    public class Animation : IEntityModifier
    {
        public int FrameCountX;
        public int FrameCountY;

        public int FrameWidth;
        public int FrameHeight;

        public int FrameX;
        public int FrameY;
        protected int FrameCounterX;
        protected int FrameCounterY;

        public int AnimationSpeed;

        public Rectangle Frame => new Rectangle(FrameX * FrameWidth, FrameY * FrameHeight, FrameWidth, FrameHeight);

        public Animation(int X, int Y, int W, int H, int S)
        {
            FrameCountX = X;
            FrameCountY = Y;

            FrameWidth = W;
            FrameHeight = H;

            AnimationSpeed = S;
        }

        public void Animate(int Y, int AnimSpeed, int XLimit)
        {
            FrameY = Y;
            FrameCountX++;
            AnimationSpeed = AnimSpeed;
            if (FrameCountX % AnimationSpeed == 0)
                FrameX++;

            FrameX %= XLimit;
        }

        public virtual void Dispose() { }

        public virtual void Update(in EntityCore entity, GameTime gameTime) { }
    }
}
