
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QueefCord.Core.Input;
using QueefCord;
using System.IO;

namespace QueefCord.Core.Interfaces
{
    //Wrapper for classification
    public interface ISerializable : IComponent
    {
        public IComponent Read(BinaryReader br);
        public void Write(BinaryWriter bw);

    }
}
