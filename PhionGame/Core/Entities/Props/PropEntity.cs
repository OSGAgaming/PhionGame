

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Collections.Generic;
using QueefCord.Core.Entities;
using QueefCord.Core.Interfaces;
using Newtonsoft.Json;
using QueefCord.Content.UI;
using QueefCord.Core.Helpers;
using QueefCord.Core.DataStructures;

namespace QueefCord.Core.Tiles
{
    public abstract class PropEntity : EntityCore
    {

        public static Dictionary<string, PropEntity> PropEntities = new Dictionary<string, PropEntity>();
        [JsonIgnore]
        protected Texture2D Texture => PropManager.Instance.Props[Prop];
        public virtual Color MapColor => Color.White;
        public abstract string Prop { get; }
        public virtual bool Draw(SpriteBatch spriteBatch, Prop prop) { return true; }
        public virtual bool DrawMiniMap(SpriteBatch spriteBatch, Prop prop) { return false; }
        public virtual void Update(Prop prop) { }
        public virtual void OnCollide(Prop prop, Entity2D col) { }
        public virtual void PostLoad(Prop prop) { }
    }
}

