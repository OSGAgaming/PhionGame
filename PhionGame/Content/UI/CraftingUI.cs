

using QueefCord.Content.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.IO;
using QueefCord.Core.Resources;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace QueefCord.Content.UI
{
    internal class CraftingUI : UIScreen
    {
        public CraftingPanel[]? Panels;
        public CraftIngredientPanel[]? IngredientPanels;

        ICraftable[] AllCraftables;
        ICraftable[] Craftables;

        internal override void DrawToScreen()
        {
        }

        protected override void OnUpdate()
        {
        }

        public void ReCalculate()
        {
            if (Player.LocalPlayer == null) return;

            foreach (UIElement e in elements.ToArray())
                if (e is CraftingPanel) elements.Remove(e);

            Craftables = new ICraftable[AllCraftables.Length];

            int index = 0;

            foreach (ICraftable craftable in AllCraftables)
            {
                if (craftable.Recipie == null) continue;

                IEnumerable<string> Items = Player.LocalPlayer.CraftItems.ToList().FindAll(n => n.item != null).Select(n => n.item.Id);

                if (!craftable.Recipie.ToList().FindAll(n => n.item != null).Select(n => n.item.Id).Except(Items).Any())
                {
                    bool can = false;

                    foreach (SlotInfo storeable in craftable.Recipie)
                    {
                        int threshold = storeable.stack;

                        foreach (SlotInfo playerItems in Player.LocalPlayer.CraftItems)
                        {
                            if (playerItems.item != null && storeable.item != null && playerItems.item.Id == storeable.item.Id)
                                threshold -= playerItems.stack;
                        }

                        if (threshold <= 0) can = true;
                        else
                        {
                            can = false;
                            break;
                        }
                    }

                    if (!can) continue;

                    Craftables[index] = craftable;
                    index++;
                }
            }

            Panels = new CraftingPanel[index];

            for (int i = 0; i < index; i++)
            {
                Panels[i] = new CraftingPanel();
                Panels[i].index = i;
                Panels[i].ToBeCrafted = Craftables[i];

                elements.Add(Panels[i]);
            }
        }


        protected override void OnLoad()
        {
            AllCraftables = Utils.GetInheritedClasses<ICraftable>().ToArray();
            Craftables = new ICraftable[AllCraftables.Length];

            IngredientPanels = new CraftIngredientPanel[4];

            IngredientPanels[0] = new CraftIngredientPanel(new Point(100, 150), 0);
            elements.Add(IngredientPanels[0]);

            IngredientPanels[1] = new CraftIngredientPanel(new Point(132, 150), 1);
            elements.Add(IngredientPanels[1]);

            IngredientPanels[2] = new CraftIngredientPanel(new Point(132, 182), 2);
            elements.Add(IngredientPanels[2]);

            IngredientPanels[3] = new CraftIngredientPanel(new Point(100, 182), 3);
            elements.Add(IngredientPanels[3]);

            ReCalculate();
        }

        internal override void OnDrawToScreenDirect() { }
    }

    public class CraftIngredientPanel : UIElement
    {
        private int dims = 32;
        public int index;
        public CraftIngredientPanel(Point p, int index) { dimensions.Location = p; this.index = index; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (UIScreenManager.Instance.GetScreen<Inventory>().Hidden) return;

            Texture2D panel = Assets<Texture2D>.Get("Textures/InventoryPanel").GetValue();
            dimensions.Size = new Point(dims);

            spriteBatch.Draw(panel, dimensions, Color.White);

            if (Player.LocalPlayer.CraftItems[index].item == null) return;

            spriteBatch.Draw(Player.LocalPlayer.CraftItems[index].item.Icon, dimensions.Inf(-2, -2), Color.White);

            Utils.DrawTextToLeft(Player.LocalPlayer.CraftItems[index].stack.ToString(), Color.Black, dimensions.Location.ToVector2(), 1f, 0f);
            Utils.DrawTextToLeft(Player.LocalPlayer.CraftItems[index].stack.ToString(), Color.White, dimensions.Location.ToVector2() + new Vector2(1), 1f, 0f);

            base.Draw(spriteBatch);
        }

        protected override void OnLeftClick()
        {
            SlotInfo pui = Inventory.PickedUpItem;

            Inventory.PickedUpItem = Player.LocalPlayer.CraftItems[index];
            Player.LocalPlayer.CraftItems[index] = pui;

            UIScreenManager.Instance.GetScreen<CraftingUI>().ReCalculate();
        }
    }

    internal class CraftingPanel : UIElement
    {
        public ICraftable ToBeCrafted { get; set; }
        public int index;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (UIScreenManager.Instance.GetScreen<Inventory>().Hidden) return;

            Texture2D panel = Assets<Texture2D>.Get("Textures/InventoryPanel").GetValue();

            dimensions = new Rectangle(116, 100 + index * 32, 32, 32);

            spriteBatch.Draw(panel, dimensions.Inf(-2, -2), Color.White);
            spriteBatch.Draw(ToBeCrafted.Icon, dimensions, Color.White);

            int padding = 5;
            int ingDims = 16;

            int l = ToBeCrafted.Recipie.Length;


            for (int i = 0; i < ToBeCrafted.Recipie.Length; i++)
            {
                Rectangle r = new Rectangle(dimensions.Right + padding + i * (ingDims + padding), dimensions.Bottom - ingDims, ingDims, ingDims);
                //spriteBatch.Draw(ToBeCrafted.Recipie[i].item.Icon, r, Color.White * (1 - (i / (float)l)));
            }
        }
        protected override void OnLeftClick()
        {
            Inventory.AddItem(ToBeCrafted);

            for (int i = 0; i < ToBeCrafted.Recipie.Length; i++)
            {
                int amount = ToBeCrafted.Recipie[i].stack;

                Inventory.RemoveItems(ToBeCrafted.Recipie[i].item, Player.LocalPlayer.CraftItems, amount);
            }
        }
    }

}


