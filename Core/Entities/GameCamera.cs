using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QueefCord.Content.Entities;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using Microsoft.Xna.Framework;

namespace QueefCord.Core.Entities
{
    public class EntityFocalCamera : CameraTransform
    {
        public Entity entity;
        public float Smoothing => 16f;
        public float TargetScale { get; set; } = 1;

        Vector2 SmoothPosition;
        protected override void OnUpdateTransform(GameTime gameTime)
        {
            Vector2 position = Player.LocalPlayer.Center - Renderer.BackBufferSize.ToVector2() / (2 * Transform.Scale);

            SmoothPosition = Transform.Position.ReciprocateTo(position, Smoothing);
            Transform.Position = new Vector2((int)SmoothPosition.X, (int)SmoothPosition.Y);

            Transform.Scale += (Vector2.One * TargetScale - Transform.Scale) / Smoothing;
        }
    }
}

