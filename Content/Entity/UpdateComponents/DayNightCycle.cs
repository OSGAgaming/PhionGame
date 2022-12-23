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
    public class DayNightCycle : IUpdate
    {
        public float Time;

        private readonly int MaxTimeInDay = 86400;
        private float SpeedOfDay => 00;

        public void Update(GameTime gameTime)
        {
            Time += SpeedOfDay;

            Time %= MaxTimeInDay;
            Time = 10000;

            Assets<Effect>.Get("Effects/DayNightCycle").GetValue().Parameters["colorMult"].SetValue(GetColorMod(Time).ToVector4());
        }

        public Color GetColorMod(float Time)
        {
            float Morning = 19800;
            float Evening = 51200;
            float Night = 80200;

            Color mC = Color.White;
            Color eC = Color.Orange;
            Color nC = Color.MidnightBlue;

            float perc = (MaxTimeInDay - Night) / (MaxTimeInDay - Night + Morning);

            if (Time > Morning && Time <= Evening)
                return Color.Lerp(mC, eC, (Time - Morning) / (Evening - Morning));
            else if (Time > Evening && Time <= Night)
                return Color.Lerp(eC, nC, (Time - Evening) / (Night - Evening));
            else if (Time > Night)           
                return Color.Lerp(nC, mC, perc * ((Time - Night) / (MaxTimeInDay - Night)));
            else
                return Color.Lerp(nC, mC, Time / Morning + perc);
        }
    }
}
