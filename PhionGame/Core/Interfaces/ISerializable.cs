
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
        public IComponent Load(BinaryReader br);
        public void Save(BinaryWriter bw);

    }
}
