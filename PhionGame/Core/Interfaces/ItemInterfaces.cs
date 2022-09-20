using System;
using QueefCord.Content.UI;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace QueefCord.Core.Interfaces
{
    public interface IStoreable
    {
        public Texture2D Icon { get; }
        public int MaxStack { get; }
        public string Id { get; }
        public string Tooltip { get; }
    }

    public interface ICraftable : IStoreable
    {
        public SlotInfo[] Recipie { get; set; }
    }
}
