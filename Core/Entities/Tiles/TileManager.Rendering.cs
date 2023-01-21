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
        public void RenderTiles(string ID)
        {
            Point TL = (LayerHost.GetLayer(ParentWorld.Layer).Camera.Transform.Position / drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / drawResolution).ToPoint();

            int res = drawResolution;

            Texture2D tex = Assets<Texture2D>.Get($"Textures/Tiles/TestTile");

            LayerHost.GetLayer(ParentWorld.Layer).MapHost.Maps.DrawToBatchedMap("TileMap",
             (SpriteBatch sb) =>
             {
                 for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, ParentWorld.TileBounds.X); i++)
                     for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, ParentWorld.TileBounds.Y); j++)
                     {
                         Tile tile = GetTile(i, j, ID);
                         bool IsActive(int a, int b)
                         {
                             if (i + a < 0 || j + b < 0) return false;

                             return GetTile(i + a, j + b, ID).Active;
                         }

                         if (!GetTile(i, j, ID).Active)
                         {
                             if ((IsActive(1, 0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1)))
                             {
                                 Rectangle r = new Rectangle(i * res, j * res, res, res);
                                 Rectangle s = ParentWorld.TileSets[ID].FramingMethod?.Invoke(i, j) ?? Rectangle.Empty;

                                 sb.Draw(tex, r, s, Color.White, 0.1f);
                             }
                         }

                         if (GetTile(i, j, ID).Active)
                         {
                             Rectangle r = new Rectangle(i * res, j * res, res, res);
                             Rectangle s = ParentWorld.TileSets[ID].FramingMethod?.Invoke(i, j) ?? Rectangle.Empty;

                             sb.Draw(tex, r, s, Color.White, 0.1f);
                         }
                     }
             });
        }

        public void RenderTileMaps(SpriteBatch sb, string ID)
        {
            Point TL = (LayerHost.GetLayer(ParentWorld.Layer).Camera.Transform.Position / drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / drawResolution).ToPoint();

            int fRes = frameResolution;
            int dRes = drawResolution;


            LayerHost.GetLayer(ParentWorld.Layer).MapHost.Maps.DrawToBatchedMap("TileTextureMap",
             (SpriteBatch sb) =>
             {
                 for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, ParentWorld.TileBounds.X); i++)
                     for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, ParentWorld.TileBounds.Y); j++)
                     {
                         Tile tile = GetTile(i, j, ID);

                         if (tile.Active)
                         {
                             Texture2D tex = TileTextures[tile.id];
                             int loop = (tex.Width / fRes);

                             Rectangle r = new Rectangle(i * dRes, j * dRes, dRes, dRes);
                             Rectangle s = new Rectangle(i % loop * fRes, j % loop * fRes, fRes, fRes);

                             sb.Draw(tex, r, s, Color.White, 0.075f);
                         }
                     }
             });

        }

        public void DrawToMiniMap(string ID)
        {
            int res = drawResolution;

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            for (int i = Math.Max(TL.X - 1,0); i < Math.Min(TL.X + BR.X + 1, ParentWorld.TileBounds.X); i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, ParentWorld.TileBounds.Y); j++)
                {
                    //Greedy Meshing
                    Tile tile = GetTile(i, j, ID);

                    if (tile.Active)
                    {
                        Rectangle r = new Rectangle(i, j, 1, 1);
                        Utils.DrawBoxFill(r, TileColors[tile.id], 0.99f);
                    }
                }
        }
    }
}
