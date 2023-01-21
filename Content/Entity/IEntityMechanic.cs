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
    public interface IEntityModifier : IDisposable
    {
        public void Update(in EntityCore entity, GameTime gameTime);
    }

    public interface IRendereableEntityModifier : IEntityModifier
    {
        public void Draw(in EntityCore entity, GameTime gameTime, SpriteBatch sb);
    }
}
