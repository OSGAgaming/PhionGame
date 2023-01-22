using System;
using System.Collections.Generic;
using QueefCord.Content.Entities;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Maths;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using QueefCord.Core.Helpers;

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

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);

            bw.Write(Mechanics.Count);

            foreach (IEntityModifier iem in Mechanics)
            {
                if(iem is ISerializable ser)
                {
                    bw.Write(iem.GetType());
                    ser.Write(bw);
                }
            }
        }

        public override IComponent Read(BinaryReader br)
        {
            EntityCore ic = base.Read(br) as EntityCore;

            int mecCount = br.ReadInt32();

            for(int i = 0; i < mecCount; i++)
            {
                Type t = br.ReadType();

                ISerializable modifier = (ISerializable)Activator.CreateInstance(t);
                IComponent component = modifier.Read(br);

                ic.Mechanics.Add(component as IEntityModifier);
            }

            return ic;
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
