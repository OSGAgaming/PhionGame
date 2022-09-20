using QueefCord.Content.Graphics;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace QueefCord.Core.Tiles
{
    public class Prop : Entity2D, IDraw, IUpdate, IMappable
    {
        public virtual string Layer { get; }
        public PropEntity PropEntity;

        [JsonIgnore]
        public Texture2D Texture => PropManager.Instance.Props[id];
        public void DrawToMiniMap(SpriteBatch sb)
        {
            if (PropEntity != null)
                if (PropEntity.DrawMiniMap(sb, this))
                    return;

            if (Texture != null)
                Utils.DrawBoxFill(new RectangleF(Transform.Position / MiniMap.Resolution, Size / MiniMap.Resolution), PropManager.Instance.MiniMapColor[id], 1 - Center.Y / 100000f);
        }

        public void Draw(SpriteBatch sb)
        {
            if (PropEntity != null)
                if (PropEntity.Draw(sb, this))
                    return;

            if (Texture != null)
                sb.Draw(Texture, Transform.Position, 1 - Center.Y / 100000f);
        }

        public override void OnCollide(Entity2D entity)
        {
            PropEntity?.OnCollide(this, entity);
        }

        public override void PostLoad(BinaryReader br, Entity2D e)
        {
            Debug.WriteLine("ID: " + e.id);

            if (PropEntity.PropEntities.ContainsKey(e.id))
                (e as Prop).PropEntity = Activator.CreateInstance(PropEntity.PropEntities[e.id].GetType()) as PropEntity;
        }


        public void Update(GameTime gameTime) => PropEntity?.Update(this);

        public Prop(string id, string Layer)
        {
            this.id = id;
            this.Layer = Layer;
            this.PropEntity = Activator.CreateInstance(PropEntity.PropEntities[id].GetType()) as PropEntity;

            Size = Texture.Bounds.Size.ToVector2();
        }

        public Prop() { }
    }
}
