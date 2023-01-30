using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using QueefCord.Core.Entities.EntitySystems;
using QueefCord.Content.Entities;
using QueefCord.Core.DataStructures;
using QueefCord.Content.Maths;
using QueefCord.Content.UI;
using QueefCord.Core.Tiles;
using QueefCord.Core.Input;
using QueefCord.Core.Graphics;
using QueefCord.Core.Entities;
using QueefCord.Core.Resources;
using PhionGame.Content.Entity.WorldGeneration;

namespace QueefCord.Content.Scenes
{
    public class OrthoTestScene : Scene
    {
        private World World;
        private Player Player;
        private WorldGeneration Generation;

        public override void OnActivate()
        {
            HealthManaUI.PlayerFocus = Player;

            Generation = new WorldGeneration();
            Generation.AddGenerationPass<CaveGeneration>();
            World = new World(new Point(75, 75), new Point(30, 107), Generation, new TileSetInfo("TileFrame", "ground", true));
            Player = new Player();
            Player.Transform.Position = new Vector2(100, 0);

            AddEntity(UIScreenManager.Instance);
            AddEntity(World);
            AddEntity(new DayNightCycle());
            AddEntity(Player);
            //AddEntity(new LineOfSight());

            float GlobalScale = 1.6f;

            CrepsularRays Rays = new CrepsularRays();
            ParalaxedSprite furthest = new ParalaxedSprite("Textures/Backgrounds/PlainsFurthest", "Default", new Vector2(0.95f, 0.95f), 3, Vector2.Zero, GlobalScale, 0.94f);
            ParalaxedSprite far = new ParalaxedSprite("Textures/Backgrounds/PlainsFar", "Default", new Vector2(0.9f, 0.9f), 3, Vector2.Zero, GlobalScale, 0.93f);
            ParalaxedSprite mid = new ParalaxedSprite("Textures/Backgrounds/PlainsMid", "Default", new Vector2(0.85f, 0.85f), 3, Vector2.Zero, GlobalScale, 0.92f);
            ParalaxedSprite close = new ParalaxedSprite("Textures/Backgrounds/PlainsClose", "Default", new Vector2(0.8f, 0.8f), 3, Vector2.Zero, GlobalScale, 0.91f);
            ParalaxedSprite closest = new ParalaxedSprite("Textures/Backgrounds/PlainsClosest", "Default", new Vector2(0.75f, 0.75f), 3, Vector2.Zero, GlobalScale, 0.9f);

            AddEntity(Rays);

            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsSky", "Default", new Vector2(1,1), 3, Vector2.Zero, GlobalScale, 0.95f));
            Rays.OcclusionMaps.Add(furthest);
            AddEntity(furthest);
            Rays.OcclusionMaps.Add(far);
            AddEntity(far);
            Rays.OcclusionMaps.Add(mid);
            AddEntity(mid);
            Rays.OcclusionMaps.Add(close);
            AddEntity(close); 
            Rays.OcclusionMaps.Add(closest);
            AddEntity(closest);
        }

        public override void RegisterSystems()
        {
            AddSystem<CollisionSystem>();
        }
        public override void Update(GameTime time)
        {
            if (GameInput.Instance["ScrollU"].IsDown())
                (LayerHost.GetLayer("Default").Camera as EntityFocalCamera).TargetScale += 0.01f;
            if (GameInput.Instance["ScrollD"].IsDown())
                (LayerHost.GetLayer("Default").Camera as EntityFocalCamera).TargetScale -= 0.01f;
        }
    }
}