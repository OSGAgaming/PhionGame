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
using QueefCord.Core.Graphics;

namespace QueefCord.Content.Entities
{
    /// <summary>
    /// Would add paralax entity modifier, but
    /// </summary>
    public class ParalaxedSprite : Entity2D, IDraw
    {
        private Texture2D Texture;
        private int Loop = 1;
        private Vector2 Offset;
        private float Scale;
        private Vector2 Paralax;
        private float Depth;
        public ParalaxedSprite(string Texture, string Layer, Vector2 Paralax, int Loop, Vector2 Offset, float Scale = 1, float depth = 0)
        {
            this.Texture = Assets<Texture2D>.Get(Texture);
            this.Layer = Layer;
            this.Loop = Loop;
            this.Offset = Offset;
            this.Paralax = Paralax;
            this.Scale = Scale;
            Depth = depth;
        }

        public string Layer { get; }

        public void Draw(SpriteBatch sb)
        {
            CameraTransform cam = LayerHost.GetLayer(Layer).Camera;

            for(int i = -Loop; i < Loop + 1; i++)
            {
                float XParalax = (Texture.Width * i * Scale + cam.Transform.Position.X * Paralax.X);
                float YParalax = (cam.Transform.Position.Y * Paralax.Y);

                Vector2 pLax = new Vector2(XParalax, YParalax);

                sb.Draw(Texture, Offset + Vector2.One * pLax, Scale, Color.White, Depth);
            }
        }
    }
}
