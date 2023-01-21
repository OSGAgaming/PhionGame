using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Content.Entities
{
    public class Projectile : Entity2D
    {
        public virtual int Damage { get; }
        public virtual int TimeAlive { get; }
        public virtual float KnockBack { get; }

        protected int TimeLeft;

        public virtual void OnSpawn() { }

        public static void SpawnProjectile<T>() where T : Projectile, new() => SceneHolder.CurrentScene.AddEntity(new T());

        public static void SpawnProjectile(Projectile proj)
        {
            proj.AddMechanic(new EntityCollision(proj, proj.Size, true, true));
            proj.OnSpawn();
            SceneHolder.CurrentScene.AddEntity(proj);
        }

    }
}
