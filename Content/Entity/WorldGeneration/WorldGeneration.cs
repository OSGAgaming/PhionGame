using QueefCord.Core.Entities;
using QueefCord.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhionGame.Content.Entity.WorldGeneration
{
    public class WorldGeneration
    {
        HashSet<WorldGenerationPass> passSet = new HashSet<WorldGenerationPass>();

        public void AddGenerationPass<T>() where T : WorldGenerationPass, new()
        {
            passSet.Add(new T());
            passSet = passSet.OrderBy(key => key.Priority).ToHashSet();
        }

        public void Generate(World World)
        {
            foreach(WorldGenerationPass pass in passSet)
            {
                pass.GenerationPass(World);
            }
        }

        public WorldGeneration()
        {
            passSet = new HashSet<WorldGenerationPass>();
        }
    }

    public abstract class WorldGenerationPass
    {
        public TileManager tiles => World.CurrentWorld.WorldTileManager;

        public Tile GetTile(int i, int j) => tiles.GetTile(i, j, "ground");

        public void AddTile(int i, int j, short id) => tiles.AddTile(i,j, "ground", id);

        public float Priority;
        public virtual void GenerationPass(World world) { }
    }
}
