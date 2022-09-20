using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace QueefCord.Core.Entities
{
	//for grouping
	public interface IEntitySystem : IUpdate, IDraw
	{
		public void AddEntity(IComponent entity);

		public void RemoveEntity(IComponent entity);

		public void Load();
	}
	public abstract class EntitySystem<T> : IEntitySystem where T : Entity
	{
		[JsonIgnore]
		public List<T> Entities = new List<T>();

        public virtual string Layer => "Default";

        public void AddEntity(IComponent entity)
		{
			if (entity is T e)
			{
				Entities.Add(e);
				EntityWatch(e);
			}
		}

		public void RemoveEntity(IComponent entity) => Entities.Remove(entity as T);

		public abstract void Update(GameTime gameTime);

		public virtual void Draw(SpriteBatch sb) { }

		public virtual void Load() { }

		public virtual void EntityWatch(in T entity) { }
	}
}
