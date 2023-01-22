using QueefCord.Content.Entities;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QueefCord.Content.UI
{
    public struct SlotInfo
    {
        public IStoreable item;
        public int stack;
    }

    public class Inventory : UIScreen
    {
        public static InventoryPanel[] ItemPanels;
        public static HotbarPanel[] HotbarPanels;

        public static SlotInfo[] Items => Player.LocalPlayer.Get<PlayerInventory>().BagItems.Concat(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems).ToArray();

        public static SlotInfo PickedUpItem;
        public static SlotInfo BinnedItem;

        public int InventoryWidth;
        public int InventoryHeight;
        public Point PanelDimensions;
        public int PanelPadding = 2;

        public int ActiveHotbarIndex;
        public IStoreable ActiveHotbarItem => HotbarPanels[ActiveHotbarIndex].Item;

        public int TextOffsetX => 1;
        public int TextOffsetY => 1;

        public Vector2 InventoryScreenSpace;

        public bool Hidden;

        public static bool AddItemToArray(SlotInfo[] items, IStoreable item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item == null)
                    continue;

                if (items[i].item.Id == item.Id && items[i].stack < item.MaxStack)
                {
                    items[i].stack++;

                    return true;
                }
            }

            //Nulls later
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item == null)
                {
                    items[i].item = item;
                    items[i].stack++;

                    return true;
                }
            }

            return false;
        }

        public static bool RemoveItemFromArray(SlotInfo[] items, IStoreable item, int amount)
        {
            for (int i = 0; i < items.Length; i++)
            {
                SlotInfo info = items[i];

                if (info.item == null) continue;

                if (info.item.Id == item.Id)
                {
                    if (info.stack >= amount)
                    {
                        items[i].stack -= amount;

                        if(items[i].stack == 0) items[i].item = null;

                        return false;
                    }
                    else
                    {
                        amount -= info.stack;
                        items[i].stack = 0;
                        items[i].item = null;
                    }
                }
            }

            return true;
        }

        public static bool AddItem(IStoreable item)
        {
            bool hotbarCheck = AddItemToArray(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems, item);
            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();

            if (hotbarCheck)
                return true;

            return AddItemToArray(Player.LocalPlayer.Get<PlayerInventory>().BagItems, item);
        }

        public static bool RemoveItems(IStoreable item, int amount)
        {
            bool hotbarCheck = RemoveItemFromArray(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems, item, amount);
            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();

            if (!hotbarCheck)
                return true;

            return RemoveItemFromArray(Player.LocalPlayer.Get<PlayerInventory>().BagItems, item, amount);
        }

        public static bool RemoveItems(IStoreable item, SlotInfo[] slots, int amount)
        {
            bool hotbarCheck = RemoveItemFromArray(slots, item, amount);
            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();

            if (!hotbarCheck)
                return true;

            return RemoveItemFromArray(slots, item, amount);
        }

        public static bool RemoveItems<T>(int amount) where T : IStoreable, new()
        {
            IStoreable item = new T();

            bool hotbarCheck = RemoveItemFromArray(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems, item, amount);
            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();

            if (!hotbarCheck)
                return true;

            return RemoveItemFromArray(Player.LocalPlayer.Get<PlayerInventory>().BagItems, item, amount);
        }

        protected override void OnLoad()
        {
            ItemPanels = new InventoryPanel[Player.LocalPlayer.Get<PlayerInventory>().InventorySpace];
            HotbarPanels = new HotbarPanel[Player.LocalPlayer.Get<PlayerInventory>().HotbarSpace];

            PanelDimensions = new Point(35, 35);

            InventoryWidth = 300;
            InventoryScreenSpace = Renderer.BackBufferSize.ToVector2() / 2 - new Vector2(InventoryWidth, InventoryHeight) / 2;

            for (int i = 0; i < Player.LocalPlayer.Get<PlayerInventory>().InventorySpace; i++)
            {
                ItemPanels[i] = new InventoryPanel(i);
                AddElement(ItemPanels[i]);
            }

            for (int i = 0; i < Player.LocalPlayer.Get<PlayerInventory>().HotbarSpace; i++)
            {
                HotbarPanels[i] = new HotbarPanel(i);
                AddElement(HotbarPanels[i]);
            }

            AddElement(new BinPanel());
        }
        protected override void OnUpdate()
        {
            if (GameInput.Instance["Space"].IsJustPressed())
            {
                Hidden = !Hidden;
            }

            if (GameInput.Instance["Right"].IsJustPressed())
                ActiveHotbarIndex++;
            if (GameInput.Instance["Left"].IsJustPressed())
                ActiveHotbarIndex--;

            ActiveHotbarIndex = Math.Clamp(ActiveHotbarIndex, 0, Player.LocalPlayer.Get<PlayerInventory>().HotbarSpace - 1);
        }
        protected override void PostDraw(SpriteBatch sb)
        {
            int panelColums = InventoryWidth / PanelDimensions.X;
            int panelRows = InventoryHeight / PanelDimensions.Y;

            PanelDimensions = new Point(40, 40);

            InventoryWidth = 300;
            InventoryHeight = (Player.LocalPlayer.Get<PlayerInventory>().InventorySpace / panelColums + 1) * PanelDimensions.Y;
            InventoryScreenSpace = Renderer.BackBufferSize.ToVector2() / 2 - new Vector2(InventoryWidth, InventoryHeight - 100) / 2;

            int ActualSizeX = InventoryWidth + PanelPadding * (panelColums + 2);
            int ActualSizeY = InventoryHeight + PanelPadding * (panelRows + 2);

            Texture2D panel = Assets<Texture2D>.Get("Textures/InventoryPanel").GetValue();

            if (PickedUpItem.item != null)
            {
                sb.Draw(PickedUpItem.item.Icon, new Rectangle(Mouse.GetState().Position, PanelDimensions), Color.White);

                int X = Mouse.GetState().Position.X;
                int Y = Mouse.GetState().Position.Y;

                Utils.DrawTextToLeft(PickedUpItem.stack.ToString(), Color.Black, new Vector2(X + TextOffsetX, Y + TextOffsetY), 1f, 0f);
                Utils.DrawTextToLeft(PickedUpItem.stack.ToString(), Color.White, new Vector2(X + TextOffsetX - 1, Y + TextOffsetY - 1), 1f, 0f);
            }
            //if (!Hidden)
            //   sb.Draw(panel, new Rectangle(InventoryScreenSpace.ToPoint(), new Point(ActualSizeX, ActualSizeY)), Color.White);
        }
    }

    public class InventoryPanel : UIElement
    {
        public int index { get; set; }
        public IStoreable Item => Inventory.Items[index].item;

        private int outerPadding = 2;

        public InventoryPanel(int index)
        {
            this.index = index;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D panel = Assets<Texture2D>.Get("Textures/InventoryPanel").GetValue();
            Point dims = panel.Bounds.Size;

            Inventory parent = GetParent<Inventory>();

            if (!parent.Hidden)
            {
                int panelColums = parent.InventoryWidth / parent.PanelDimensions.X;
                int panelRows = parent.InventoryHeight / parent.PanelDimensions.Y;

                int iX = index % panelColums;
                int iY = index / panelColums;

                int X = iX * (parent.PanelDimensions.X + parent.PanelPadding) + parent.PanelPadding + (int)parent.InventoryScreenSpace.X;
                int Y = iY * (parent.PanelDimensions.Y + parent.PanelPadding) + parent.PanelPadding + (int)parent.InventoryScreenSpace.Y;

                spriteBatch.Draw(panel, new Rectangle(new Point(X, Y), parent.PanelDimensions), panel.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                dimensions = new Rectangle(new Point(X + outerPadding, Y + outerPadding), parent.PanelDimensions.Sub(new Point(outerPadding * 2)));

                if (Inventory.PickedUpItem.item == null && Inventory.Items[index].item != null && IsBeingHovered)
                {
                    string tooltip = Inventory.Items[index].item.Tooltip;
                    SpriteFont font = Assets<SpriteFont>.Get("Fonts/Arial");

                    Vector2 textureDims = Mouse.GetState().Position.ToVector2() + new Vector2(20, 10);
                    Vector2 ToolTipDims = font.MeasureString(tooltip);
                    Utils.DrawBoxFill(new RectangleF(textureDims, ToolTipDims), Color.Gray, 0.01f);

                    Utils.DrawTextToLeft(tooltip, Color.Black, textureDims, 1f, 0f);
                    Utils.DrawTextToLeft(tooltip, Color.White, textureDims + new Vector2(-1), 1f, 0f);
                }

                if (Item != null)
                {
                    spriteBatch.Draw(Item.Icon, dimensions, Item.Icon.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.02f);

                    Utils.DrawTextToLeft(Inventory.Items[index].stack.ToString(), Color.Black, new Vector2(X + parent.TextOffsetX, Y + parent.TextOffsetY), 1f, 0.02f);
                    Utils.DrawTextToLeft(Inventory.Items[index].stack.ToString(), Color.White, new Vector2(X + parent.TextOffsetX - 1, Y + parent.TextOffsetY - 1), 1f, 0.02f);
                }
            }
        }

        protected override void OnLeftClick()
        {
            if (GetParent<Inventory>().Hidden) return;

            SlotInfo pui = Inventory.PickedUpItem;

            Inventory.PickedUpItem = Player.LocalPlayer.Get<PlayerInventory>().BagItems[index];
            Player.LocalPlayer.Get<PlayerInventory>().BagItems[index] = pui;

            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();
        }
    }

    public class HotbarPanel : UIElement
    {
        public int index { get; set; }
        public IStoreable Item => Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index].item;

        private int outerPadding = 2;

        public HotbarPanel(int index)
        {
            this.index = index;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D panel = Assets<Texture2D>.Get("Textures/InventoryPanel").GetValue();
            Point dims = panel.Bounds.Size;

            Inventory parent = GetParent<Inventory>();

            int panelColums = parent.InventoryWidth / parent.PanelDimensions.X;
            int panelRows = parent.InventoryHeight / parent.PanelDimensions.Y;

            int iX = index;

            int width = Player.LocalPlayer.Get<PlayerInventory>().HotbarSpace * (parent.PanelDimensions.X + parent.PanelPadding) + parent.PanelPadding;

            int X = iX * (parent.PanelDimensions.X + parent.PanelPadding) + parent.PanelPadding + (Renderer.BackBufferSize.X - width) / 2;
            int Y = Renderer.BackBufferSize.Y - parent.PanelDimensions.Y - parent.PanelPadding;

            int X2 = iX * (parent.PanelDimensions.X + parent.PanelPadding) + parent.PanelPadding + (int)parent.InventoryScreenSpace.X;
            int Y2 = -1 * (parent.PanelDimensions.Y + parent.PanelPadding) + parent.PanelPadding + (int)parent.InventoryScreenSpace.Y;

            if (parent.Hidden)
            {
                spriteBatch.Draw(panel, new Rectangle(new Point(X, Y), parent.PanelDimensions), panel.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                dimensions = new Rectangle(new Point(X + outerPadding, Y + outerPadding), parent.PanelDimensions.Sub(new Point(outerPadding * 2)));

                if (index == parent.ActiveHotbarIndex)
                    Utils.DrawRectangle(new Rectangle(new Point(X, Y), parent.PanelDimensions));

                if (Item != null)
                {
                    spriteBatch.Draw(Item.Icon, dimensions, Item.Icon.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

                    Utils.DrawTextToLeft(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index].stack.ToString(), Color.Black, new Vector2(X + parent.TextOffsetX, Y + parent.TextOffsetY), 1f, 0f);
                    Utils.DrawTextToLeft(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index].stack.ToString(), Color.White, new Vector2(X + parent.TextOffsetX - 1, Y + parent.TextOffsetY - 1), 1f, 0f);
                }
            }
            else
            {
                spriteBatch.Draw(panel, new Rectangle(new Point(X2, Y2), parent.PanelDimensions), panel.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                dimensions = new Rectangle(new Point(X2 + outerPadding, Y2 + outerPadding), parent.PanelDimensions.Sub(new Point(outerPadding * 2)));

                if (index == parent.ActiveHotbarIndex)
                    Utils.DrawRectangle(new Rectangle(new Point(X2, Y2), parent.PanelDimensions));

                if (Inventory.PickedUpItem.item == null && Inventory.HotbarPanels[index].Item != null && IsBeingHovered)
                {
                    string tooltip = Inventory.HotbarPanels[index].Item.Tooltip;
                    SpriteFont font = Assets<SpriteFont>.Get("Fonts/Arial");

                    Vector2 textureDims = Mouse.GetState().Position.ToVector2() + new Vector2(20, 10);
                    Vector2 ToolTipDims = font.MeasureString(tooltip);
                    Utils.DrawBoxFill(new RectangleF(textureDims, ToolTipDims), Color.Gray, 0.01f);

                    Utils.DrawTextToLeft(tooltip, Color.Black, textureDims, 1f, 0f);
                    Utils.DrawTextToLeft(tooltip, Color.White, textureDims + new Vector2(-1), 1f, 0f);
                }

                if (Item != null)
                {
                    spriteBatch.Draw(Item.Icon, dimensions, Item.Icon.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

                    Utils.DrawTextToLeft(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index].stack.ToString(), Color.Black, new Vector2(X2 + parent.TextOffsetX, Y2 + parent.TextOffsetY), 1f, 0f);
                    Utils.DrawTextToLeft(Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index].stack.ToString(), Color.White, new Vector2(X2 + parent.TextOffsetX - 1, Y2 + parent.TextOffsetY - 1), 1f, 0f);
                }
            }
        }

        protected override void OnLeftClick()
        {
            SlotInfo pui = Inventory.PickedUpItem;

            Inventory.PickedUpItem = Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index];
            Player.LocalPlayer.Get<PlayerInventory>().HotbarItems[index] = pui;

            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();
        }
    }

    public class BinPanel : UIElement
    {
        public IStoreable Item => Inventory.BinnedItem.item;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D panel = Assets<Texture2D>.Get("Textures/InventoryPanel").GetValue();

            Inventory parent = GetParent<Inventory>();

            if (!parent.Hidden)
            {
                int panelColums = parent.InventoryWidth / parent.PanelDimensions.X;
                int panelRows = parent.InventoryHeight / parent.PanelDimensions.Y;

                parent.InventoryHeight = (Player.LocalPlayer.Get<PlayerInventory>().InventorySpace / panelColums + 1) * parent.PanelDimensions.Y;

                int ActualSizeX = parent.InventoryWidth;

                int X = ActualSizeX + (int)parent.InventoryScreenSpace.X;
                int Y = (int)parent.InventoryScreenSpace.Y + parent.PanelPadding;

                spriteBatch.Draw(panel, new Rectangle(new Point(X, Y), parent.PanelDimensions), panel.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                dimensions = new Rectangle(new Point(X, Y), parent.PanelDimensions);

                if (Inventory.PickedUpItem.item == null && Inventory.BinnedItem.item != null && IsBeingHovered)
                {
                    string tooltip = Inventory.BinnedItem.item.Tooltip;
                    SpriteFont font = Assets<SpriteFont>.Get("Fonts/Arial");

                    Vector2 textureDims = Mouse.GetState().Position.ToVector2() + new Vector2(20, 10);
                    Vector2 ToolTipDims = font.MeasureString(tooltip);
                    Utils.DrawBoxFill(new RectangleF(textureDims, ToolTipDims), Color.Gray, 0.01f);

                    Utils.DrawTextToLeft(tooltip, Color.Black, textureDims, 1f, 0f);
                    Utils.DrawTextToLeft(tooltip, Color.White, textureDims + new Vector2(-1), 1f, 0f);
                }
                if (Item != null)
                {
                    spriteBatch.Draw(Item.Icon, dimensions, Item.Icon.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

                    Utils.DrawTextToLeft(Inventory.BinnedItem.stack.ToString(), Color.Black, new Vector2(X + parent.TextOffsetX, Y + parent.TextOffsetY), 1f, 0f);
                    Utils.DrawTextToLeft(Inventory.BinnedItem.stack.ToString(), Color.White, new Vector2(X + parent.TextOffsetX - 1, Y + parent.TextOffsetY - 1), 1f, 0f);
                }
                Rectangle binDims = new Rectangle(new Point(X + parent.PanelPadding + parent.PanelDimensions.X, Y), parent.PanelDimensions);

                Utils.DrawRectangle(binDims, Color.Red);

                if (binDims.Contains(Mouse.GetState().Position) && GameInput.Instance.JustClickingLeft)
                {
                    Inventory.BinnedItem.item = null;
                    Inventory.BinnedItem.stack = 0;
                }
            }
        }

        protected override void OnLeftClick()
        {
            SlotInfo bi = Inventory.BinnedItem;
            Inventory.BinnedItem = Inventory.PickedUpItem;

            if (Inventory.BinnedItem.item != null)
            {
                Inventory.PickedUpItem.item = null;
                Inventory.PickedUpItem.stack = 0;
            }
            else
            {
                Inventory.PickedUpItem = bi;
            }

            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();
        }
    }
}
