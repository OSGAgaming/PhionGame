

using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.IO;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace QueefCord.Content.UI
{
    internal class SavingUI : UIScreen
    {
        WorldLoadScroll[]? Load;
        public static bool Active;

        internal override void DrawToScreen()
        {
            Utils.DrawTextToLeft("Save World As:", Color.Yellow, new Vector2(10, 10), 0.1f);

            Utils.DrawTextToLeft("Load World", Color.Yellow, new Vector2(10, 75), 0.1f);
        }

        protected override void OnUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && GameInput.Instance["S"].IsJustPressed())
            {
                LevelInfo.SaveCurrentLevelToJSON(Path.GetFileName("CurrentWorld"));
                Logger.NewText("Current world saved");
            }

            if (GameInput.Instance["Alt"].IsJustPressed())
            {
                Active = !Active;
            }
        }

        protected override void OnLoad()
        {
            string[] files = Directory.GetFiles(Utils.LocalWorldPath, "*.mgsc");
            Load = new WorldLoadScroll[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Load[i] = new WorldLoadScroll();
                Load[i].index = i;
                Load[i].path = files[i];

                elements.Add(Load[i]);
            }
        }

        internal override void OnDrawToScreenDirect() { }
    }

    internal class WorldLoadScroll : UIElement
    {
        public string path = "";
        public int index;

        float X = -200;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SavingUI.Active)
                X = X.ReciprocateTo(20);
            else
                X = X.ReciprocateTo(-200);

            dimensions = new Rectangle((int)X, 100 + 50*index, 120, 30);
            Utils.DrawTextToLeft(Path.GetFileName(path), Color.White, new Vector2(dimensions.X, dimensions.Y));

        }
        protected override void OnLeftClick()
        {
            LevelInfo.LoadLevelFromJSON(Path.GetFileName(path).Replace(".mgsc", ""));
        }
    }

}


