

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
using QueefCord.Core.Scenes;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Graphics;
using QueefCord.Content.Graphics;

namespace QueefCord.Content.UI
{
    public interface IMappable
    {
        void DrawToMiniMap(SpriteBatch sb) { }
    }

    internal class MiniMapUI : UIScreen
    {
        protected override void PreDraw(SpriteBatch sb)
        {             
            foreach(IComponent component in SceneHolder.CurrentScene.DistinctElements)
            {
                if (component is IMappable m)
                 LayerHost.GetLayer("Default").MapHost.Maps.Get("MiniMap").DrawToBatchedTarget
                 ((sb) =>
                 {
                     m.DrawToMiniMap(sb);
                 });
            }
            
            Point p = (LayerHost.GetLayer("Default").Camera.Transform.Position / MiniMap.Resolution).ToPoint();

            Utils.DrawRectangle(new Rectangle(MiniMap.Position, MiniMap.Size));

            sb.Draw(
                LayerHost.GetLayer("Default").MapHost.Maps.Get("MiniMap").MapTarget,
                new Rectangle(MiniMap.Position, MiniMap.Size), new Rectangle(Point.Zero, MiniMap.Size), Color.White);
        }
        protected override void OnUpdate()
        {
            
        }

        protected override void OnLoad()
        {

        }

        internal override void OnDrawToScreenDirect() { }
    }
}


