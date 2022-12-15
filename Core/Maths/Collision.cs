using QueefCord.Core.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace QueefCord.Core.Maths
{
    public enum Bound
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }
    public struct CollisionInfo
    {
        public Bound AABB;
        public Vector2 d;
        public static CollisionInfo Default => new CollisionInfo(Vector2.Zero, Bound.None);
        public CollisionInfo(Vector2 d, Bound AABB)
        {
            this.AABB = AABB;
            this.d = d;
        }
    }


    public static class Collision
    {
        public static bool Intersects(this RectangleF r1, RectangleF r2) => r1.x < r2.right && r1.right > r2.x && r1.y < r2.bottom && r1.bottom > r2.y;
    }
}
