using QueefCord.Core.Input;
using Microsoft.Xna.Framework.Input;

namespace QueefCord.Content.Controls
{
#nullable disable
    public class RegisterControls
    {
        public static void Invoke()
        {
            GameInput.Instance.RegisterControl("A", Keys.A, Buttons.LeftThumbstickLeft);
            GameInput.Instance.RegisterControl("D", Keys.D, Buttons.LeftThumbstickRight);
            GameInput.Instance.RegisterControl("W", Keys.W, Buttons.LeftThumbstickUp);
            GameInput.Instance.RegisterControl("S", Keys.S, Buttons.LeftThumbstickDown);
            GameInput.Instance.RegisterControl("Q", Keys.Q, Buttons.LeftThumbstickDown);
            GameInput.Instance.RegisterControl("L", Keys.L, Buttons.LeftThumbstickDown);
            GameInput.Instance.RegisterControl("J", Keys.J, Buttons.LeftThumbstickDown);
            GameInput.Instance.RegisterControl("Space", Keys.Space, Buttons.A);
            GameInput.Instance.RegisterControl("RightC", MouseInput.Right, Buttons.RightShoulder);
            GameInput.Instance.RegisterControl("LeftC", MouseInput.Left, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("Esc", Keys.Escape, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("Delete", Keys.Delete, Buttons.Y);
            GameInput.Instance.RegisterControl("Tab", Keys.Tab, Buttons.Y);
            GameInput.Instance.RegisterControl("Right", Keys.Right, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("Left", Keys.Left, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("Up", Keys.Up, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("Down", Keys.Down, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("Alt", Keys.LeftAlt, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("ScrollU", MouseInput.ScrollUp, Buttons.DPadUp);
            GameInput.Instance.RegisterControl("ScrollD", MouseInput.ScrollDown, Buttons.DPadDown);
        }
    }
}
