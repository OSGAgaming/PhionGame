
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QueefCord.Core.Graphics
{
    public class Map
    {
        internal Dictionary<string, MapPass> MapPasses = new Dictionary<string, MapPass>();
        public List<RenderTarget2D> Buffers = new List<RenderTarget2D>();

        public Layer Parent { get; set; }

        public void Sort() => MapPasses.OrderBy(key => key.Value.Priority);

        public void OrderedRenderPassBatched(SpriteBatch sb, GraphicsDevice GD, bool Batched = true)
        {
            int i = 0;

            for (int a = 0; a < MapPasses.Count; a++)
            {
                foreach (KeyValuePair<string, MapPass> Map in MapPasses)
                {
                    var Pass = Map.Value;

                    if (Pass.Priority != i) continue;

                    if (Pass.ManualTarget == null)
                    {
                        GD.SetRenderTarget(Pass.MapTarget);
                        GD.Clear(Color.Transparent);

                        sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Pass.Matrix);
                        Pass.RenderBatched(sb);
                        sb.End();

                        Pass.RenderPrimitive(sb);
                    }
                }

                i++;
            }
        }

        public RenderTarget2D OrderedShaderPass(SpriteBatch sb, RenderTarget2D target)
        {
            if (MapPasses.Count != 0)
            {
                int a = 0;
                foreach (KeyValuePair<string, MapPass> Map in MapPasses)
                {
                    Renderer.Device.SetRenderTarget(Buffers[a]);
                    Renderer.Device.Clear(Color.Transparent);

                    sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: null, samplerState: SamplerState.PointClamp);

                    Map.Value.ApplyShader();

                    RenderTarget2D rT;
                    if (a < 1) rT = target; else rT = Buffers[a - 1];

                    if (Renderer.Device != null)
                    {
                        Rectangle frame = new Rectangle(0, 0,
                            (int)(Renderer.MaxResolution.X),
                            (int)(Renderer.MaxResolution.Y));

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

        public void AddMap(string MapName, MapPass MP, Layer layer)
        {
            MP.Parent = this;
            MP.LayerParent = layer;

            MapPasses.Add(MapName, MP);

            Buffers.Add(new RenderTarget2D(Renderer.Device, Renderer.MaxResolution.X, Renderer.MaxResolution.Y));
        }

        public MapPass Get(string MapName) => MapPasses[MapName];

        public MapPass Get<T>() where T : MapPass
        {
            //TODO: Support for multiple Passes with different ID's

            foreach (MapPass pass in MapPasses.Values)
            {
                if (pass is T) return (T)pass;
            }

            throw new System.Exception("Pass does not exist");
        }

    }
}