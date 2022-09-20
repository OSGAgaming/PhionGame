using QueefCord.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Core.Graphics
{
    internal delegate void LightTargetEvent();
    public class MapHost
    {
        public Map Maps;

        public MapHost(ContentManager Content, Layer Parent)
        {
            Maps = new Map();
            Maps.Parent = Parent;

            foreach (Type t in Utils.GetInheritedClasses(typeof(MapPass)))
            {
                MapPass? state = (MapPass?)Activator.CreateInstance(t);

                if (state != null) Maps?.AddMap(t.Name, state, Parent);
            }

            Maps.Sort();
        }
    }
}