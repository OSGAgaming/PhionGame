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

        public Dictionary<Point, Chunk> ActiveChunks = new Dictionary<Point, Chunk>();
        public Point ChunkSize;
        public TileManager WorldTileManager;
        public Dictionary<string, TileSetInfo> TileSets;
        public Point ChunkSpan;

        public string Layer => "Default";

        public Point TileBounds => ChunkSize.Dot(ChunkSpan);

        public World(Point chunkSize, Point chunkSpan, params TileSetInfo[] tileSets)
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
                    ActiveChunks.Add(new Point(i, j), new Chunk(this, bounds, tileSets));
                }
            }

            CurrentWorld = this;
        }

        public void Update(GameTime gameTime)
        {
            WorldTileManager.Update(gameTime);

            foreach (var chunk in ActiveChunks)
            {
                chunk.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            WorldTileManager.Draw(sb);
        }
    }
}
