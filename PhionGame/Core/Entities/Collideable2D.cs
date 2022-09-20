using System;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Maths;
using Microsoft.Xna.Framework;
using QueefCord.Core.Helpers;
using QueefCord.Content.UI;
using QueefCord.Content.Entities;

namespace QueefCord.Core.Entities
{
    public class Collideable2D : Entity2D
    {
        public bool Trigger { get; set; }
        public bool Static { get; set; }
        public Entity2D BindedEntity { get; set; }

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
            BindedEntity = entity;
            RelativeCollisionFrame = rect;

            Size = RelativeCollisionFrame.Size;
            Transform.Position = CollisionFrame.TL;

            if(BindedEntity != null) id = BindedEntity.id;

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

            if (Static || !(entity is Collideable2D c) || c.Trigger || BindedEntity == null)
                return;

            RectangleF A = CollisionFrame;
            RectangleF B = entity.Box;

            Vector2 vel = BindedEntity.LastTransform.Position - BindedEntity.Transform.Position;

            RectangleF ALast = CollisionFrame.AddPos(vel);

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

            float epsilon = 0.1f;

            if (ALast.x + epsilon >= B.right)
            {
                d += new Vector2(B.right - A.x, 0);
                SetVelX0();
            }
            else if (ALast.right - epsilon <= B.x)
            {
                d += new Vector2(B.x - A.right, 0);
                SetVelX0();
            }

            if (ALast.y + epsilon >= B.bottom)
            {
                d += new Vector2(0, B.bottom - A.y);
                SetVelY0();
            }
            else if (ALast.bottom - epsilon <= B.y)
            {
                d += new Vector2(0, B.y - A.bottom);
                SetVelY0();
            }

            if (BindedEntity is KinematicEntity2D k && d != Vector2.Zero) Colliding = true;
            else Colliding = false;

            BindedEntity.Transform.Position += d;
        }
    }
}
