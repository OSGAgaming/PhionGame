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
        public Dictionary<Point, IEnumerable<Rectangle>> CollisionBoxes;

        public Dictionary<Point, IEnumerable<Rectangle>> UpdateChunkOverBounds(string ID, Rectangle bounds)
        {
            Rectangle r = bounds.Divide(World.CurrentWorld.ChunkSize);

            for (int i = r.X; i <= r.Right; i++)
            {
                for (int j = r.Y; j <= r.Bottom; j++)
                {
                    CollisionBoxes[new Point(i, j)] = UpdateChunkCollision(ID, i, j);
                }
            }
            
            return CollisionBoxes;
        }

        public IEnumerable<Rectangle> UpdateChunkCollision(string ID, int a, int b) => GetCollision(ID, new Rectangle(a, b, 1, 1).Multiply(World.CurrentWorld.ChunkSize));

        public IEnumerable<Rectangle> GetCollision(string ID, Rectangle bounds)
        {
            int res = drawResolution;

            List<Rectangle> rectangleCache = new List<Rectangle>();

            bool iterate = false;
            int startX = -1;
            int startY = -1;

            Point TL = (LayerHost.GetLayer(ParentWorld.Layer).Camera.Transform.Position / drawResolution).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / drawResolution).ToPoint();

            for (int i = Math.Max(bounds.X, 0); i < Math.Min(bounds.Right, ParentWorld.TileBounds.X); i++)
                for (int j = Math.Max(bounds.Y, 0); j < Math.Min(bounds.Bottom, ParentWorld.TileBounds.Y); j++)
                {
                    Tile tile = GetTile(i, j, ID);

                    int endX;
                    int endY;
                    if (tile.Active && !iterate && !rectangleCache.Any(n => n.Contains(new Point(i * res, j * res))) && tile.solid)
                    {
                        iterate = true;
                        startX = i;
                        startY = j;
                    }

                    if (iterate && (!tile.Active || j >= bounds.Bottom - 1))
                    {
                        endX = i;
                        endY = (j == bounds.Bottom - 1 && tile.Active) ? j : j - 1;

                        int greedyCount;
                        int step = 0;

                        do
                        {
                            greedyCount = 0;
                            step++;

                            for (int top = startY; top <= endY; top++)
                            {
                                Tile t = GetTile(startX + step, top, ID);
                                if (t.Active && t.solid) greedyCount++;
                            }
                        }
                        while (greedyCount == endY - startY + 1 && startX + step < bounds.Right);

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
    }
}
