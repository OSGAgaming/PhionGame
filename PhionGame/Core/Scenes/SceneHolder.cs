using QueefCord.Core.Entities;
using QueefCord.Core.Graphics;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QueefCord.Core.Scenes
{
    public static class SceneHolder
    {
        public static Scene CurrentScene { get; set; }

        private static SceneTransition sceneTransition;

        public static void Update(GameTime time)
        { 
			CurrentScene.UpdateTickables(time);
			CurrentScene.Update(time);
            CurrentScene.UpdateSystems(time);

            CurrentScene.TimeAlive++;

			if (sceneTransition != null)
				UpdateTransition(time);
    }

    public static void StartScene(Scene scene)
    {
        CurrentScene?.OnDeactivate();
        CurrentScene = scene;
        CurrentScene.Activate();
        CurrentScene.TimeAlive = 0;
    }

    public static void StartTransition(Scene targetScene, SceneTransition transition)
    {
        sceneTransition = transition;
        sceneTransition.TargetScene = targetScene;
    }

    public static void UpdateTransition(GameTime time)
    {
        sceneTransition.Update(time);

        if (--sceneTransition.TimeLeft == sceneTransition.TransitionPoint)
            StartScene(sceneTransition.TargetScene);

        if (sceneTransition.TimeLeft == 0)
            sceneTransition = null;
    }

    public static void DrawTransition(SpriteBatch sb) => sceneTransition?.Draw(sb);
}
}
