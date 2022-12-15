

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
            /*
            Texture2D tex = LayerHost.GetLayer("Default").MapHost.Maps.Get("UpscaledTileLightingMap").MapTarget;

            //Utils.DrawRectangle(new Rectangle(MiniMap.Position, tex.Bounds.Size));
            sb.Draw(tex, new Rectangle(Point.Zero, new Point(300,300)), Color.White);
            //sb.Draw(tex2, new Rectangle(MiniMap.Position, tex2.Bounds.Size), Color.White);
            */
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


