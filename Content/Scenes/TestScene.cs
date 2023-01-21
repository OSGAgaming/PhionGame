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

namespace QueefCord.Content.Scenes
{
    public class TestScene : Scene
    {
        private Player Player;
        private readonly int testItemCount = 20;
        private Prop testProp;

        public TestScene()
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

            Prop OrangeTree = new Prop("OrangeTree", "Default");
            OrangeTree.Transform.Position = new Vector2(300, 300);
            OrangeTree.Size = new Vector2(48, 48);

            Prop Crop = new Prop("PumpkinCrop", "Default");
            Crop.Transform.Position = new Vector2(200, 200);
            Crop.Size = new Vector2(16, 16);

            AddEntity(new DayNightCycle());
            AddEntity(Player);
            AddEntity(testProp);
            AddEntity(new ExampleEnemy());
            AddEntity(new ExampleEnemy());
            AddEntity(Crop);
            AddEntity(new LineOfSight());

            for (int i = 0; i < 15; i++)
                AddEntity(new Prop("OrangeTree", "Default")
                {
                    Transform = new Transform(new Vector2(Rand.random.Next(0, 600), Rand.random.Next(0, 400))),
                    Size = new Vector2(48, 48)
                });

            for (int i = 0; i < testItemCount; i++)
                AddEntity(new ItemEntity<Orange>
                {
                    Transform = new Transform(new Vector2(Rand.random.Next(0, 800), Rand.random.Next(0, 400))),
                    Size = new Vector2(16)
                });

            for (int i = 0; i < testItemCount; i++)
                AddEntity(new ItemEntity<PumpkinSeed>
                {
                    Transform = new Transform(new Vector2(Rand.random.Next(0, 800), Rand.random.Next(0, 400))),
                    Size = new Vector2(16)
                });

            AddEntity(new ItemEntity<ExampleSword>
            {
                Transform = new Transform(new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))),
                Size = new Vector2(16)
            });

            AddEntity(new ItemEntity<ExampleBow>
            {
                Transform = new Transform(new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))),
                Size = new Vector2(16)
            });

            AddEntity(new ItemEntity<StoneAxe>
            {
                Transform = new Transform(new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))),
                Size = new Vector2(16)
            });

            AddEntity(new ItemEntity<StoneHoe>
            {
                Transform = new Transform(new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))),
                Size = new Vector2(16)
            });

            AddEntity(new ItemEntity<WateringCan>
            {
                Transform = new Transform(new Vector2(Rand.random.Next(0, 100), Rand.random.Next(0, 100))),
                Size = new Vector2(16)
            });
        }


        public override void RegisterSystems()
        {
            AddSystem<CollisionSystem>();
        }
        public override void Update(GameTime time)
        {

        }
    }
}