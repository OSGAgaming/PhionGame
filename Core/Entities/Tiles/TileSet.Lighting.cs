using QueefCord.Content.Entities;
using QueefCord.Content.Maths;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Entities.EntitySystems;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace QueefCord.Core.Tiles
{
    public partial class TileSet : IDraw, IMappable
    {
        public Color[,] TileColor { get; set; }
        public Color?[,] LightSources { get; set; }

        private float DiffusionRate { get; set; } = 0.7f;

        public void SetColor(int i, int j, Color c) => TileColor[i, j] = c;

        public void ResetLights()
        {
            for (int i = 0; i < TileColor.GetLength(0); i++)
            {
                for (int j = 0; j < TileColor.GetLength(1); j++)
                {
                    TileColor[i, j] = Color.Black;
                }
            }
        }

        public void UpdateAmbientLight()
        {
            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / TileManager.drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

            int w = TileManager.width;
            int h = TileManager.height;

            for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
            {
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                {
                    if (Tiles[i, j] == null) LightSources[i, j] = Color.White;
                    else LightSources[i, j] = Color.Black;
                }
            }
        }

        public void UpdateAmbientLight(int a, int b, int radius)
        {
            int w = TileManager.width;
            int h = TileManager.height;

            int r = radius / 2;

            for (int i = Math.Max(a - r, 0); i < Math.Min(a + r, w); i++)
            {
                for (int j = Math.Max(b - r, 0); j < Math.Min(b + radius, h); j++)
                {
                    if (Tiles[i, j] == null) LightSources[i, j] = Color.White;
                    else LightSources[i, j] = Color.Black;
                }
            }
        }

        public void EmitLight(int i, int j, float intensity, int radius, int iteration = 0)
        {
            if (iteration < radius)
            {
                SetColor(i, j, Color.White * intensity);

                for (int k = i - 1; k <= i + 1; k++)
                {
                    for (int l = j - 1; l <= j + 1; l++)
                    {
                        if (k != i || l != j)
                        {
                            float dist = Vector2.Distance(new Vector2(k, l), new Vector2(i, j));
                            float newIntensity = (float)Math.Pow(DiffusionRate, dist) * intensity;
                            if (k >= 0 && l >= 0)
                            {
                                if (Tiles[k, l] != null && TileColor[k, l].R / 255f < newIntensity)
                                {
                                    EmitLight(k, l, newIntensity, radius, iteration + 1);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateSunLighting(int radius)
        {
            UpdateAmbientLight();

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / TileManager.drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

            int w = TileManager.width;
            int h = TileManager.height;

            for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
            {
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                {
                    SetColor(i, j, Color.Black);
                }
            }

            for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
            {
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                {
                    if (LightSources[i, j].Value.R > 0)
                    {
                        EmitLight(i, j, LightSources[i, j].Value.R / 255f, radius);
                    }

                }
            }
        }

        public void UpdateSunLighting(int a, int b, int radius)
        {
            UpdateAmbientLight(a, b, radius);

            int w = TileManager.width;
            int h = TileManager.height;
            int r = radius;

            for (int i = Math.Max(a - r, 0); i < Math.Min(a + r, w); i++)
            {
                for (int j = Math.Max(b - r, 0); j < Math.Min(b + r, h); j++)
                {
                    if (Tiles[i, j] != null)
                         SetColor(i, j, Color.Black);
                }
            }

            for (int i = Math.Max(a - r * 2, 0); i < Math.Min(a + r * 2, w); i++)
            {
                for (int j = Math.Max(b - r * 2, 0); j < Math.Min(b + r * 2, h); j++)
                {
                    if (LightSources[i, j] == null) continue;

                    if (LightSources[i, j].Value.R > 0)
                    {
                        EmitLight(i, j, LightSources[i, j].Value.R / 255f, radius);
                    }

                }
            }
        }

        //====================================DRAW LOGIC====================================
        public void UpdateLightBuffer()
        {
            int res = TileManager.drawResolution;
            CameraTransform Camera = LayerHost.GetLayer(Layer).Camera;
            LayerHost.GetLayer("Default").MapHost.Maps.Get("TileLightingMap").CanDraw = true;

            LayerHost.GetLayer("Default").MapHost.Maps.Get("TileLightingMap").DrawToBatchedTarget
                 ((sb) =>
                 {
                     Point TL = (Camera.Transform.Position / TileManager.drawResolution).ToPoint();
                     Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

                     int w = TileManager.width;
                     int h = TileManager.height;

                     Utils.DrawBoxFill(new Rectangle(
                         Math.Max(TL.X - 1, 0),
                         Math.Max(TL.Y - 1, 0),
                         Math.Min(TL.X + BR.X + 1, w),
                         Math.Min(TL.Y + BR.Y + 1, h)), Color.White, 1f);

                     for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                         for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                         {
                             if (TileColor[i, j] != Color.White)
                             {
                                 Rectangle r = new Rectangle(i, j, 1, 1);
                                 Utils.DrawBoxFill(r, TileColor[i, j], Solid ? 0.99f : 1f);
                             }
                         }
                 });


        }

        public void ApplyLighting()
        {
            int res = TileManager.drawResolution;

            CameraTransform Camera = LayerHost.GetLayer(Layer).Camera;
            Vector2 delta = Camera.Transform.Position - Camera.LastTransform.Position;
            Vector2 remainder = new Vector2(
                (Camera.Transform.Position.X + Math.Abs(delta.X)) % res,
                (Camera.Transform.Position.Y + Math.Abs(delta.Y)) % res);

            //if (remainder.X == 15) remainder.X = 0;
            //if (remainder.Y == 15) remainder.Y = 0;
            UpdateLightBuffer();


            LayerHost.GetLayer("Default").MapHost.Maps.Get("UpscaledTileLightingMap").DrawToBatchedTarget
            ((sb) =>
            {
                Texture2D tex = LayerHost.GetLayer("Default").MapHost.Maps.Get("TileLightingMap").MapTarget;
                sb.Draw(tex, Renderer.BackBufferBounds, Color.White);
            });
        }
    }
}

