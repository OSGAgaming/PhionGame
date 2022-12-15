using System;
using System.Collections.Generic;
using QueefCord.Content.Entities;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Maths;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace QueefCord.Core.Entities
{
    public class EntityCore : Entity, IUpdate
    {
        [JsonIgnore]
        public List<IEntityModifier> Mechanics = new List<IEntityModifier>();
        //TODO: Remove
        public Transform LastTransform;
        public virtual void Update(GameTime gameTime)
        {
            LastTransform = Transform;

            OnUpdate(gameTime);

            foreach (IEntityModifier iem in Mechanics)
                iem.Update(this, gameTime);
        }

        public void AddMechanic<T>(T ie) where T : IEntityModifier
        {
            foreach (IEntityModifier iem in Mechanics)
                if (iem is T)
                    return;

            Mechanics.Add(ie);
        }
        public bool Has<T>(out T modifier) where T : IEntityModifier
        {
            foreach (IEntityModifier iem in Mechanics)
                if (iem is T)
                {
                    modifier = Get<T>();
                    return true;
                }

            modifier = default(T);
            return false;
        }
        public T Get<T>() where T : IEntityModifier
        {
            foreach (IEntityModifier iem in Mechanics)
                if (iem is T t)
                    return t;

            throw new Exception("No such Modifier Exists");
        }
        public virtual void OnUpdate(GameTime gameTime) { }
    }
}
