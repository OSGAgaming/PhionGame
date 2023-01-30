using System;
using System.IO;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using QueefCord.Core.Tiles;
using Microsoft.Xna.Framework.Graphics;
using QueefCord.Content.Entities;
using PhionGame.Content.Entity.WorldGeneration;
using QueefCord.Core.Graphics;

namespace QueefCord.Core.Entities
{
    public struct TileSetInfo
    {
        public string FramingMethodPath;
        public string ID;
        public bool Solid;
        public Func<int, int, Rectangle> FramingMethod;

        public TileSetInfo(string FramingMethodPath, string ID, bool Solid)
        {
            this.FramingMethodPath = FramingMethodPath;
            this.ID = ID;
            this.Solid = Solid;
            FramingMethod =
                (Func<int, int, Rectangle>)
                Delegate.CreateDelegate(typeof(Func<int, int, Rectangle>),
                typeof(Framing).GetMethod(FramingMethodPath));
        }
    }

    public class World : IUpdate, IDraw
    {
        public static World CurrentWorld;
        public TileManager WorldTileManager;

        public Dictionary<Point, Chunk> Chunks = new Dictionary<Point, Chunk>();
        public Point ChunkSize;
        public Point ChunkSpan;

        public Dictionary<string, TileSetInfo> TileSets;

        public string Layer => "Default";

        public Point TileBounds => ChunkSize.Multiply(ChunkSpan);

        public Rectangle GetActiveChunks(CameraTransform c)
        {
            Point TL = (c.Transform.Position / ChunkSize.ToVector2()).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / ChunkSize.ToVector2()).ToPoint();

            return new Rectangle(TL, BR).Divide(TileManager.drawResolution);
        }

        public World(Point chunkSize, Point chunkSpan, WorldGeneration generation = null, params TileSetInfo[] tileSets)
        {
            WorldTileManager = new TileManager(this);
            TileSets = new Dictionary<string, TileSetInfo>();
            ChunkSize = chunkSize;
            ChunkSpan = chunkSpan;
            foreach (TileSetInfo tileSetInfo in tileSets)
            {
                TileSets.Add(tileSetInfo.ID, tileSetInfo);
            }

            for (int i = 0; i < ChunkSpan.X; i++)
            {
                for (int j = 0; j < ChunkSpan.Y; j++)
                {
                    Rectangle bounds = new Rectangle(i, j, 1, 1).Multiply(ChunkSize.X);
                    Chunks.Add(new Point(i, j), new Chunk(this, bounds, tileSets));
                }
            }

            CurrentWorld = this;
            generation?.Generate(this);
        }

        public void Update(GameTime gameTime)
        {
            WorldTileManager.Update(gameTime);

            foreach (var chunk in Chunks)
            {
                chunk.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle activeChunks = GetActiveChunks(LayerHost.GetLayer("Default").Camera);
            for(int i = activeChunks.X; i <= activeChunks.Right; i++)
            {
                for(int j = activeChunks.Y; j <= activeChunks.Bottom; j++)
                {
                    Utils.DrawRectangle(new Rectangle(i, j, 1, 1).Multiply(ChunkSize.Multiply(TileManager.drawResolution)), Color.Purple, 3);
                }
            }
            WorldTileManager.Draw(sb);
        }
    }
}
