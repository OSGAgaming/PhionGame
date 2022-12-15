using QueefCord.Core.Interfaces;
using QueefCord.Core.Scenes;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QueefCord.Core.Graphics
{
    public static class LayerHost
    {
        public static Dictionary<string, Layer> layers = new Dictionary<string, Layer>();

        private static void Order() => layers.OrderBy(n => -n.Value.Priority);

        public static void RegisterLayer(Layer layer, string name)
        {
            layers.Add(name, layer);
            layer.ID = name;

            Order();
        }

        public static void DrawLayers(SpriteBatch sb)
        {
            foreach (Layer layer in layers.Values)
                layer.Draw(sb);
        }

        public static void DrawLayersToTarget(Scene scene, SpriteBatch sb)
        {
            foreach (IDraw entity in scene.Drawables)
            {
                if (entity is IMeshComponent)
                    layers[entity.Layer ?? "Default"].AppendPrimitiveCall(entity.Draw);
                else
                    layers[entity.Layer ?? "Default"].AppendCall(entity.Draw);
            }

            foreach (Layer layer in layers.Values)
                layer.Camera.HasUpdated = false;

            foreach (Layer layer in layers.Values)
                layer.DrawToTarget(sb);
        }

        public static Layer GetLayer(string layerName) => layers[layerName ?? "Default"];
    }
}