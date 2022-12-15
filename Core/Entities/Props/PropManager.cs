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
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Core.Tiles
{
    public partial class PropManager
    {
        public static PropManager Instance;

        public Dictionary<string, Texture2D> Props { get; set; }
        public Dictionary<string, Color> MiniMapColor { get; set; }

        public int AddPropType(string Prop, Texture2D tex)
        {
            Props.Add(Prop, tex);
            MiniMapColor.Add(Prop, tex.GetDominantColor());

            return Props.Count - 1;
        }

        public PropManager()
        {
            Props = new Dictionary<string, Texture2D>();
            MiniMapColor = new Dictionary<string, Color>();
            LoadProps();
        }

        static PropManager()
        {
            Instance = new PropManager();

            Type[] PropEntityTypes = Utils.GetInheritedClasses(typeof(PropEntity));

            for (int i = 0; i < PropEntityTypes.Length; i++)
            {
                PropEntity PE = (PropEntity)Activator.CreateInstance(PropEntityTypes[i]);
                PropEntity.PropEntities.Add(PE.Prop, PE);
            }
        }
    }
}
