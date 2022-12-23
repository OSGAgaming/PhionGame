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
        public PostProcessingHost Maps;

        public MapHost(ContentManager Content, Layer Parent)
        {
            Maps = new PostProcessingHost();
            Maps.Parent = Parent;

            foreach (Type t in Utils.GetInheritedClasses(typeof(PostProcessingPass)))
            {
                PostProcessingPass? state = (PostProcessingPass?)Activator.CreateInstance(t);

                if (state != null) Maps?.AddMap(t.Name, state, Parent);
            }

            Maps.Sort();
        }
    }
}