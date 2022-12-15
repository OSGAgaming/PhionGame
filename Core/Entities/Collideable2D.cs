using System;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Maths;
using Microsoft.Xna.Framework;
using QueefCord.Core.Helpers;
using QueefCord.Content.UI;
using QueefCord.Content.Entities;
using System.Collections.Generic;

namespace QueefCord.Core.Entities
{
    public enum Direction
    { 
        None,
        Up,
        Right,
        Down = 4,
        Left = 8
    }
    public struct CollisionInfo
    {
        public Vector2 Resolve;
        public byte DirectionMask;

        public CollisionInfo(Vector2 r, byte d = 0)
        {
            Resolve = r;
            DirectionMask = d;
        }
    }

    public class Collideable2D : Entity2D
    {
        public bool Trigger { get; set; }
        public bool Static { get; set; }
        public Entity2D BindedEntity { get; set; }
        public CollisionInfo CollisionInfo;

        public bool Colliding { get; set; }

        public RectangleF RelativeCollisionFrame;
        public RectangleF CollisionFrame
        {
            get
            {
                if (BindedEntity != null)
                    return RelativeCollisionFrame.AddPos(BindedEntity.Transform.Position);
                else
                    return RelativeCollisionFrame;
            }
        }

        public Vector2 LastPosition;

        public Collideable2D(Entity2D entity, Rectangle rect, bool Static = false, bool Trigger = false)
        {
            CollisionInfo = new CollisionInfo(Vector2.Zero);

            BindedEntity = entity;
            RelativeCollisionFrame = rect;

            Size = RelativeCollisionFrame.Size;
            Transform.Position = CollisionFrame.TL;

            if (BindedEntity != null) id = BindedEntity.id;

            this.Static = Static;
            this.Trigger = Trigger;
        }
        public override bool Compare(Entity2D col)
        {
            if (col is Collideable2D c)
                return CollisionFrame.Intersects(c.CollisionFrame);

            return CollisionFrame.Intersects(col.Box);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            Size = RelativeCollisionFrame.Size;
            Transform.Position = CollisionFrame.TL;
        }

        public override void Collide(Entity2D entity)
        {
            if (entity is Collideable2D collideable && collideable.BindedEntity != null)
            {
                BindedEntity?.OnCollide(collideable.BindedEntity);
                Collision?.Invoke(collideable.BindedEntity);
            }

            if (Static || entity is not Collideable2D c || c.Trigger || BindedEntity == null)
                return;

            RectangleF A = CollisionFrame;
            RectangleF B = entity.Box;

            Vector2 vel = BindedEntity.LastTransform.Position - BindedEntity.Transform.Position;

            RectangleF ALast = CollisionFrame.AddPos(vel * 2);

            Vector2 d = Vector2.Zero;

            void SetVelX0()
            {
                if (BindedEntity.Has(out RigidBody modifier))
                    modifier.Velocity.X = 0;
            }

            void SetVelY0()
            {
                if (BindedEntity.Has(out RigidBody modifier))
                    modifier.Velocity.Y = 0;
            }

            if (ALast.bottom > B.y && ALast.y < B.bottom)
            {
                if (ALast.x > B.Center.X)
                {
                    d = new Vector2(B.right - A.x, 0);
                    CollisionInfo = CollisionInfo.AddDirection(Direction.Left);
                    SetVelX0();
                }
                if (ALast.right < B.Center.X)
                {
                    d = new Vector2(B.x - A.right, 0);
                    CollisionInfo = CollisionInfo.AddDirection(Direction.Right);

                    SetVelX0();
                }
            }

            if (ALast.x < B.right && ALast.right > B.x)
            {
                if (ALast.y > B.Center.Y)
                {
                    d = new Vector2(0, B.bottom - A.y);
                    CollisionInfo = CollisionInfo.AddDirection(Direction.Up);
                    SetVelY0();
                }
                if (ALast.bottom < B.Center.Y)
                {
                    d = new Vector2(0, B.y - A.bottom);

                    CollisionInfo = CollisionInfo.AddDirection(Direction.Down);

                    SetVelY0();
                }
            }

            if (BindedEntity is KinematicEntity2D && d != Vector2.Zero) Colliding = true;
            else Colliding = false;

            BindedEntity.Transform.Position += d;
            CollisionInfo = CollisionInfo.AddResolve(d);
        }
    }
}
