using QueefCord.Core.DataStructures;
using QueefCord.Core.Graphics;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace QueefCord.Core.Helpers
{
    public static partial class Utils
    {
        public static Color GetPixel(this Color[] colors, int x, int y, int width)
        {
            return colors[x + (y * width)];
        }
        public static Color[] GetPixels(this Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            return colors1D;
        }

        public static Color GetPixel(this Texture2D texture, int x, int y)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
          
            return colors1D.GetPixel(x, y, texture.Width);
        }

        public static Color GetDominantColor(this Texture2D bmp)
        {
            //Used for tally
            long r = 0;
            long g = 0;
            long b = 0;

            int total = 0;

            Color[] c = bmp.GetPixels();

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = c.GetPixel(x, y, bmp.Width);

                    if (clr == Color.Transparent)
                        continue;

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return new Color(r /255f, g / 255f, b / 255f);
        }
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);

        public static void DrawPixel(Vector2 pos, Color tint) => Renderer.Spritebatch.Draw(Assets<Texture2D>.Get("pixel"), pos, tint);

        public static void DrawBoxFill(Vector2 pos, int width, int height, Color tint) => Renderer.Spritebatch.Draw(Assets<Texture2D>.Get("pixel"), pos, new Rectangle(0, 0, width, height), tint);

        public static void DrawText(string text, Color colour, Vector2 position, float rotation = 0f, float scale = 1f, float layerDepth = 0f)
        {
            SpriteFont font = Assets<SpriteFont>.Get("Fonts/Arial");
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Renderer.Spritebatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }

        public static float DrawTextToLeft(string text, Color colour, Vector2 position, float scale = 1f, float layerDepth = 0f)
        {
            SpriteFont font = Assets<SpriteFont>.Get("Fonts/Arial");
            float textPositionLeft = position.X;
            Renderer.Spritebatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            return font.MeasureString(text).X;
        }

        public static void DrawBoxFill(Rectangle rectangle, Color tint, float depth = 0f) =>
            Renderer.Spritebatch.Draw(Assets<Texture2D>.Get("pixel"), rectangle.Location.ToVector2(), new Rectangle(0, 0, rectangle.Width, rectangle.Height), 
            tint, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f, float depth = 0f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            Renderer.Spritebatch.Draw(Assets<Texture2D>.Get("pixel"), p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, depth);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            sb.Draw(Assets<Texture2D>.Get("pixel"), p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public static void DrawSquare(Vector2 point, float size, Color color)
        {
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X, point.Y + size), color);
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X + size, point.Y), color);
            DrawLine(point, new Vector2(point.X, point.Y + size), color);
            DrawLine(point, new Vector2(point.X + size, point.Y), color);
        }

        public static void DrawRectangle(Vector2 point, float sizeX, float sizeY, Color color, float thickness = 1)
        {
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }

        public static void DrawRectangle(Rectangle rectangle, Color color = default, float thickness = 1)
        {
            if (color == default) color = Color.White;

            Vector2 point = rectangle.Location.ToVector2();
            int sizeX = rectangle.Size.X;
            int sizeY = rectangle.Size.Y;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }

        public static void DrawRectangle(SpriteBatch sb, Rectangle rectangle, Color color, float thickness = 1)
        {
            Vector2 point = rectangle.Location.ToVector2();
            int sizeX = rectangle.Size.X;
            int sizeY = rectangle.Size.Y;
            DrawLine(sb, new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(sb, new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(sb, point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(sb, point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
        public static void DrawRectangle(RectangleF rectangle, Color color = default, float thickness = 1)
        {
            if (color == default) color = Color.White;

            Vector2 point = rectangle.TL;
            float sizeX = rectangle.width;
            float sizeY = rectangle.height;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
    }
}
