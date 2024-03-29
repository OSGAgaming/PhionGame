﻿using QueefCord.Core.Graphics;
using QueefCord.Core.Helpers;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace QueefCord.Content.UI
{
    public static class Logger
    {
        private static readonly int LogCount = 100;
        internal static List<string> Logs = new List<string>();

        public static int TimeWithoutLog = 0;
        public static void NewText(string LogMessage)
        {
            TimeWithoutLog = 0;
            Logs.Insert(0, LogMessage);
            if(Logs.Count > LogCount)
            {
                Logs.RemoveAt(LogCount);
            }
        }

        public static void NewText(object LogMessage)
        {
            string? LM = LogMessage.ToString();
            if (LM != null)
            {
                TimeWithoutLog = 0;
                Logs.Insert(0, LM);
                if (Logs.Count > LogCount)
                {
                    Logs.RemoveAt(LogCount);
                }
            }
        }
    }
    internal class LoggerUI : UIScreen
    {
        public float LogAlpha;
        protected override void OnLoad()
        {

        }
        protected override void OnUpdate()
        {
            Logger.TimeWithoutLog++;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) Logger.TimeWithoutLog = 0;

            if (Logger.TimeWithoutLog > 280) LogAlpha = LogAlpha.ReciprocateTo(0);
            else LogAlpha = LogAlpha.ReciprocateTo(1,3f);
        }
        protected override void PreDraw(SpriteBatch sb)
        {
            int MaxOnscreenLogs = 20;
            var logger = Logger.Logs;

            Vector2 ASS = Renderer.BackBufferSize.ToVector2();
            int Count = MathHelper.Min(logger.Count , MaxOnscreenLogs);

            for (int i = 0; i < Count; i++)
            {
                float alpha = 1 - i / (float)MaxOnscreenLogs;
                Utils.DrawTextToLeft(logger[i], Color.Yellow * alpha * LogAlpha, new Vector2(30, ASS.Y - 30 - 20*i), 1);
            }
        }
    }

}
