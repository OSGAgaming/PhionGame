using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QueefCord.Core.Scenes
{
	public abstract class SceneTransition : IUpdate
	{
		public int TimeLeft { get; set; }

		/// <summary>
		/// SceneHolder will switch scenes to the target scene when TimeLeft is equal to this
		/// </summary>
		public int TransitionPoint => 0;

		public Scene TargetScene { get; set; }

		public abstract void Draw(SpriteBatch sb);

		public abstract void Update(GameTime gameTime);
	}
}
