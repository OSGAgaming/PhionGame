using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QueefCord.Content.Entities;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Graphics;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace QueefCord.Core.Entities
{
    public class CameraTransform : EntityCore, IUpdate
    {
        public Matrix TransformationMatrix { get; set; }

        public bool HasUpdated { get; set; }

        public float LeftBound => Renderer.BackBufferSize.X / (float)(2 * Transform.Scale.X);
        public float UpBound => Renderer.BackBufferSize.Y / (float)(2 * Transform.Scale.Y);

        public CameraTransform()
        {
            Transform.Scale = Vector2.One;
        }

        protected virtual void OnUpdateTransform(GameTime gameTime) { }

        protected virtual void TransformConfiguration()
        {
            TransformationMatrix =
            Matrix.CreateTranslation(new Vector3(-Transform.Position, 0)) *
            Matrix.CreateRotationZ(Transform.Rotation) *
            Matrix.CreateScale(GetScreenScale());
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (HasUpdated) return;

            OnUpdateTransform(gameTime);

            Transform.Position.X = Math.Max(Transform.Position.X, 0);
            Transform.Position.Y = Math.Max(Transform.Position.Y, 0);

            TransformConfiguration();

            HasUpdated = true;
        }

        public Vector3 GetScreenScale()
        {
            float scaleX = 1;
            float scaleY = 1;

            return new Vector3(scaleX * Transform.Scale.X, scaleY * Transform.Scale.Y, 1.0f);
        }
    }
}

