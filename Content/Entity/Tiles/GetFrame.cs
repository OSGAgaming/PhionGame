using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using QueefCord.Core.Scenes;
using QueefCord.Core.Tiles;
using QueefCord.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueefCord.Content.Entities
{
    public class TileBitMask
    {
        public TileBitMask()
        {
            OnLoad();
            //Some Constant bit logic
        }

        protected virtual Dictionary<byte, Rectangle> BitMask { get; set; }

        protected virtual void OnLoad() { }

        public void AddConfig(string config, Point p, Point s) => BitMask.Add(Convert.ToByte(config, 2), new Rectangle(p,s));

        public Rectangle GetConfig(string config) => BitMask[Convert.ToByte(config, 2)];
    }

    public class OutlineBitMask : TileBitMask
    {
        protected override void OnLoad()
        {
            /*
            AddConfig(
                 "0"+
               "0"+"0"
                +"0"
                ,Point.Zero);
            */
        }
    }

    internal static class Framing
    {
        /*
        public static Rectangle OutlineFrame(TileSet manager, int i, int j)
        {
            
        }
        */
        //TODO: abstract
        public static Rectangle TileFrame(TileSet manager, int i, int j)
        {
            Tile?[,] tiles = manager.Tiles;
            int res = TileManager.frameResolution;

            Rectangle r(int a, int b) => new Rectangle(a * res, b * res, res, res);

            return r(0, 5);
        }
    }
}
