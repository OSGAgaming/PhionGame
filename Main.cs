using QueefCord.Content.Controls;
using QueefCord.Content.Entities;
using QueefCord.Content.Scenes;
using QueefCord.Content.UI;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using QueefCord.Core.IO;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.Tiles;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace QueefCord
{
    public class Main : Game
    {
        public static GraphicsDeviceManager Graphics => Renderer.GraphicsDeviceManager;
        public static SpriteBatch Spritebatch => Renderer.Spritebatch;

        public static List<IStaticUpdateable> StaticUpdateables;

        public Main()
        {
            Renderer.InitializeGraphics(this);
            AssetServer.GraphicsDevice = Renderer.GraphicsDeviceManager.GraphicsDevice;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        //protected override void OnExiting(object sender, EventArgs args) => LevelInfo.SaveCurrentLevelToJSON("CurrentWorld");
        

        protected override void LoadContent()
        {
            Renderer.PrepareRenderer();
            AssetServer.Start(Content);

            Texture2D pixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            
            Assets<Texture2D>.Register("pixel", pixel);

            StaticUpdateables = new List<IStaticUpdateable>();

            //TODO: leave for now but get rid of later
            PropManager.Instance = PropManager.Instance;

            try
            {
                LevelInfo.LoadLevel("CurrentWorld");
            }
            catch
            {
                SceneHolder.StartScene(new OrthoTestScene());
            }

            //SceneHolder.StartScene(new TestScene());

            foreach (Type type in Utils.GetInheritedClasses(typeof(IStaticUpdateable)))
            {
                type.TypeInitializer.Invoke(null, null);
            }

            RegisterControls.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Time.Current = gameTime;

            foreach (IStaticUpdateable update in StaticUpdateables)
                update.Update(gameTime);

            // TODO: Add your update logic here
            SceneHolder.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Renderer.DrawScene(SceneHolder.CurrentScene);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
