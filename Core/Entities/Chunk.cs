using System;
using System.IO;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using QueefCord.Core.Tiles;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace QueefCord.Core.Entities
{
    public class Chunk : ISerializable, IUpdate
    {
        public World Parent { get; set; }
        public Rectangle Bounds { get; set; }

        public HashSet<IUpdate> Entities = new HashSet<IUpdate>();

        public Dictionary<string, TileSet> TileSets;

        public Space[,] Spaces;

        public Chunk(World parent, Rectangle bounds, params TileSetInfo[] sets)
        {
            TileSets = new Dictionary<string, TileSet>();
            Parent = parent;
            Bounds = bounds;

            Spaces = new Space[parent.ChunkSize.X, parent.ChunkSize.Y];

            foreach (var set in sets)
            {
                TileSet tileSet = new TileSet(this);
                tileSet.ID = set.ID;

                TileSets.Add(set.ID, tileSet);
            }
        }

        public IComponent Read(BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public void Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            foreach(var entity in Entities.ToArray())
            {
                entity.Update(gameTime);
            }
        }
    }
}
