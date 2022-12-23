
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Core.Graphics
{
    public delegate void MapRender(SpriteBatch spriteBatch);
    public class PostProcessingPass
    {
        //These are events split into sb and gd calls
        internal event MapRender BatchedCalls;

        internal event MapRender PrimitiveCalls;


        public RenderTarget2D MapTarget;
        public PostProcessingHost? Parent;


        public virtual Matrix Matrix => Parent.Parent.Camera.TransformationMatrix;

        public virtual SamplerState SamplerState { get; } = SamplerState.PointClamp;

        public virtual RenderTarget2D ManualTarget => null;
        protected Effect? MapEffect => Assets<Effect>.Get(MapEffectName);


        public virtual float Priority { get; }

        public virtual bool CanDraw { get; set; } = true;

        protected virtual string MapEffectName { get; }

        public Layer LayerParent { get; set; }

        internal virtual void OnApplyShader()
        {
            MapEffect?.CurrentTechnique.Passes[0].Apply();
        }

        public void ApplyShader()
        {
            if (MapEffectName == null) return;

            MapEffect?.Parameters["screenPosition"]?.SetValue(LayerParent.Camera.Transform.Position);
            MapEffect?.Parameters["screenScale"]?.SetValue(LayerParent.Camera.Transform.Scale);
            MapEffect?.Parameters["screenSize"]?.SetValue(Renderer.BackBufferSize.ToVector2());
            MapEffect?.Parameters["Map"]?.SetValue(MapTarget);
            MapEffect?.Parameters["Time"]?.SetValue(Time.Current.TotalGameTime.Milliseconds);
            //MapEffect?.Parameters["noiseMap"]?.SetValue();

            OnApplyShader();
        }

        public void DrawToBatchedTarget(MapRender method) => BatchedCalls += method;

        public void DrawToPrimitiveTarget(MapRender method) => PrimitiveCalls += method;

        public void RenderBatched(SpriteBatch spriteBatch)
        {
            BatchedCalls?.Invoke(spriteBatch);
            BatchedCalls = null;
        }

        public void RenderPrimitive(SpriteBatch spriteBatch)
        {
            PrimitiveCalls?.Invoke(spriteBatch);
            PrimitiveCalls = null;
        }

        public PostProcessingPass()
        {
            Load();
        }

        public virtual void Load()
        {
            MapTarget = new RenderTarget2D(Renderer.Device, Renderer.MaxResolution.X, Renderer.MaxResolution.Y);
        }
    }
}