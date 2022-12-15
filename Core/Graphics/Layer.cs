using QueefCord.Content.UI;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using QueefCord.Core.IO;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;

namespace QueefCord.Core.Graphics
{
    public class Layer : ISerializable
    {
        private event Action<SpriteBatch> DrawCalls;
        private event Action<SpriteBatch> PrimitiveCalls;

        [JsonIgnore]
        public MapHost MapHost { get; set; }

        [JsonIgnore]
        private RenderTarget2D Target { get; set; }

        public string EffectPath { get; private set; }
        public string ID { get; set; }

        public CameraTransform Camera { get; private set; }

        public float Priority { get; private set; }

        public Rectangle ScissorSource { get; set; }

        public Rectangle Destination { get; set; }

        public SpriteSortMode SortMode { get; set; }

        public Layer(float priority, CameraTransform camera, string effect = "", Rectangle scissor = default, Rectangle destination = default, SpriteSortMode SortMode = SpriteSortMode.Deferred, bool HasMap = false)
        {
            EffectPath = effect;
            Priority = priority;
            Camera = camera;

            ScissorSource = scissor == default ? Renderer.MaxResolutionBounds : scissor;
            Destination = destination == default ? Renderer.MaxResolutionBounds : destination;

            this.SortMode = SortMode;

            if (HasMap)
                MapHost = new MapHost(Renderer.Instance.Content, this);

            Target = new RenderTarget2D(Renderer.Device, Renderer.MaxResolution.X, Renderer.MaxResolution.Y, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }

        public Layer() { }

        public virtual void OnDraw() { }

        public void AppendCall(Action<SpriteBatch> call) => DrawCalls += call;

        public void AppendPrimitiveCall(Action<SpriteBatch> call) => PrimitiveCalls += call;

        public void DrawToTarget(SpriteBatch sb)
        {
            Camera.Update(Time.Current);

            Renderer.Device.SetRenderTarget(Target);
            Renderer.Device.Clear(Color.Transparent);

            sb.Begin(SortMode, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Camera.TransformationMatrix);
            DrawCalls?.Invoke(sb);
            sb.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Camera.TransformationMatrix);
            PrimitiveCalls?.Invoke(sb);
            sb.End();

            DrawCalls = null;
            PrimitiveCalls = null;

            if (MapHost == null) return;

            if (Renderer.Device != null) MapHost.Maps.OrderedRenderPassBatched(sb, Renderer.Device);
            Target = MapHost.Maps.OrderedShaderPass(sb, Target);
        }

        public void Draw(SpriteBatch sb)
        {
            OnDraw();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            if (Assets<Effect>.Has(EffectPath))
                Assets<Effect>.Get(EffectPath).GetValue()?.CurrentTechnique.Passes[0]?.Apply();

            sb.Draw(Target, Destination, ScissorSource, Color.White);

            sb.End();


        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(Priority);
            Camera.Save(bw);
            bw.Write(EffectPath);
            bw.Write(ScissorSource);
            bw.Write(Destination);

            bw.Write(ID);
            bw.Write(MapHost != null);
        }

        public IComponent Load(BinaryReader br)
        {
            Layer layer = 
                new Layer(br.ReadSingle(), ContentWriter.Load<CameraTransform>(br), br.ReadString(), br.ReadRect(), br.ReadRect())
                {
                    ID = br.ReadString()
                };

            layer.MapHost = br.ReadBoolean() ? new MapHost(Renderer.Instance.Content, layer) : null;

            return layer;
        }
    }
}