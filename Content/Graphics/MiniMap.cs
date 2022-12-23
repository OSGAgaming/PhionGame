using QueefCord.Core.Entities;
using QueefCord.Core.Graphics;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace QueefCord.Content.Graphics
{
    public class MiniMap : PostProcessingPass 
    {
        public override float Priority => 0;

        public override Matrix Matrix => Matrix.CreateTranslation(new Vector3(-Parent.Parent.Camera.Transform.Position / Resolution, 0));

        public static Point Position = new Point(Renderer.BackBufferSize.X - 105, 5);
        public static Point Size = new Point(100);
        public const int Resolution = 8;

        public override void Load()
        {
            MapTarget = new RenderTarget2D(Renderer.Device, Size.X, Size.Y);
        }
    }
}