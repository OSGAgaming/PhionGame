
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace QueefCord.Core.UI
{
    public class UIScreenManager : IUpdate, IDraw
    {
        public static UIScreenManager? Instance;

        internal List<UIScreen> Components = new List<UIScreen>();

        public string Layer => "UI";

        public virtual void Update(GameTime gameTime)
        {
            foreach (UIScreen foo in Components)
            {
                if (foo != null)
                    foo.Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIScreen foo in Components)
            {
                if (foo != null)
                    foo.Draw(spriteBatch);
            }
        }

        public void DrawOnScreen()
        {
            foreach (UIScreen UIS in Components)
            {
                UIS.DrawToScreen();
            }
        }

        public void DrawDirectOnScreen(SpriteBatch sb)
        {
            foreach (UIScreen UIS in Components)
            {
                UIS.DrawToScreenDirect(sb);
            }
        }

        public T GetScreen<T>() where T : UIScreen
        {
            foreach(UIScreen screen in Components)
            {
                if (screen is T) return screen as T;
            }

            throw new Exception("Could not find Screen");
        }

        public void AddComponent(UIScreen Component)
        => Components.Add(Component);

        public void RemoveComponent(int index)
        => Components.RemoveAt(index);
        public void RemoveComponent(UIScreen instance)
        => Components.Remove(instance);

        static UIScreenManager()
        {
            Instance = new UIScreenManager();

            foreach (Type type in Utils.GetInheritedClasses(typeof(UIScreen)))
            {
                UIScreen? Screen = Activator.CreateInstance(type) as UIScreen;
            }
        }
    }
}
