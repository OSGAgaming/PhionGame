using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QueefCord.Core.Graphics
{
	public class TriMesh : Triangles
	{
		public ref VertexPositionColorTexture FirstVertex => ref vertices[0];

		public ref VertexPositionColorTexture SecondVertex => ref vertices[1];

		public ref VertexPositionColorTexture ThirdVertex => ref vertices[2];

		public TriMesh(Vector2 v1, Vector2 v2, Vector2 v3, Color color, string layer = "Default", Texture2D texture = null, Effect effect = null) : base(3, 3, layer, texture, effect)
		{
			Construct(v1, v2, v3, color);
		}

		public void Construct(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
			AddVertex(v1, color, Vector2.One);
			AddVertex(v2, color, Vector2.Zero);
			AddVertex(v3, color, Vector2.UnitX);

			AddIndex(0);
			AddIndex(1);
			AddIndex(2);

			Finish();
		}
	}
}
