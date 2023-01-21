using QueefCord.Content.Entities;
using QueefCord.Content.Maths;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.IO;
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
using System.Security.Cryptography;
using QueefCord.Core.Graphics;
using QueefCord.Core.Entities.EntitySystems;

namespace QueefCord.Core.Tiles
{
    public partial class TileManager : IUpdate
    {
        private const float DiffusionRate = 0.7f;
        private const int LightIntensity = 7;

        public void ResetLights()
        {
            for (int i = 0; i < ParentWorld.TileBounds.X; i++)
            {
                for (int j = 0; j < ParentWorld.TileBounds.Y; j++)
                {
                    SetTileColor(i, j, Color.Black);
                }
            }
        }

        public void UpdateAmbientLight(string ID, Rectangle bounds, int r)
        {
            for (int i = Math.Max(bounds.X - r, 0); i < Math.Min(bounds.Right + r, ParentWorld.TileBounds.X); i++)
            {
                for (int j = Math.Max(bounds.Y - r, 0); j < Math.Min(bounds.Bottom + r, ParentWorld.TileBounds.Y); j++)
                {
                    if (!GetTile(i, j, ID).Active) SetLightColor(i, j, Color.White);
                    else SetLightColor(i, j, Color.Black);
                }
            }
        }

        public void UpdateAmbientLight(int a, int b, int radius, string ID)
        {
            int r = radius / 2;

            for (int i = Math.Max(a - r, 0); i < Math.Min(a + r, ParentWorld.TileBounds.X); i++)
            {
                for (int j = Math.Max(b - r, 0); j < Math.Min(b + r, ParentWorld.TileBounds.Y); j++)
                {
                    if (!GetTile(i, j, ID).Active) SetLightColor(i, j, Color.White);
                    else SetLightColor(i, j, Color.Black);
                }
            }
        }

        public void EmitLight(int i, int j, float intensity, int radius, string ID, int iteration = 0)
        {
            if (iteration < radius)
            {
                SetTileColor(i, j, Color.Lerp(Color.Black, Color.White, intensity));
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
                                if (GetTile(k, l, ID).Active)
                                {
                                    if (GetSpace(k, l).TileColor.R / 255f < newIntensity)
                                        EmitLight(k, l, newIntensity, radius, ID, iteration + 1);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateSunLighting(int radius, string ID, Rectangle bounds)
        {
            UpdateAmbientLight(ID, bounds, radius);

            int r = radius;

            for (int i = Math.Max(bounds.X - r, 0); i < Math.Min(bounds.Right + r, ParentWorld.TileBounds.X); i++)
            {
                for (int j = Math.Max(bounds.Y - r, 0); j < Math.Min(bounds.Bottom + r, ParentWorld.TileBounds.Y); j++)
                {
                    if (GetTile(i, j, ID).Active)
                        SetTileColor(i, j, Color.Black);
                }
            }

            for (int i = Math.Max(bounds.X - r * 2, 0); i < Math.Min(bounds.Right + r * 2, ParentWorld.TileBounds.X); i++)
            {
                for (int j = Math.Max(bounds.Y - r * 2, 0); j < Math.Min(bounds.Bottom + r * 2, ParentWorld.TileBounds.Y); j++)
                {
                    if (GetSpace(i, j).LightSources == null) continue;

                    if (GetSpace(i, j).LightSources.Value.R > 0)
                    {
                        EmitLight(i, j, GetSpace(i, j).LightSources.Value.R / 255f, radius, ID);
                    }
                }
            }
        }

        //====================================DRAW LOGIC====================================
        public void UpdateLightBuffer()
        {
            CameraTransform Camera = LayerHost.GetLayer(ParentWorld.Layer).Camera;
            LayerHost.GetLayer("Default").MapHost.Maps.Get("TileLightingMap").CanDraw = true;

            LayerHost.GetLayer("Default").MapHost.Maps.Get("TileLightingMap").DrawToBatchedTarget
                 ((sb) =>
                 {
                     Point TL = (Camera.Transform.Position / TileManager.drawResolution).ToPoint();
                     Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

                     Utils.DrawBoxFill(new Rectangle(
                         Math.Max(TL.X - 1, 0),
                         Math.Max(TL.Y - 1, 0),
                         Math.Min(TL.X + BR.X + 1, ParentWorld.TileBounds.X),
                         Math.Min(TL.Y + BR.Y + 1, ParentWorld.TileBounds.Y)), Color.White, 0.1f);

                     for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, ParentWorld.TileBounds.X); i++)
                         for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, ParentWorld.TileBounds.Y); j++)
                         {
                             if (GetSpace(i, j).TileColor != Color.White)
                             {
                                 Rectangle r = new Rectangle(i, j, 1, 1);
                                 Utils.DrawBoxFill(r, GetSpace(i, j).TileColor, 0f);
                             }
                         }
                 });


        }

        public void UpdateSunLighting(int a, int b, int radius, string ID)
        {
            UpdateAmbientLight(a, b, radius, ID);

            int r = radius;

            for (int i = Math.Max(a - r, 0); i < Math.Min(a + r, ParentWorld.TileBounds.X); i++)
            {
                for (int j = Math.Max(b - r, 0); j < Math.Min(b + r, ParentWorld.TileBounds.Y); j++)
                {
                    if (GetTile(i, j, ID).Active)
                        SetTileColor(i, j, Color.Black);
                }
            }

            for (int i = Math.Max(a - r * 2, 0); i < Math.Min(a + r * 2, ParentWorld.TileBounds.X); i++)
            {
                for (int j = Math.Max(b - r * 2, 0); j < Math.Min(b + r * 2, ParentWorld.TileBounds.Y); j++)
                {
                    if (GetSpace(i, j).LightSources == null) continue;

                    if (GetSpace(i, j).LightSources.Value.R > 0)
                    {
                        EmitLight(i, j, GetSpace(i, j).LightSources.Value.R / 255f, radius, ID);
                    }
                }
            }
        }

        public void ApplyLighting()
        {
            int res = drawResolution;

            CameraTransform Camera = LayerHost.GetLayer(ParentWorld.Layer).Camera;
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
