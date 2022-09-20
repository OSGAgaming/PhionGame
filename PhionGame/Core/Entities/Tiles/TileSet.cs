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
    public class TileSet : IDraw, IMappable
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

        public IEnumerable<Rectangle> GetCollision(Tile?[,] tiles)
        {
            int w = TileManager.width;
            int h = TileManager.height;
            int res = TileManager.drawResolution;

            List<Rectangle> rectangleCache = new List<Rectangle>();

            bool iterate = false;
            int startX = -1;
            int startY = -1;

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    Tile? tile = tiles[i, j];

                    int endX;
                    int endY;
                    if (tile != null && !iterate && !rectangleCache.Any(n => n.Contains(new Point(i * res, j * res))) && tile.Value.solid)
                    {
                        iterate = true;
                        startX = i;
                        startY = j;
                    }

                    if (iterate && tile == null)
                    {
                        endX = i;
                        endY = j - 1;

                        int greedyCount;
                        int step = 0;

                        do
                        {
                            greedyCount = 0;
                            step++;

                            for (int top = startY; top <= endY; top++)
                            {
                                Tile? t = Tiles[startX + step, top];
                                if (t != null) greedyCount++;
                            }
                        }
                        while (greedyCount == endY - startY + 1);

                        Rectangle r = new Rectangle(startX * res, startY * res, step * res, (endY - startY + 1) * res);

                        i = startX;
                        j = startY;

                        startX = -1;
                        startY = -1;
                        iterate = false;

                        rectangleCache.Add(r);
                    }
                }

            return rectangleCache;
        }

        public void AddTile(int i, int j, short id)
        {
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

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / 16).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / 16).ToPoint();

            for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                {
                    //Greedy Meshing
                    Tile? tile = Tiles[i, j];

                    if (tile != null)
                    {
                        Rectangle r = new Rectangle(i * res, j * res, res, res).ToMiniMap();
                        Utils.DrawRectangle(r, TileManager.TileColors[tile.Value.id], Solid ? 0.99f : 1f);
                    }
                }
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

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / TileManager.drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / TileManager.drawResolution).ToPoint();

            for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                {
                    //Greedy Meshing
                    Tile? tile = Tiles[i, j];

                    if (tile != null)
                    {
                        Rectangle r = new Rectangle(i * res, j * res, res, res);
                        Rectangle s = FramingMethod?.Invoke(this, i, j) ?? Rectangle.Empty;

                        sb.Draw(Assets<Texture2D>.Get($"Textures/Tiles/{TileManager.TileTextures[tile.Value.id]}"), r, s, Color.White, Solid ? 0.99f : 1f);
                    }
                }

            foreach (Rectangle r in CollisionBoxes)
                SceneHolder.CurrentScene.GetSystem<AABBCollisionSystem>().GenerateHitbox(new Collideable2D(null, r, Solid));
        }

    }
}
