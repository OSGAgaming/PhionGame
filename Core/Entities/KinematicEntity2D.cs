using System;
using System.IO;
using QueefCord.Content.Entities;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Helpers;
using QueefCord.Core.Maths;
using Microsoft.Xna.Framework;

namespace QueefCord.Core.Entities
{
    public class KinematicEntity2D : Entity2D
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Friction = Vector2.One * 0.9f;

        public override void Update(GameTime gameTime)
        {
            LastTransform = Transform;

            Transform.Position += Velocity;
            Velocity += Acceleration;

            Velocity *= Friction;

            OnUpdate(gameTime);

            foreach (IEntityModifier iem in Mechanics)
                iem.Update(this, gameTime);
        }
    }
}
