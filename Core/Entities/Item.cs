using System;
using QueefCord.Content.UI;
using Microsoft.Xna.Framework.Graphics;

namespace QueefCord.Core.Interfaces
{

    public class Item : ICraftable
    {
        public virtual SlotInfo[] Recipie { get; set; }
        public virtual Texture2D Icon { get; }
        public virtual int MaxStack { get; }
        public virtual string Id { get; }
        public virtual string Tooltip { get; }

        public Item() { SetDefaults(); }
        
        public virtual void SetDefaults() { }
        public virtual void OnUse() { }
        public virtual void OnActive() { }
        public virtual void OnPassive() { }
        public virtual void OnDraw(SpriteBatch sb) { }

        public void AddRecepie<T>(int amount) where T : IStoreable, new()
        {
            for (int i = 0; i < Recipie.Length; i++)
            {
                if (Recipie[i].item == null)
                {
                    Recipie[i] = new SlotInfo()
                    {
                        item = new T(),
                        stack = amount
                    };
                    break;
                }
            }
        }
    }
}
