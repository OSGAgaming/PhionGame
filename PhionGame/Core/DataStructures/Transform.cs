using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QueefCord.Core.DataStructures
{
	public struct Transform
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Transform(Vector2 pos = default, float rot = 0f, Vector2 scale = default)
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
}
