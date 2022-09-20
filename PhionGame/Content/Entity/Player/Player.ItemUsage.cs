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

namespace QueefCord.Content.Entities
{
    public class PlayerItemUsage : IEntityModifier
    {
        public PlayerItemUsage()
        {
            //MineHitbox
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

        public void Update(in Entity entity, GameTime gameTime)
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
