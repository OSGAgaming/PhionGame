using Microsoft.Xna.Framework;
using QueefCord.Content.Maths;
using QueefCord.Core.Entities;
using QueefCord.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhionGame.Content.Entity.WorldGeneration
{
    public class CaveGeneration : WorldGenerationPass
    {
        public byte[,] CreateCellNoise(Rectangle bounds, int iterations, float density)
        {
            byte[,] grid = new byte[bounds.Width, bounds.Height];

            int w = bounds.Width;
            int h = bounds.Height;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    float a = Rand.NextFloat(1);
                    if (a < density) grid[i, j] = 1;
                }
            }

            for (int s = 0; s < iterations; s++)
            {
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        int neighbourCount = 0;
                        for (int a = -1; a < 2; a++)
                        {
                            for (int b = -1; b < 2; b++)
                            {
                                if (a != 0 || b != 0)
                                {
                                    if (i + a > bounds.Width - 1 ||
                                        j + b > bounds.Height - 1 ||
                                        i + a < 0 ||
                                        j + b < 0)
                                    {
                                        neighbourCount++;
                                        continue;
                                    }
                                    else
                                    {
                                        neighbourCount += grid[i + a, j + b];
                                    }
                                }
                            }
                        }

                        if (neighbourCount > 4) grid[i, j] = 1;
                        else grid[i, j] = 0;
                    }
                }
            }

            return grid;
        }
        public override void GenerationPass(World world)
        {
            Rectangle startingTiles = new Rectangle(0, 60, 300, 300);
            byte[,] cell = CreateCellNoise(startingTiles, 7, 0.65f);

            for (int i = startingTiles.X; i < startingTiles.Right; i++)
            {
                for (int j = startingTiles.Y; j < startingTiles.Bottom; j++)
                {
                    if (cell[i - startingTiles.X, j - startingTiles.Y] == 1)
                        AddTile(i, j, 2);
                }
            }

            for (int i = startingTiles.X; i < startingTiles.Right; i++)
            {
                for (int j = startingTiles.Y; j < startingTiles.Bottom; j++)
                {
                    if (j > 0 && i > 0)
                    {
                        if (GetTile(i, j - 1).Active &&
                            GetTile(i, j + 1).Active &&
                            GetTile(i - 1, j).Active &&
                            GetTile(i + 1, j).Active)
                            AddTile(i, j, 1);
                    }
                }
            }

            world.WorldTileManager.UpdateTileMetaData(startingTiles);
        }
    }
}
