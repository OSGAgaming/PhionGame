
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QueefCord.Core.Input;
using QueefCord;

namespace QueefCord.Core.UI
{
    public class UIElement
    {
        public Rectangle dimensions;

        public bool IsBeingClicked;
        public bool IsBeingHovered;

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void DrawOnScreenDirect(SpriteBatch spriteBatch) { }

        protected virtual void OnUpdate() { }

        protected virtual void OnHover() { }

        protected virtual void NotOnHover() { }

        protected virtual void OnLeftClick() { }

        protected virtual void OnLeftClickAway() { }

        protected virtual void OnRightClick() { }

        public UIScreen? Parent;

        public T GetParent<T>() where T : class => Parent as T;

        public void Update()
        {
            IsBeingHovered = false;
            OnUpdate();
            MouseState state = GameInput.Instance.CurrentMouseState;
            if (dimensions.Contains(state.Position))
            {
                OnHover();
                IsBeingHovered = true;
            }
            if (!dimensions.Contains(state.Position)) NotOnHover();
            if (GameInput.Instance.JustClickingLeft && dimensions.Contains(state.Position)) OnLeftClick();        
            if (GameInput.Instance.JustClickingLeft && !dimensions.Contains(state.Position)) OnLeftClickAway();
            if (GameInput.Instance.JustClickingRight && dimensions.Contains(state.Position)) OnRightClick();

            if (state.LeftButton == ButtonState.Pressed && dimensions.Contains(state.Position)) IsBeingClicked = true;
            else IsBeingClicked = false;
        }

        public void SetDimensions(int x, int y, int width, int height)
        {
            dimensions = new Rectangle(x, y, width, height);
        }

        public void SetDimensionsPercentage(float x, float y, int width, int height)
        {
            Point screenSize = Main.Graphics.GraphicsDevice.Viewport.Bounds.Size;

            dimensions = new Rectangle((int)(x * screenSize.X), (int)(y * screenSize.Y), width, height);
        }
    }
}
