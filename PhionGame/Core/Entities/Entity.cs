using System;
using System.IO;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace QueefCord.Core.Entities
{
    public class Entity : ISerializable
    {
        public Transform Transform;

        public virtual IComponent Load(BinaryReader br) 
        {
            Transform Transform = br.ReadTransform();
            Type type = br.ReadType();

            Entity e = Activator.CreateInstance(type) as Entity;
            e.Transform = Transform;

            return e;
        }
        public virtual void Save(BinaryWriter bw) 
        {
            bw.Write(Transform);
            bw.Write(GetType());
        }
    }
}
