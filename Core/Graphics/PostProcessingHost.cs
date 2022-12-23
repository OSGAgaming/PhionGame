
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QueefCord.Core.Graphics
{
    public class PostProcessingHost
    {
        internal Dictionary<string, PostProcessingPass> MapPasses = new Dictionary<string, PostProcessingPass>();
        public List<RenderTarget2D> Buffers = new List<RenderTarget2D>();
        public RenderTarget2D OriginalBuffer = new RenderTarget2D(Renderer.Device, Renderer.MaxResolution.X, Renderer.MaxResolution.Y);
        public int CurrentBuffer = 0;

        public Layer Parent { get; set; }

        public void Sort() => MapPasses = MapPasses.OrderBy(key => key.Value.Priority).ToDictionary(pair => pair.Key, pair => pair.Value);

        public void OrderedRenderPassBatched(SpriteBatch sb, GraphicsDevice GD, bool Batched = true)
        {
            foreach (KeyValuePair<string, PostProcessingPass> Map in MapPasses)
            {
                var Pass = Map.Value;

                if (Pass.ManualTarget == null && Pass.CanDraw)
                {
                    GD.SetRenderTarget(Pass.MapTarget);
                    GD.Clear(Color.Transparent);

                    sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, Pass.SamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Pass.Matrix);
                    Pass.RenderBatched(sb);
                    sb.End();

                    Pass.RenderPrimitive(sb);
                }

                Pass.CanDraw = true;
            }
        }

        public RenderTarget2D OrderedShaderPass(SpriteBatch sb, RenderTarget2D target)
        {
            Renderer.Device.SetRenderTarget(OriginalBuffer);
            Renderer.Device.Clear(Color.Transparent);

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            sb.Draw(target, Vector2.Zero, Color.White);
            sb.End();
            int a = 0;

            if (MapPasses.Count != 0)
            {
                foreach (KeyValuePair<string, PostProcessingPass> Map in MapPasses)
                {

                    Renderer.Device.SetRenderTarget(Buffers[a]);
                    Renderer.Device.Clear(Color.Transparent);

                    sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: null, samplerState: SamplerState.PointClamp);

                    Map.Value.ApplyShader();

                    RenderTarget2D rT;
                    if (a < 1) rT = target;
                    else
                    {
                        rT = Buffers[a - 1];
                        CurrentBuffer = a - 1;
                    }

                    if (Renderer.Device != null)
                    {
                        Rectangle frame = new Rectangle(0, 0,
                            Renderer.MaxResolution.X,
                            Renderer.MaxResolution.Y);

                        sb.Draw(rT, Vector2.Zero, frame, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }

                    sb.End();

                    a++;
                }

                return Buffers[Buffers.Count - 1];
            }

            return target;
        }
        public void DrawToBatchedMap(string Map, MapRender MR) => MapPasses[Map].DrawToBatchedTarget(MR);
        public void DrawToPrimitiveMap(string Map, MapRender MR) => MapPasses[Map].DrawToPrimitiveTarget(MR);

        public void AddMap(string MapName, PostProcessingPass MP, Layer layer)
        {
            MP.Parent = this;
            MP.LayerParent = layer;

            MapPasses.Add(MapName, MP);

            Buffers.Add(new RenderTarget2D(Renderer.Device, Renderer.MaxResolution.X, Renderer.MaxResolution.Y));
        }

        public PostProcessingPass Get(string MapName) => MapPasses[MapName];

        public PostProcessingPass Get<T>() where T : PostProcessingPass
        {
            //TODO: Support for multiple Passes with different ID's

            foreach (PostProcessingPass pass in MapPasses.Values)
            {
                if (pass is T) return (T)pass;
            }

            throw new System.Exception("Pass does not exist");
        }

    }
}