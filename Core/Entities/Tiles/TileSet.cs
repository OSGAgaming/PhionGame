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
        public Tile?[,] Tiles { get; set; }

        public string ID { get; set; }

        public string Layer => "Default";

        //TODO: Temporary

        public int width;
        public int height;
        public int Index;
        public bool Solid;

        public string FramingMethodPath;

        [JsonIgnore]
        public Func<TileSet, int, int, Rectangle> FramingMethod;
        public IEnumerable<Rectangle> CollisionBoxes;

        [JsonConstructor]
        public TileSet(string FramingMethodPath)
        {
            this.FramingMethodPath = FramingMethodPath;
            CollisionBoxes = new List<Rectangle>();

            Tiles = new Tile?[TileManager.width, TileManager.height];
            TileColor = new Color[TileManager.width, TileManager.height];
            LightSources = new Color?[TileManager.width, TileManager.height];
            Outline = new byte[TileManager.width, TileManager.height];
            Top = new byte[TileManager.width, TileManager.height];

            ResetLights();

            FramingMethod =
                (Func<TileSet, int, int, Rectangle>)
                Delegate.CreateDelegate(typeof(Func<TileSet, int, int, Rectangle>),
                typeof(Framing).GetMethod(FramingMethodPath));
        }

        public TileSet() { }

        public void Save(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Index);

            bw.Write(FramingMethod.Method.DeclaringType);
            bw.Write(FramingMethod.Method.Name);

            bw.Write(Tiles.GetLength(0));
            bw.Write(Tiles.GetLength(1));

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    bw.Write(Tiles[i, j] == null);

                    if (Tiles[i, j] != null)
                    {
                        bw.Write(Tiles[i, j].Value.id);
                        bw.Write(Tiles[i, j].Value.solid);
                    }
                }
            }
        }

        public static TileSet Load(BinaryReader br)
        {
            string id = br.ReadString();
            int index = br.ReadInt32();

            Type declaringType = br.ReadType();
            string methodName = br.ReadString();

            Func<TileSet, int, int, Rectangle> tilingMethod =
                (Func<TileSet, int, int, Rectangle>)
                Delegate.CreateDelegate(typeof(Func<TileSet, int, int, Rectangle>),
                declaringType.GetMethod(methodName));

            TileSet set = new TileSet(methodName);

            int width = br.ReadInt32();
            int height = br.ReadInt32();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tile tile = new Tile();

                    bool isNull = br.ReadBoolean();

                    if (!isNull)
                    {
                        tile.id = br.ReadInt16();
                        tile.solid = br.ReadBoolean();

                        set.Tiles[i, j] = tile;
                    }
                }
            }

            set.ID = id;
            set.Index = index;

            return set;
        }

        public void AddTile(int i, int j, short id)
        {
            if (i < 0 || j < 0) return;

            Tiles[i, j] = new Tile()
            {
                solid = Solid,
                id = id
            };
        }
        public void DrawToMiniMap(SpriteBatch sb)
        {
            int w = TileManager.width;
            int h = TileManager.height;
            int res = TileManager.drawResolution;

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                {
                    //Greedy Meshing
                    Tile? tile = Tiles[i, j];

                    if (tile != null)
                    {
                        Rectangle r = new Rectangle(i, j, 1, 1);
                        Utils.DrawBoxFill(r, TileManager.TileColors[tile.Value.id], Solid ? 0.99f : 1f);
                    }
                }
        }

        public void RenderTiles()
        {
            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / TileManager.drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

            int res = TileManager.drawResolution;
            int w = TileManager.width;
            int h = TileManager.height;

            Texture2D tex = Assets<Texture2D>.Get($"Textures/Tiles/TestTile");

            LayerHost.GetLayer(Layer).MapHost.Maps.DrawToBatchedMap("TileMap",
             (SpriteBatch sb) =>
             {
                 for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                     for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                     {
                         Tile? tile = Tiles[i, j];
                         bool IsActive(int a, int b)
                         {
                             if (i + a < 0 || j + b < 0) return false;

                             return Tiles[i + a, j + b] != null;
                         }

                         if((IsActive(1,0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1)) && tile == null)
                         {
                             Rectangle r = new Rectangle(i * res, j * res, res, res);
                             Rectangle s = FramingMethod?.Invoke(this, i, j) ?? Rectangle.Empty;

                             sb.Draw(tex, r, s, Color.White, 0.1f);
                         }

                         if (tile != null)
                         {
                             Rectangle r = new Rectangle(i * res, j * res, res, res);
                             Rectangle s = FramingMethod?.Invoke(this, i, j) ?? Rectangle.Empty;

                             sb.Draw(tex, r, s, Color.White, 0.1f);
                         }
                     }
             });
        }

        public void RenderTileMaps(SpriteBatch sb)
        {
            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / TileManager.drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

            int fRes = TileManager.frameResolution;
            int dRes = TileManager.drawResolution;

            int w = TileManager.width;
            int h = TileManager.height;

            LayerHost.GetLayer(Layer).MapHost.Maps.DrawToBatchedMap("TileTextureMap",
             (SpriteBatch sb) =>
             {
                 for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                     for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                     {
                         Tile? tile = Tiles[i, j];

                         if (tile != null)
                         {
                             Texture2D tex = TileManager.TileTextures[tile.Value.id];
                             int loop = (tex.Width / fRes);

                             Rectangle r = new Rectangle(i * dRes, j * dRes, dRes, dRes);
                             Rectangle s = new Rectangle(i % loop * fRes, j % loop * fRes, fRes, fRes);

                             sb.Draw(tex, r, s, Color.White, 0.075f);
                         }
                     }
             });

        }
        public void Draw(SpriteBatch sb)
        {
            if (FramingMethod == null && FramingMethodPath != null)
            {
                FramingMethod =
                (Func<TileSet, int, int, Rectangle>)
                Delegate.CreateDelegate(typeof(Func<TileSet, int, int, Rectangle>),
                typeof(Framing).GetMethod(FramingMethodPath));
            }

            Vector2 p = Utils.DefaultMouseWorld.Snap(TileManager.drawResolution);

            int w = TileManager.width;
            int h = TileManager.height;
            int res = TileManager.drawResolution;

            int a = (int)(p.X / TileManager.drawResolution);
            int b = (int)(p.Y / TileManager.drawResolution);



            if (TileManager.ActiveTileSet == Index && TileManager.PlaceMode)
            {
                Rectangle drag = GameInput.Instance.WorldDragArea.Snap(res).AddSize(new Vector2(res));

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    AddTile(a, b, TileManager.ActiveAtlas);

                    //make event
                    UpdateSunLighting(a,b, 8);
                    ConfigureOutline(a, b);
                    Logger.NewText(new Vector2(a,b));

                    CollisionBoxes = GetCollision(Tiles);
                }

                if (!GameInput.Instance.IsRightClicking)
                    Utils.DrawRectangle(p, TileManager.drawResolution, TileManager.drawResolution, Color.Yellow, 1f);
                else
                    Utils.DrawRectangle(drag, Color.Yellow, 1f);

                if (GameInput.Instance.JustReleaseRight)
                {
                    for (int j = drag.X / res; j < drag.Right / res; j++)
                    {
                        for (int k = drag.Y / res; k < drag.Bottom / res; k++)
                        {
                            AddTile(j, k, TileManager.ActiveAtlas);
                        }
                    }

                    UpdateSunLighting(8);
                    ConfigureOutline(a, b);

                    CollisionBoxes = GetCollision(Tiles);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && GameInput.Instance["A"].IsJustPressed() && !Solid)
                {
                    for (int j = 0; j < w; j++)
                    {
                        for (int k = 0; k < h; k++)
                        {
                            AddTile(j, k, TileManager.ActiveAtlas);
                        }
                    }

                    CollisionBoxes = GetCollision(Tiles);
                }
            }

            RenderTiles();
            RenderTileMaps(sb);
            ApplyLighting();
            RenderOutline(sb);

            foreach (Rectangle r in CollisionBoxes)
                SceneHolder.CurrentScene.GetSystem<AABBCollisionSystem>().GenerateHitbox(new Collideable2D(null, r, Solid));
        }

    }
}
