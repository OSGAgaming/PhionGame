
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QueefCord.Core.Input;
using QueefCord;

namespace QueefCord.Core.Interfaces
{
    //Wrapper for classification
    public interface IStaticUpdateable : IUpdate { }


    public interface IUpdate : IComponent
    {    
        void Update(GameTime gameTime);
    }
}
