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
    }
}
