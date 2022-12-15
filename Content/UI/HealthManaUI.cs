using QueefCord.Content.Entities;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace QueefCord.Content.UI
{
    internal class HealthManaUI : UIScreen
    {
        internal static Player PlayerFocus { get; set; }

        public HealthBar HealthBar;
        public ManaBar ManaBar;
        public ExperienceBar ExperienceBar;

        protected override void OnLoad()
        {
            HealthBar = new HealthBar();
            ManaBar = new ManaBar();
            ExperienceBar = new ExperienceBar();

            AddElement(HealthBar);
            AddElement(ManaBar);
            AddElement(ExperienceBar);
        }
    }

    public class Bar : UIElement
    {
        protected int BarPadding = 5;

        public Player player => Player.LocalPlayer;
        public Inventory inventory => UIScreenManager.Instance.GetScreen<Inventory>();

        public virtual int Height { get; } = 8;
        public virtual int MaxWidth { get; } = 200;
        public virtual int BorderWidth { get; } = 2;
        public virtual int MaxValue { get; }
        public virtual int Value { get; }
        public virtual int YOffset { get; }
        public virtual Color Color { get; set; }
        public virtual Color BorderColor { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (HealthManaUI.PlayerFocus != null)
            {
                HealthManaUI parent = GetParent<HealthManaUI>();

                Inventory inventory = UIScreenManager.Instance.GetScreen<Inventory>();

                int width = Player.LocalPlayer.HotbarSpace * inventory.PanelDimensions.X;

                int X = (Renderer.BackBufferSize.X - width) / 2;
                int Y = Renderer.BackBufferSize.Y - YOffset;

                Player player = HealthManaUI.PlayerFocus;

                float perc = Value / (float)MaxValue;
                int barWidth = (int)(MaxWidth * perc);

                Utils.DrawBoxFill(new Rectangle(X, Y, MaxWidth, Height).Inf(BorderWidth, BorderWidth), BorderColor);
                Utils.DrawBoxFill(new Rectangle(X, Y, barWidth, Height), Color);
            }
        }
    }

    public class HealthBar : Bar
    {
        public override int MaxValue => Player.LocalPlayer.Get<BaseNPCStats>().MaxHealth;
        public override int Value => Player.LocalPlayer.Get<BaseNPCStats>().Health;
        public override Color Color => Color.Red;
        public override Color BorderColor => Color.IndianRed;
        public override int YOffset => Height + BarPadding + BorderWidth + inventory.PanelDimensions.Y;
    }

    public class ManaBar : Bar
    {
        public override int MaxValue => Player.LocalPlayer.Get<BaseNPCStats>().MaxMana;
        public override int Value => Player.LocalPlayer.Get<BaseNPCStats>().Mana;
        public override Color Color => Color.CadetBlue;
        public override Color BorderColor => Color.DodgerBlue;
        public override int YOffset => (Height + BarPadding + BorderWidth) * 2 + inventory.PanelDimensions.Y;
    }

    public class ExperienceBar : Bar
    {
        public override int MaxValue => Player.LocalPlayer.Get<BaseNPCStats>().MaxExperience;
        public override int Value => Player.LocalPlayer.Get<BaseNPCStats>().Experience;
        public override Color Color => Color.ForestGreen;
        public override Color BorderColor => Color.DarkSeaGreen;
        public override int YOffset => (Height + BarPadding + BorderWidth) * 3 + inventory.PanelDimensions.Y;
    }
}
