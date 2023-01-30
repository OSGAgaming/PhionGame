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
    public partial class TileManager : EntityCore, IDraw, IMappable
    {
        public static TileManager Instance;
        public World ParentWorld;

        public const int drawResolution = 16;
        public const int frameResolution = 16;

        public static int ActiveTileSet = 0;
        public static short ActiveAtlas = 0;

        public static bool PlaceMode;

        public string ID => ParentWorld.TileSets.ElementAt(ActiveTileSet).Key;

        public string Layer => "Default";

        public TileManager(World Parent)
        {
            AddTileSets();
            Instance = this;
            ParentWorld = Parent;
            CollisionBoxes = new Dictionary<Point, IEnumerable<Rectangle>>();

            ResetLights();
        }

        public void UpdateTileMetaData(Rectangle bounds)
        {
            SceneHolder.CurrentScene?.GetSystem<CollisionSystem>().StaticHitboxes.Clear();

            UpdateSunLighting(LightIntensity, ID, bounds);
            ConfigureOutline(bounds.Right, bounds.Bottom, ID);

            CollisionBoxes = UpdateChunkOverBounds(ID, bounds);

            foreach (var r in CollisionBoxes.Values)
            {
                foreach (Rectangle rect in r)
                {
                    SceneHolder.CurrentScene?.GetSystem<CollisionSystem>().GenerateStaticHitbox(new Collideable2D(null, rect, true));
                }
            }
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (PlaceMode)
            {
                if (GameInput.Instance["Tab"].IsJustPressed())
                {
                    ActiveTileSet++;
                    ActiveTileSet %= ParentWorld.TileSets.Count;

                    Logger.NewText("TileSet switched to: " + ID);
                }

                if (GameInput.Instance["L"].IsJustPressed())
                {
                    ActiveAtlas = (short)((ActiveAtlas + 1) % TileTexturePaths.Count);
                    Logger.NewText("Atlas switched to: " + TileTexturePaths[ActiveAtlas]);
                }
            }

            if (GameInput.Instance["Q"].IsJustPressed())
            {
                PlaceMode = !PlaceMode;
                Logger.NewText(PlaceMode);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 p = Utils.DefaultMouseWorld.Snap(drawResolution);

            int res = drawResolution;

            int a = (int)(p.X / drawResolution);
            int b = (int)(p.Y / drawResolution);

            Rectangle drag = GameInput.Instance.WorldDragArea.Snap(res).AddSize(new Vector2(res));
            Rectangle tileDrag = drag.Divide(drawResolution);

            if (PlaceMode)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    AddTile(a, b, ID, ActiveAtlas);

                    UpdateTileMetaData(new Rectangle(a, b, 1, 1));
                }

                if (!GameInput.Instance.IsRightClicking)
                    Utils.DrawRectangle(p, drawResolution, drawResolution, Color.Yellow, 1f);
                else
                    Utils.DrawRectangle(drag, Color.Yellow, 1f);

                if (GameInput.Instance.JustReleaseRight)
                {
                    for (int j = tileDrag.X; j < tileDrag.Right; j++)
                    {
                        for (int k = tileDrag.Y; k < tileDrag.Bottom; k++)
                        {
                            AddTile(j, k, ID, ActiveAtlas);
                        }
                    }

                    UpdateTileMetaData(tileDrag);
                }
            }

            ApplyLighting();

            foreach (TileSetInfo set in ParentWorld.TileSets.Values)
            {
                RenderTiles(set.ID);
                RenderTileMaps(sb, set.ID);
                RenderOutline(sb, set.ID);
                DrawToMiniMap(set.ID);
            }
        }

        //Utils
        public Chunk GetChunk(int x, int y) => ParentWorld.Chunks[new Point(x / ParentWorld.ChunkSize.X, y / ParentWorld.ChunkSize.Y)];

        public Tile GetTile(int x, int y, string set) => GetChunk(x, y).TileSets[set].Tiles[x % ParentWorld.ChunkSize.X, y % ParentWorld.ChunkSize.Y];

        public Tile SetTile(int x, int y, string set, Tile tile) => GetChunk(x, y).TileSets[set].Tiles[x % ParentWorld.ChunkSize.X, y % ParentWorld.ChunkSize.Y] = tile;

        public Tile SetOutline(int x, int y, string set, byte n)
        {
            Tile result = GetTile(x, y, set);
            result.Outline = n;

            SetTile(x, y, set, result);

            return result;
        }

        public Tile SetTop(int x, int y, string set, byte n)
        {
            Tile result = GetTile(x, y, set);
            result.Top = n;

            SetTile(x, y, set, result);

            return result;
        }

        public Space GetSpace(int x, int y) => GetChunk(x, y).Spaces[x % ParentWorld.ChunkSize.X, y % ParentWorld.ChunkSize.Y];

        public Space SetSpace(int x, int y, Space space) => GetChunk(x, y).Spaces[x % ParentWorld.ChunkSize.X, y % ParentWorld.ChunkSize.Y] = space;

        public Space SetTileColor(int x, int y, Color c)
        {
            Space result = GetSpace(x, y);
            result.TileColor = c;

            SetSpace(x, y, result);

            return result;
        }

        public Space SetLightColor(int x, int y, Color c)
        {
            Space result = GetSpace(x, y);
            result.LightSources = c;

            SetSpace(x, y, result);

            return result;
        }

        public void AddTile(int i, int j, string ID, short id)
        {
            if (i < 0 || j < 0) return;
            SetTile(i, j, ID,
            new Tile()
            {
                solid = true,
                id = id,
                Active = true
            });
        }
    }
}
