using QueefCord.Core.Graphics;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using QueefCord.Core.Tiles;
using QueefCord.Content.UI;
using QueefCord.Core.Entities;

namespace QueefCord.Content.Graphics
{
    public class TileTextureMap : PostProcessingPass
    {
        protected override string MapEffectName => "Effects/TileMapEffect";
        public override float Priority => 1;

        internal override void OnApplyShader()
        {
            MapEffect?.Parameters["TileTexture"]?.SetValue(LayerParent.MapHost.Maps.Get("TileMap").MapTarget);
            MapEffect?.Parameters["LightTexture"]?.SetValue(LayerParent.MapHost.Maps.Get("UpscaledTileLightingMap").MapTarget);
            MapEffect?.CurrentTechnique.Passes[0].Apply();
        }
    }
    public class TileMap : PostProcessingPass { public override float Priority => 0; }

    public class UpscaledTileLightingMap : PostProcessingPass
    {
        public override Matrix Matrix
        {
            get
            {
                int res = TileManager.drawResolution;

                CameraTransform Camera = Parent.Parent.Camera;
                Vector2 remainder = new Vector2(
                    (Camera.Transform.Position.X) % res,
                    (Camera.Transform.Position.Y) % res);

                //if (remainder.X == 15) remainder.X = 0;
                //if (remainder.Y == 15) remainder.Y = 0;

                return Matrix.CreateTranslation(new Vector3(-remainder,0));
            }
        }
        public override float Priority => 0.5f;
        public override SamplerState SamplerState => SamplerState.LinearClamp;
    }

    public class TileLightingMap : PostProcessingPass
    {
        public override float Priority => 0;
        public override Matrix Matrix => Matrix.CreateTranslation(
            Vector3.Ceiling(new Vector3(-Parent.Parent.Camera.Transform.Position / TileManager.drawResolution, 0)));

        public override void Load()
        {
            MapTarget = new RenderTarget2D(Renderer.Device,
                Renderer.BackBufferSize.X / TileManager.drawResolution,
                Renderer.BackBufferSize.Y / TileManager.drawResolution, false,
                                           Renderer.Device.PresentationParameters.BackBufferFormat,
                                           DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
        }
    }

}