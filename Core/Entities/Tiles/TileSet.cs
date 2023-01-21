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
    public partial class TileSet : IMappable
    {
        public Chunk Parent { get; set; }
        public Tile[,] Tiles { get; set; }
        public string ID { get; set; }

        public Point Size => Parent.Bounds.Size;
        public Point Position => Parent.Bounds.Location;

        public TileSet(Chunk chunk)
        {
            Parent = chunk;
            Tiles = new Tile[Size.X, Size.Y];
        }

        public Tile GetTile(int x, int y) => Parent.Parent.WorldTileManager.GetTile(x, y, ID);

        public void Save(BinaryWriter bw)
        {
            bw.Write(ID);

            bw.Write(Tiles.GetLength(0));
            bw.Write(Tiles.GetLength(1));

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    bw.Write(!GetTile(i, j).Active);

                    if (GetTile(i, j).Active)
                    {
                        bw.Write(Tiles[i, j].id);
                        bw.Write(Tiles[i, j].solid);
                    }
                }
            }
        }

        public static TileSet Load(BinaryReader br)
        {
            string id = br.ReadString();

            Type declaringType = br.ReadType();
            string methodName = br.ReadString();

            Func<TileSet, int, int, Rectangle> tilingMethod =
                (Func<TileSet, int, int, Rectangle>)
                Delegate.CreateDelegate(typeof(Func<TileSet, int, int, Rectangle>),
                declaringType.GetMethod(methodName));

            TileSet set = new TileSet(null);

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

            return set;
        }
    }
}
