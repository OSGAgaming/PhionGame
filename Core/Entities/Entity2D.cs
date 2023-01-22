using System;
using System.IO;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Maths;
using Microsoft.Xna.Framework;

namespace QueefCord.Core.Entities
{
    public class Entity2D : EntityCore
    {
        //For now just AABB
        public Vector2 Size;
        public Point Trim;

        public Action<Entity2D> Collision;

        public bool Active;
        public string id;

        public bool CanCollide => Active;
        public Vector2 Center
        {
            get => Transform.Position + Size / 2;
            set => Transform.Position = value - Size / 2;
        }

        public RectangleF Box => new RectangleF(Transform.Position, Size).Inf(Trim.X, Trim.Y);
        public RectangleF LastBox => new RectangleF(LastTransform.Position, Size).Inf(Trim.X, Trim.Y);

        public virtual void Collide(Entity2D entity)
        {
            OnCollide(entity);
            Collision?.Invoke(entity);
        }

        public virtual void OnCollide(Entity2D entity) { }

        public virtual bool Compare(Entity2D col) => Box.Intersects(col.Box);

        public virtual void PostSave(BinaryWriter bw) { }
        public virtual void PostLoad(BinaryReader br, Entity2D e) { }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            bw.Write(id);
            bw.Write(Size);

            PostSave(bw);
        }

        public override IComponent Read(BinaryReader br)
        {
            Entity2D e2D = base.Read(br) as Entity2D;

            string id = br.ReadString();
            Vector2 Size = br.ReadVector2();

            e2D.Size = Size;
            e2D.id = id;

            PostLoad(br, e2D);

            return e2D;
        }

        public Entity2D(Vector2 Position = default, Vector2 Size = default, string id = "", Action<Entity2D> col = null)
        {
            Transform.Position = Position;

            this.id = id;
            this.Size = Size;

            Collision = col;
            Active = true;
        }
    }
}
