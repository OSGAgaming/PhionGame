using QueefCord.Core.Entities;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Numerics;

namespace QueefCord.Core.Graphics
{
    public static class Renderer
    {
        public static Point MaxResolution => new Point(2560, 1440);

        public static Rectangle MaxResolutionBounds => new Rectangle(0, 0, MaxResolution.X, MaxResolution.Y);

        public static Rectangle BackBufferBounds => new Rectangle(Point.Zero, BackBufferSize);

        public static GraphicsDevice Device => GraphicsDeviceManager.GraphicsDevice;

        public static Viewport Viewport => Device.Viewport;

        public static Point ViewportSize => new Point(Viewport.Width, Viewport.Height);

        public static PresentationParameters PresentationParameters => Device.PresentationParameters;

        public static Point BackBufferSize => new Point(PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight);



        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public static RenderTarget2D RenderTarget { get; private set; }

        public static Game Instance { get; private set; }

        public static SpriteBatch Spritebatch { get; private set; }

        public static Rectangle Destination { get; set; }

        public static void InitializeGraphics(Game game)
        {
            Instance = game;

            GraphicsDeviceManager = new GraphicsDeviceManager(Instance)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };

            GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
            GraphicsDeviceManager.ApplyChanges();

            Destination = PresentationParameters.Bounds;
        }

        public static void PrepareRenderer()
        {
            RenderTarget = new RenderTarget2D(Device, MaxResolution.X, MaxResolution.Y, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            Spritebatch = new SpriteBatch(Device);

            RegisterLayers();
        }

        public static void RegisterLayers()
        {
            LayerHost.RegisterLayer(new Layer(0, new EntityFocalCamera(), "Effects/DayNightCycle", default, default, SpriteSortMode.BackToFront, false), "Background");
            LayerHost.RegisterLayer(new Layer(1, new EntityFocalCamera(), "Effects/DayNightCycle", default, default, SpriteSortMode.BackToFront, true), "Default");
            LayerHost.RegisterLayer(new Layer(2, new CameraTransform()), "UI");
        }

        public static void DrawScene(Scene scene)
        {
            DrawSceneToTarget(scene);

            Device.SetRenderTarget(null);

            Spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Spritebatch.Draw(RenderTarget, Destination, PresentationParameters.Bounds, Color.White);
       
            Destination = PresentationParameters.Bounds;

            Spritebatch.End();
        }

        private static void DrawSceneToTarget(Scene scene)
        {
            LayerHost.DrawLayersToTarget(scene, Spritebatch);

            Device.SetRenderTarget(RenderTarget);
            Device.Clear(Color.Transparent);

            LayerHost.DrawLayers(Spritebatch);

            Device.SetRenderTarget(null);
        }
    }

}
