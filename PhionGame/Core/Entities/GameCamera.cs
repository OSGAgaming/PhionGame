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

        protected override void OnUpdateTransform(GameTime gameTime)
        {           
            Transform.Position = 
                Transform.Position.ReciprocateTo(Player.LocalPlayer.Center -
                Renderer.BackBufferSize.ToVector2() / (2 * Transform.Scale), Smoothing);         
        }
    }
}

