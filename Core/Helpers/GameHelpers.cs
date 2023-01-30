using QueefCord.Content.Graphics;
using QueefCord.Content.UI;
using QueefCord.Core.Entities;
using QueefCord.Core.Graphics;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueefCord.Core.Helpers
{
    public static partial class Utils
    {
        public static Vector2 DefaultMouseWorld => Mouse.GetState().Position.ToVector2().ToScreen(DefaultCamera());
        public static Vector2 ToScreen(this Vector2 v, CameraTransform c) => v / c.Transform.Scale + c.Transform.Position;
        public static Vector2 ToDefaultScreen(this Vector2 v) => 
            v / LayerHost.GetLayer("Default").Camera.Transform.Scale +
            LayerHost.GetLayer("Default").Camera.Transform.Position;

        public static CameraTransform DefaultCamera() => LayerHost.GetLayer("Default").Camera;
        public static Vector2 ToScreen(this Vector2 v, string LayerName) => 
            v / LayerHost.GetLayer(LayerName).Camera.Transform.Scale + 
            LayerHost.GetLayer(LayerName).Camera.Transform.Position;
        public static Vector2 ToMiniMap(this Vector2 v) => v / MiniMap.Resolution;
        public static Vector2 ToMiniMap(this Point v) => v.Multiply(new Vector2(1 / (float)MiniMap.Resolution)).ToVector2();
        public static Rectangle ToMiniMap(this Rectangle r) => r.Multiply(1 / (float)MiniMap.Resolution);

        public static Texture2D pixel => Assets<Texture2D>.Get("pixel");
    }
}
