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

namespace QueefCord.Content.Scenes
{
    public class OrthoTestScene : Scene
    {
        private Player Player;
        private readonly int testItemCount = 20;
        private Prop testProp;
        private TileManager SceneTileManager;

        public OrthoTestScene()
        {
            Player = new Player();
            Player.Transform.Position = new Vector2(100, 0);

            AddEntity(UIScreenManager.Instance);
        }

        public override void OnActivate()
        {
            HealthManaUI.PlayerFocus = Player;

            testProp = new Prop("Rock", "Default");
            testProp.Transform.Position = new Vector2(100, 100);
            testProp.Size = new Vector2(20, 20);
            SceneTileManager = new TileManager();

            SceneTileManager.AddTileSet("ground", "TileFrame", true);

            AddEntity(SceneTileManager);
            AddEntity(new DayNightCycle());
            AddEntity(Player);
            AddEntity(testProp);
            AddEntity(new ExampleEnemy());
            //AddEntity(new LineOfSight());

            AddEntity(new ItemEntity<ExampleSword>(new Vector2(16), new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100)))); 
            AddEntity(new ItemEntity<ExampleBow>(new Vector2(16), new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))));
            AddEntity(new ItemEntity<FruitAxe>(new Vector2(16), new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))));
            AddEntity(new ItemEntity<StoneHoe>(new Vector2(16), new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))));
            AddEntity(new ItemEntity<WateringCan>(new Vector2(16), new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))));

            float GlobalScale = 1.6f;

            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsSky", "Background", 1, 3, Vector2.Zero, GlobalScale, 0.5f));
            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsFurthest", "Background", 0.8f, 3, Vector2.Zero, GlobalScale, 0.4f));
            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsFar", "Background", 0.6f, 3, Vector2.Zero, GlobalScale, 0.3f));
            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsMid", "Background", 0.4f, 3, Vector2.Zero, GlobalScale, 0.2f));
            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsClose", "Background", 0.2f, 3, Vector2.Zero, GlobalScale, 0.1f));
            AddEntity(new ParalaxedSprite("Textures/Backgrounds/PlainsClosest", "Background", 0.1f, 3, Vector2.Zero, GlobalScale, 0));
        }


        public override void RegisterSystems()
        {
            AddSystem<AABBCollisionSystem>();
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