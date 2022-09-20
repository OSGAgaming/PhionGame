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
    public class Orange : IStoreable
    {
        public Texture2D Icon => Assets<Texture2D>.Get("Textures/Orange");
        public int MaxStack => 7;
        public string Id => "Orange";
        public string Tooltip => "Reasonably annoying";
    }
}