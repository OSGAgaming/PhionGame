using QueefCord.Core.Graphics;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using QueefCord.Core.Entities;
using System;
using QueefCord.Content.UI;
using QueefCord.Core.Helpers;

namespace QueefCord.Content.Graphics
{
    public class CrepsularOcclusionMap : PostProcessingPass
    {
        public override float Priority => 0.7f;
    }
    public class CrepsularSource : PostProcessingPass 
    {
        protected override string MapEffectName => "Effects/BrightPass";

        public Texture2D LightSource;
        public Vector2 LightScreenSourcePos;
        public Vector2 HalfPixel;
        public float LightSize = 1500;
        public Texture2D OrgScene;

        public override void Load() { }
        public override float Priority => 0.8f;
        internal override void OnApplyShader()
        {
            OrgScene = Parent.Buffers[Parent.CurrentBuffer - 1];

            if (LightSource == null)
                LightSource = Assets<Texture2D>.Get("Textures/Maps/flare");

            MapEffect.Parameters["screenRes"]?.SetValue(Renderer.BackBufferSize.ToVector2());
            MapEffect.Parameters["halfPixel"]?.SetValue(HalfPixel);
            MapEffect.CurrentTechnique = MapEffect.Techniques["LightSourceMask"];

            MapEffect.Parameters["flare"]?.SetValue(LightSource);

            MapEffect.Parameters["SunSize"]?.SetValue(LightSize);
            MapEffect.Parameters["scene"]?.SetValue(Parent.Get("CrepsularOcclusionMap").MapTarget);
            MapEffect.Parameters["lightScreenPosition"]?.SetValue(LightScreenSourcePos);

            MapEffect.CurrentTechnique.Passes[0].Apply();
        }
    }

    public class CrepsularRaysMap : PostProcessingPass
    {
        protected override string MapEffectName => "Effects/LightRays";

        public Vector2 LightScreenSourcePos;
        public float Density = .5f;
        public float Decay = .95f;
        public float Weight = 1.0f;
        public float Exposure = .3f;
        public float BloomThreshold = .02f;
        public Vector2 HalfPixel;

        public override float Priority => 0.9f;

        internal override void OnApplyShader()
        {
            MapEffect.CurrentTechnique = MapEffect.Techniques["LightRayFX"];

            MapEffect.Parameters["halfPixel"]?.SetValue(HalfPixel);
            MapEffect.Parameters["Density"]?.SetValue(Density);
            MapEffect.Parameters["Decay"]?.SetValue(Decay);
            MapEffect.Parameters["Weight"]?.SetValue(Weight);
            MapEffect.Parameters["Exposure"]?.SetValue(Exposure);
            MapEffect.Parameters["lightScreenPosition"]?.SetValue(LightScreenSourcePos);
            MapEffect.Parameters["scene"]?.SetValue(Parent.Get("CrepsularSource").MapTarget);
            MapEffect.Parameters["orgSceneFore"]?.SetValue((Parent.Get("CrepsularSource") as CrepsularSource).OrgScene);
            MapEffect.Parameters["BloomThreshold"]?.SetValue(BloomThreshold);

            MapEffect.CurrentTechnique.Passes[0].Apply();
        }
    }
}