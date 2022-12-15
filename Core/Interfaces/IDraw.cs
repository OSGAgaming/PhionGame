using Microsoft.Xna.Framework.Graphics;

namespace QueefCord.Core.Interfaces
{
    public interface IDraw : IComponent
    {
        void Draw(SpriteBatch sb);

        string Layer { get; }
    }
}