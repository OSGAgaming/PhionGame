using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace QueefCord.Core.Graphics
{
    // OpenGL is a specification developed by the Khronos Group in 1992 specifying a flexible and efficient graphics API used to interface with a computer's graphics processo
    public interface IMeshComponent : IDraw { }

    public class Triangles : IMeshComponent
    {
        protected VertexPositionColorTexture[] vertices;
        protected short[] indices;
        private int vertexPointer;
        private int indexPointer;
        private bool finished;
        private string layer;
        private Texture2D texture;

        private Effect effect;
        private Action<Effect> param;

        public string Layer => layer;

        public Triangles(int vertexCount, int indexCount, string layer, Texture2D texture = null, Effect effect = null, Action<Effect> param = null)
        {
            vertices = new VertexPositionColorTexture[vertexCount];
            indices = new short[indexCount];
            this.layer = layer;
            this.effect = effect;
            this.texture = texture;
            this.param = param;
        }

        public void AddVertex(Vector2 position, Color color, Vector2 uv)
        {
            ResetIfFinished();

            vertices[vertexPointer++] = new VertexPositionColorTexture(new Vector3(position, 0), color, uv);

            if (vertexPointer == vertices.Length + 1)
                Array.Resize(ref vertices, vertices.Length * 2);
        }

        public void AddIndex(short index)
        {
            ResetIfFinished();

            indices[indexPointer++] = index;

            if (indexPointer == indices.Length + 1)
                Array.Resize(ref indices, indices.Length * 2);
        }

        public void Finish() => finished = true;

        public void Reset()
        {
            Array.Clear(vertices, 0, vertices.Length);
            Array.Clear(indices, 0, indices.Length);
            vertexPointer = 0;
            indexPointer = 0;
        }

        private void ResetIfFinished()
        {
            if (finished)
            {
                finished = false;
                Reset();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            BasicEffect basicEffect = Assets<Effect>.Get("BasicEffect").GetValue() as BasicEffect;
            Layer currentlayer = LayerHost.GetLayer(layer);

            basicEffect.TextureEnabled = texture != null;
            basicEffect.Texture = texture;
            basicEffect.VertexColorEnabled = true;

            int width = Renderer.ViewportSize.X;
            int height = Renderer.ViewportSize.Y;

            Matrix view = 
                Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * 
                Matrix.CreateTranslation(
                    width / 2 + currentlayer.Camera.Transform.Position.X, 
                    height / -2 - currentlayer.Camera.Transform.Position.Y, 0) * 
                Matrix.CreateRotationZ(MathHelper.Pi) *
                Matrix.CreateScale(1f, 1f, 1f);

            Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

            basicEffect.View = view;
            basicEffect.Projection = projection;

            VertexBuffer vertexBuffer = new VertexBuffer(sb.GraphicsDevice, typeof(VertexPositionColorTexture), vertexPointer, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            IndexBuffer indexBuffer = new IndexBuffer(sb.GraphicsDevice, typeof(short), indexPointer, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);

            sb.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            sb.GraphicsDevice.Indices = indexBuffer;

            if (effect == null)
            {
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    sb.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexPointer / 3);
                }
            }
            else
            {
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    param.Invoke(effect);

                    pass.Apply();
                    sb.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexPointer / 3);
                }
            }
        }
    }
}