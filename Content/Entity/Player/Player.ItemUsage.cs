using QueefCord.Content.UI;
using QueefCord.Core.Entities;
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
using System.IO;

namespace QueefCord.Content.Entities
{
    public class PlayerInventory : IEntityModifier, ISerializable
    {
        public SlotInfo[] BagItems;
        public SlotInfo[] HotbarItems;
        public SlotInfo[] CraftItems;

        public int InventorySpace = 25;
        public int HotbarSpace = 5;
        public int CraftSpace = 4;

        public PlayerInventory()
        {
            BagItems = new SlotInfo[InventorySpace];
            HotbarItems = new SlotInfo[HotbarSpace];
            CraftItems = new SlotInfo[CraftSpace];
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch sb)
        {
            IStoreable storeable = UIScreenManager.Instance.GetScreen<Inventory>().ActiveHotbarItem;

            if (storeable is Item i)
            {
                i.OnDraw(sb);
            }
        }

        public IComponent Read(BinaryReader br)
        {
            int bagItems = br.ReadInt32();
            int hotbarItems = br.ReadInt32();

            PlayerInventory inv = new PlayerInventory(); 

            for (int i = 0; i < bagItems; i++)
            {
                if (br.ReadBoolean()) continue;

                inv.BagItems[i].item = Activator.CreateInstance(br.ReadType()) as IStoreable;
                inv.BagItems[i].stack = br.ReadInt32();
            }

            for (int i = 0; i < hotbarItems; i++)
            {
                if (br.ReadBoolean()) continue;

                inv.HotbarItems[i].item = Activator.CreateInstance(br.ReadType()) as IStoreable;
                inv.HotbarItems[i].stack = br.ReadInt32();
            }

            return inv;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(BagItems.Length);
            bw.Write(HotbarItems.Length);

            for (int i = 0; i < BagItems.Length; i++)
            {
                bw.Write(BagItems[i].item == null);

                if (BagItems[i].item == null) continue;

                bw.Write(BagItems[i].item.GetType());
                bw.Write(BagItems[i].stack);
            }

            for (int i = 0; i < HotbarItems.Length; i++)
            {
                bw.Write(HotbarItems[i].item == null);

                if (HotbarItems[i].item == null) continue;

                bw.Write(HotbarItems[i].item.GetType());
                bw.Write(HotbarItems[i].stack);
            }
        }

        public void Update(in EntityCore entity, GameTime gameTime)
        {
            IStoreable storeable = UIScreenManager.Instance.GetScreen<Inventory>().ActiveHotbarItem;

            if (storeable is Item i)
            {
                if (GameInput.Instance["LeftC"].IsJustPressed())
                {
                    i.OnUse();
                }

                i.OnActive();
            }

            foreach(SlotInfo slot in Inventory.Items)
            {
                if (slot.item == null)
                    continue;

                if (slot.item.Equals(storeable))
                    continue;

                if (slot.item is Item item)
                    item.OnPassive();
            }
        }
    }
}
