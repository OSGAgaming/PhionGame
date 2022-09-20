using QueefCord.Content.UI;
using QueefCord.Core.Entities;
using QueefCord.Core.Entities.EntitySystems;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Content.Entities
{
    internal class MilkItem : IStoreable
    {
        public Texture2D Icon => Assets<Texture2D>.Get("Textures/Items/Milk");
        public int MaxStack => 64;
        public string Id => "Milk";
        public string Tooltip => "For fostering bones stronger than any great force could imagine to break";
    }
}
