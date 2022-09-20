using QueefCord.Core.Entities;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using QueefCord.Core.Helpers;
using QueefCord.Core.IO;
using System.Diagnostics;
using QueefCord.Content.UI;
using Newtonsoft.Json;

namespace QueefCord.Core.Scenes
{
    public class Scene : ISerializable
    {
        public int TimeAlive;

        [JsonIgnore]
        public IList<IDraw> Drawables = new List<IDraw>();
        [JsonIgnore]
        public IList<IUpdate> Updateables = new List<IUpdate>();

        public IList<IEntitySystem> Systems = new List<IEntitySystem>();

        //For serializing
        public IList<IComponent> DistinctElements = new List<IComponent>();


        public void UpdateTickables(GameTime time)
        {
            foreach (IUpdate tickable in Updateables.ToArray())
                tickable.Update(time);
        }

        public void AddEntity(IComponent entity)
        {
            if (entity is IUpdate tickable)
                Updateables.Add(tickable);

            if (entity is IDraw drawable)
                Drawables.Add(drawable);

            DistinctElements.Add(entity);

            AddEntityToSystems(entity);
        }

        public void UpdateSystems(GameTime time)
        {
            foreach (IEntitySystem system in Systems)
                system.Update(time);
        }

        public void AddEntityToSystems(IComponent entity)
        {
            foreach (IEntitySystem e in Systems)
            {
                foreach (Type t in e.GetType().BaseType.GetGenericArguments())
                {
                    if (entity.GetType().IsSubclassOf(t))
                    {
                        e.AddEntity(entity);
                        break;
                    }
                }
            }
        }

        public void AddSystem<T>() where T : IEntitySystem, new()
        {
            T system = new T();

            Systems.Add(system);
            Drawables.Add(system);
        }

        public void AddSystem(IEntitySystem ies)
        {
            Systems.Add(ies);
            Drawables.Add(ies);
        }

        public T GetSystem<T>() where T : IEntitySystem
        {
            foreach (IEntitySystem e in Systems)
            {
                if (e is T t) return t;
            }

            throw new NullReferenceException("Could not find system");
        }

        public void Activate()
        {
            RegisterSystems();
            OnActivate();

            foreach (IEntitySystem system in Systems)
                system.Load();
        }

        public void RemoveDrawable(IDraw draw) => Drawables.Remove(draw);

        public void RemoveUpdateable(IUpdate update) => Updateables.Remove(update);

        public void RemoveEntity(IComponent entity)
        {
            if (entity is IUpdate tickable)
                RemoveUpdateable(tickable);

            if (entity is IDraw drawable)
                RemoveDrawable(drawable);

            foreach (IEntitySystem e in Systems)
            {
                foreach (Type t in e.GetType().BaseType.GetGenericArguments())
                {
                    if (entity.GetType().IsSubclassOf(t))
                    {
                        e.RemoveEntity(entity);
                        break;
                    }
                }
            }
        }
        public virtual void RegisterSystems() { }

        public virtual void Update(GameTime time) { }

        public virtual void OnActivate() { }

        public virtual void OnDeactivate() { }

        public void Save(BinaryWriter bw)
        {
            bw.Write(GetType());

            bw.Write(Systems.Count);
            foreach (IEntitySystem system in Systems)
                bw.Write(system.GetType());

            IEnumerable<IComponent> igc = new List<IComponent>();
            igc = igc.Concat(Drawables).Concat(Updateables).Distinct();

            int noOfValid = igc.Count(n => n is ISerializable);
            bw.Write(noOfValid);

            Logger.NewText(noOfValid);

            foreach (IComponent component in igc)
            {
                if (component is ISerializable s)
                {
                    bw.Write(s.GetType());

                    s.Save(bw);
                }
            }
        }

        public IComponent Load(BinaryReader br)
        {
            Type type = br.ReadType();
            Scene scene = Activator.CreateInstance(type) as Scene;

            int systemCount = br.ReadInt32();
            for(int i = 0; i < systemCount; i++)
            {
                Type systemType = br.ReadType();
                IEntitySystem ies = Activator.CreateInstance(systemType) as IEntitySystem;

                scene.AddSystem(ies);
            }

            int dCount = br.ReadInt32();
            //Debug.WriteLine(dCount);

            for (int i = 0; i < dCount; i++)
            {
                Type entitytype = br.ReadType();

                IComponent ic = ContentWriter.Load(entitytype, br);

                scene.AddEntity(ic);
            }

            return scene;
        }
    }
}

