using QueefCord.Core.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace QueefCord.Core.Helpers
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 pos, Color c, float rot) => sb.Draw(tex, pos, tex.Bounds, c, rot, tex.TextureCenter(), 1, SpriteEffects.None, 0f);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Rectangle r, Rectangle s, Color c, float depth) => sb.Draw(tex, r, s, c, 0, Vector2.Zero, SpriteEffects.None, depth);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Rectangle r, Color c, float depth) => sb.Draw(tex, r, tex.Bounds, c, 0, Vector2.Zero, SpriteEffects.None, depth);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 v, Rectangle s, Color c, float depth) => sb.Draw(tex, v, s, c, 0, Vector2.Zero, 1f, SpriteEffects.None, depth);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 v, float depth) => sb.Draw(tex, v, tex.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, depth);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 pos, Color c, Vector2 origin) => sb.Draw(tex, pos, tex.Bounds, c, 0, origin, 1, SpriteEffects.None, 0f);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 pos, float Scale, Color c) => sb.Draw(tex, pos, tex.Bounds, c, 0, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 pos, Vector2 scale, Color c) => sb.Draw(tex, pos, tex.Bounds, c, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        public static void Draw(this SpriteBatch sb, Texture2D tex, Vector2 pos, float Scale, Color c, float depth) => sb.Draw(tex, pos, tex.Bounds, c, 0, Vector2.Zero, Scale, SpriteEffects.None, depth);

    }
}
