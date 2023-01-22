using QueefCord.Content.UI;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QueefCord.Core.IO
{
    public static class ContentWriter
    {
        public static IComponent Load(Type type, BinaryReader br)
        {
            object o = Activator.CreateInstance(type);
            if (o is ISerializable s)
                return s.Read(br);

            throw new Exception("Unserializable");

        }

        public static T Load<T>(Type type, BinaryReader br)
        {
            object o = Activator.CreateInstance(type);
            if (o is ISerializable s)
                return (T)s.Read(br);

            throw new Exception("Unserializable");

        }

        public static T Load<T>(BinaryReader br) where T : IComponent => Load<T>(typeof(T), br);

        public static IComponent Load<T>(string type, BinaryReader br) => Load(Type.GetType(type), br);
    }
}