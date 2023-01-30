using QueefCord.Core.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using QueefCord.Core.Interfaces;

namespace QueefCord.Core.Helpers
{
    public static partial class Helpers
    {
        public static void Write(this BinaryWriter binaryWriter, Point point)
        {
            binaryWriter.Write(point.X);
            binaryWriter.Write(point.Y);
        }
        public static void Write(this BinaryWriter binaryWriter, Vector2 point)
        {
            binaryWriter.Write(point.X);
            binaryWriter.Write(point.Y);
        }
        public static void Write(this BinaryWriter binaryWriter, Rectangle rect)
        {
            binaryWriter.Write(rect.Location);
            binaryWriter.Write(rect.Size);
        }
        public static void Write(this BinaryWriter binaryWriter, RectangleF rect)
        {
            binaryWriter.Write(rect.TL);
            binaryWriter.Write(rect.Size);
        }
        public static void Write(this BinaryWriter binaryWriter, Color color)
        {
            binaryWriter.Write(color.R);
            binaryWriter.Write(color.G);
            binaryWriter.Write(color.B);
        }
        public static void Write(this BinaryWriter binaryWriter, Vector4 vec)
        {
            binaryWriter.Write(vec.X);
            binaryWriter.Write(vec.Y);
            binaryWriter.Write(vec.Z);
            binaryWriter.Write(vec.W);
        }
        public static void Write(this BinaryWriter binaryWriter, Transform transform)
        {
            binaryWriter.Write(transform.Position);
            binaryWriter.Write(transform.Rotation);
            binaryWriter.Write(transform.Scale);
        }
        public static void WriteObject(this BinaryWriter binaryWriter, object obj)
        {
            if (obj is ISerializable s)
            {
                binaryWriter.Write(obj.GetType());
                s.Write(binaryWriter);
            }
        }
        public static void Write(this BinaryWriter binaryWriter, Type type)
        {
            binaryWriter.Write(type?.FullName ?? "throw");
        }
        public static Point ReadPoint(this BinaryReader binaryReader)
        {
            return new Point(binaryReader.ReadInt32(), binaryReader.ReadInt32());
        }
        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
        public static Vector4 ReadVector4(this BinaryReader binaryReader)
        {
            return new Vector4(
                binaryReader.ReadSingle(), binaryReader.ReadSingle(),
                binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
        public static Rectangle ReadRect(this BinaryReader binaryReader)
        {
            return new Rectangle(binaryReader.ReadPoint(), binaryReader.ReadPoint());
        }
        public static RectangleF ReadRectF(this BinaryReader binaryReader)
        {
            return new RectangleF(binaryReader.ReadVector2(), binaryReader.ReadVector2());
        }
        public static Color ReadColor(this BinaryReader binaryReader)
        {
            return new Color(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
        }
        public static Transform ReadTransform(this BinaryReader binaryReader)
        {
            return new Transform(binaryReader.ReadVector2(), binaryReader.ReadSingle(), binaryReader.ReadVector2());
        }

        public static T ReadObject<T>(this BinaryReader binaryReader) where T : class, ISerializable, IComponent, new()
        {
            T t = new T();
            return t.Read(binaryReader) as T;
        }

        public static Type? ReadType(this BinaryReader binaryReader)
        {
            string TypeName = binaryReader.ReadString();

            Debug.WriteLine(TypeName);

            if (TypeName == "throw") throw new TypeAccessException();
            else return Type.GetType(TypeName);
        }

        public static void ReadObject(this BinaryReader binaryReader)
        {
            Type t = binaryReader.ReadType();
            ISerializable serializable = Activator.CreateInstance(t) as ISerializable;
            serializable.Read(binaryReader);
        }
    }
}
