using QueefCord.Content.Entities;
using QueefCord.Content.Maths;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Entities.EntitySystems;
using QueefCord.Core.Graphics;
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
using System.Linq;
using System.Runtime.Serialization;
using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;

namespace QueefCord.Core.Tiles
{

    public partial class TileSet : IDraw, IMappable
    {
        private byte[,] Outline;
        private byte[,] Top;

        public Rectangle ByteToFrame(byte n)
        {
            Rectangle ManhattanToRect(int x, int y) => new Rectangle(x, y, 1, 1).Multiply(TileManager.frameResolution);
            //UpEdge
            switch (n)
            {
                case 1:
                    return ManhattanToRect(1, 0);
                case 21:
                    return ManhattanToRect(2, 0);
                case 22:
                    return ManhattanToRect(3, 0);
                case 31:
                    return ManhattanToRect(4, 0);
                case 32:
                    return ManhattanToRect(5, 0);
                case 33:
                    return ManhattanToRect(6, 0);
            }

            //DownEdge
            switch (n)
            {
                case 4:
                    return ManhattanToRect(1, 1);
                case 51:
                    return ManhattanToRect(2, 1);
                case 52:
                    return ManhattanToRect(3, 1);
                case 61:
                    return ManhattanToRect(4, 1);
                case 62:
                    return ManhattanToRect(5, 1);
                case 63:
                    return ManhattanToRect(6, 1);
            }

            //LeftEdge
            switch (n)
            {
                case 7:
                    return ManhattanToRect(6, 3);
                case 81:
                    return ManhattanToRect(6, 4);
                case 82:
                    return ManhattanToRect(6, 5);
                case 91:
                    return ManhattanToRect(6, 6);
                case 92:
                    return ManhattanToRect(6, 7);
                case 93:
                    return ManhattanToRect(6, 8);
            }


            //RightEdge
            switch (n)
            {
                case 10:
                    return ManhattanToRect(7, 3);
                case 111:
                    return ManhattanToRect(7, 4);
                case 112:
                    return ManhattanToRect(7, 5);
                case 121:
                    return ManhattanToRect(7, 6);
                case 122:
                    return ManhattanToRect(7, 7);
                case 123:
                    return ManhattanToRect(7, 8);
            }

            //Hori
            switch (n)
            {
                case 200:
                    return ManhattanToRect(1, 2);
                case 201:
                    return ManhattanToRect(2, 2);
                case 202:
                    return ManhattanToRect(3, 2);
                case 203:
                    return ManhattanToRect(4, 2);
                case 204:
                    return ManhattanToRect(5, 2);
                case 205:
                    return ManhattanToRect(6, 2);
            }

            //Vert
            switch (n)
            {
                case 206:
                    return ManhattanToRect(4, 7);
                case 207:
                    return ManhattanToRect(4, 8);
                case 208:
                    return ManhattanToRect(4, 9);
                case 209:
                    return ManhattanToRect(5, 7);
                case 210:
                    return ManhattanToRect(5, 8);
                case 211:
                    return ManhattanToRect(5, 9);
            }

            //Corner
            switch (n)
            {
                case 11:
                    return ManhattanToRect(0, 0);
                case 12:
                    return ManhattanToRect(7, 0);
                case 13:
                    return ManhattanToRect(7, 1);
                case 14:
                    return ManhattanToRect(0, 1);
            }

            //CornerEdges
            switch (n)
            {
                case 15:
                    return ManhattanToRect(6, 9);
                case 16:
                    return ManhattanToRect(7, 9);
                case 17:
                    return ManhattanToRect(0, 2);
                case 18:
                    return ManhattanToRect(7, 2);
                //Orphan
                case 19:
                    return ManhattanToRect(4, 10);
                //Orphan2
                case 20:
                    return ManhattanToRect(5, 10);
            }

            //CornerHollow
            switch (n)
            {
                case 40:
                    return ManhattanToRect(0, 11);
                case 41:
                    return ManhattanToRect(1, 11);
                case 42:
                    return ManhattanToRect(1, 12);
                case 43:
                    return ManhattanToRect(0, 12);
            }

            //MiscConfig1
            switch (n)
            {
                case 44:
                    return ManhattanToRect(2, 11);
                case 45:
                    return ManhattanToRect(3, 11);
                case 46:
                    return ManhattanToRect(2, 12);
                case 47:
                    return ManhattanToRect(3, 12);
            }

            //MiscConfig2
            switch (n)
            {
                case 70:
                    return ManhattanToRect(4, 11);
                case 71:
                    return ManhattanToRect(5, 11);
                case 72:
                    return ManhattanToRect(4, 12);
                case 73:
                    return ManhattanToRect(5, 12);
            }

            //MiscConfig3
            switch (n)
            {
                case 74:
                    return ManhattanToRect(6, 11);

                case 75:
                    return ManhattanToRect(7, 11);
                case 76:
                    return ManhattanToRect(6, 12);
                case 77:
                    return ManhattanToRect(7, 12);
            }

            //MiscConfig4
            switch (n)
            {
                case 78:
                    return ManhattanToRect(0, 13);
                case 79:
                    return ManhattanToRect(1, 13);
                case 80:
                    return ManhattanToRect(0, 14);
                case 101:
                    return ManhattanToRect(1, 14);
            }

            //MiscConfig5
            switch (n)
            {
                case 102:
                    return ManhattanToRect(2, 14);
                case 83:
                    return ManhattanToRect(3, 14);
                case 84:
                    return ManhattanToRect(5, 13);
                case 85:
                    return ManhattanToRect(5, 14);
            }

            //MiscConfig6
            switch (n)
            {
                case 86:
                    return ManhattanToRect(2, 13);
                case 87:
                    return ManhattanToRect(3, 13);
                case 88:
                    return ManhattanToRect(4, 13);
                case 89:
                    return ManhattanToRect(4, 14);
                case 90:
                    return ManhattanToRect(2, 15);
            }

            //OpCorners
            switch (n)
            {
                case 103:
                    return ManhattanToRect(3, 15);
                case 104:
                    return ManhattanToRect(4, 15);
            }
            return Rectangle.Empty;
        }

        public Rectangle ByteToFrameTop(byte n)
        {
            Rectangle ManhattanToRect(int x, int y) => new Rectangle(x, y, 1, 1).Multiply(TileManager.frameResolution);
            //Top
            switch (n)
            {
                case 0:
                    return ManhattanToRect(0, 0);
                case 1:
                    return ManhattanToRect(1, 0);
                case 2:
                    return ManhattanToRect(2, 0);
                case 3:
                    return ManhattanToRect(3, 0);
                case 4:
                    return ManhattanToRect(4, 0);
                case 5:
                    return ManhattanToRect(5, 0);
            }
            //Bottom
            switch (n)
            {
                case 10:
                    return ManhattanToRect(0, 1);
                case 11:
                    return ManhattanToRect(1, 1);
                case 12:
                    return ManhattanToRect(2, 1);
                case 13:
                    return ManhattanToRect(3, 1);
                case 14:
                    return ManhattanToRect(4, 1);
                case 15:
                    return ManhattanToRect(5, 1);
            }

            //Right
            switch (n)
            {
                case 20:
                    return ManhattanToRect(0, 2);
                case 21:
                    return ManhattanToRect(0, 3);
                case 22:
                    return ManhattanToRect(1, 2);
                case 23:
                    return ManhattanToRect(1, 3);
                case 24:
                    return ManhattanToRect(2, 2);
                case 25:
                    return ManhattanToRect(2, 3);
            }

            //Left
            switch (n)
            {
                case 30:
                    return ManhattanToRect(3, 2);
                case 31:
                    return ManhattanToRect(3, 3);
                case 32:
                    return ManhattanToRect(4, 2);
                case 33:
                    return ManhattanToRect(4, 3);
                case 34:
                    return ManhattanToRect(5, 2);
                case 35:
                    return ManhattanToRect(5, 3);
            }
            return Rectangle.Empty;
        }

        public void RenderOutline(SpriteBatch sb)
        {
            int w = TileManager.width;
            int h = TileManager.height;
            int res = TileManager.drawResolution;

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            LayerHost.GetLayer(Layer).MapHost.Maps.DrawToBatchedMap("TileTextureMap",
             (SpriteBatch sb) =>
             {
                 for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, w); i++)
                     for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 1, h); j++)
                     {
                         bool IsActive(int a, int b)
                         {
                             if (i + a < 0 || j + b < 0) return false;

                             return Tiles[i + a, j + b] != null;
                         }

                         Tile? tile = Tiles[i, j];
                         if ((IsActive(1, 0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1)) && tile == null && Top[i, j] != 255
                         && i == Math.Clamp(i, 1, w) && j == Math.Clamp(j, 1, h))
                         {
                             Tile? tileR = Tiles[i + 1, j];
                             Tile? tileL = Tiles[i - 1, j];
                             Tile? tileU = Tiles[i, j - 1];
                             Tile? tileD = Tiles[i, j + 1];

                             Texture2D texTop = null;

                             if (tileR != null && TileManager.TileOutlines.ContainsKey(tileR.Value.id))
                                 texTop = TileManager.TileTop[tileR.Value.id];
                             if (tileL != null && TileManager.TileOutlines.ContainsKey(tileL.Value.id))
                                 texTop = TileManager.TileTop[tileL.Value.id];

                             if (tileU != null && TileManager.TileOutlines.ContainsKey(tileU.Value.id))
                                 texTop = TileManager.TileTop[tileU.Value.id];
                             if (tileD != null && TileManager.TileOutlines.ContainsKey(tileD.Value.id))
                                 texTop = TileManager.TileTop[tileD.Value.id];

                             if (texTop == null) continue;

                             Rectangle r = new Rectangle(i * res, j * res, res, res);
                             Rectangle s = FramingMethod?.Invoke(this, i, j) ?? Rectangle.Empty;

                             sb.Draw(texTop, r, ByteToFrameTop(Top[i, j]), Color.White, 0.05f);

                         }

                         if (tile != null)
                         {
                             if (!TileManager.TileOutlines.ContainsKey(tile.Value.id)) continue;

                             Texture2D tex = TileManager.TileOutlines[tile.Value.id];

                             if (!IsActive(1, 0) || !IsActive(-1, 0) || !IsActive(0, 1) || !IsActive(0, -1)
                              || !IsActive(-1, -1) || !IsActive(-1, 1) || !IsActive(1, 1) || !IsActive(1, -1))
                             {
                                 Rectangle r = new Rectangle(i * res, j * res, res, res);
                                 Rectangle s = ByteToFrame(Outline[i, j]);

                                 if (Outline[i, j] != 0)
                                 {
                                     sb.Draw(tex, r, s, Color.White, 0.05f);
                                 }
                             }
                         }
                     }
             });
        }

        public void ConfigureTop(int x, int y)
        {
            int res = TileManager.drawResolution;

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                {
                    bool IsActive(int a, int b)
                    {
                        if (i + a < 0 || j + b < 0) return false;

                        return Tiles[i + a, j + b] != null;
                    }

                    Tile? tile = Tiles[i, j];

                    if (tile == null)
                    {
                        if (IsActive(1, 0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1))
                        {
                            if ((j * j * 27 % 11 % 7) < 2)
                            {
                                Top[i, j] = 255;
                                continue;
                            }

                            if (IsActive(1, 0))
                            {
                                Top[i, j] = (byte)(30 + (j * 17) % 4);
                                if (!IsActive(0, 1) && IsActive(1, 1) && (j * 17) % 7 < 1)
                                {
                                    Top[i, j] = 34;
                                    Top[i, j + 1] = 35;
                                    j++;
                                }
                            }

                            if (IsActive(-1, 0))
                            {
                                Top[i, j] = (byte)(20 + (j * 17) % 4);
                                if (!IsActive(0, 1) && IsActive(-1, 1) && (j * 27) % 7 < 1)
                                {
                                    Top[i, j] = 24;
                                    Top[i, j + 1] = 25;
                                    j++;
                                }
                            }
                        }
                    }
                }

            for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                {
                    bool IsActive(int a, int b)
                    {
                        if (i + a < 0 || j + b < 0) return false;

                        return Tiles[i + a, j + b] != null;
                    }

                    Tile? tile = Tiles[i, j];

                    if (tile == null)
                    {
                        if (IsActive(1, 0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1))
                        {
                            if (((i + j) * 27 % 11 % 7) < 2)
                            {
                                Top[i, j] = 255;
                                continue;
                            }

                            if (IsActive(0, 1))
                            {
                                Top[i, j] = (byte)((i * 17) % 4);
                                if (!IsActive(1, 0) && IsActive(1, 1) && (i * 17) % 7 < 1)
                                {
                                    Top[i, j] = 4;
                                    Top[i + 1, j] = 5;
                                    i++;
                                }
                            }

                            if (IsActive(0, -1))
                            {
                                Top[i, j] = (byte)(10 + (i * 17) % 4);
                                if (!IsActive(1, 0) && IsActive(1, -1) && (i * 27) % 7 < 1)
                                {
                                    Top[i, j] = 14;
                                    Top[i + 1, j] = 15;
                                    i++;
                                }
                            }
                        }
                    }
                }
        }

        public void ConfigureOutline(int x, int y)
        {
            int res = TileManager.drawResolution;

            Point TL = (LayerHost.GetLayer(Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            ConfigureTop(x, y);

            for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                {
                    //Greedy Meshing
                    Tile? tile = Tiles[i, j];
                    if (tile != null)
                    {
                        bool IsActive(int a, int b)
                        {
                            if (i + a < 0 || j + b < 0) return false;
                            return Tiles[i + a, j + b] != null;
                        }

                        if (!IsActive(1, 0) || !IsActive(-1, 0) || !IsActive(0, 1) || !IsActive(0, -1))
                        {
                            if (IsActive(0, -1) && IsActive(0, 1) && !IsActive(1, 0) && !IsActive(-1, 0))
                            {
                                int posMove = 0;
                                Outline[i, j] = 206;
                                if (IsActive(0, 0) && IsActive(0, 2) && !IsActive(1, 1) && !IsActive(-1, 1))
                                {
                                    Outline[i, j] = (byte)(207 + j % 2);
                                    Outline[i, j + 1] = (byte)(207 + (j + 1) % 2);
                                    posMove++;

                                    if (IsActive(0, 1) && IsActive(0, 3) && !IsActive(1, 2) && !IsActive(-1, 2))
                                    {
                                        int pseudoRandomJ = ((j * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                Outline[i, j] = (byte)(209 + j % 3);
                                                Outline[i, j + 1] = (byte)(209 + (j + 1) % 3);
                                                Outline[i, j + 2] = (byte)(209 + (j + 2) % 3);
                                                posMove++;
                                                break;
                                            case 1:
                                                Outline[i, j] = (byte)(207 + j % 2);
                                                Outline[i, j + 1] = (byte)(207 + (j + 1) % 2);
                                                break;
                                            case 2:
                                                Outline[i, j] = 206;
                                                posMove--;
                                                break;
                                        }
                                    }
                                }

                                j += posMove;
                            }

                            if (IsActive(0, -1) && IsActive(0, 1) && IsActive(1, 0))
                            {
                                int posMove = 0;
                                Outline[i, j] = 7;
                                if (IsActive(0, 0) && IsActive(0, 2) && IsActive(1, 1) && !IsActive(-1, 1))
                                {
                                    Outline[i, j] = (byte)(81 + j % 2);
                                    Outline[i, j + 1] = (byte)(81 + (j + 1) % 2);
                                    posMove++;
                                    if (IsActive(0, 1) && IsActive(0, 3) && IsActive(1, 2) && !IsActive(-1, 2))
                                    {
                                        int pseudoRandomJ = ((j * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                Outline[i, j] = (byte)(91 + j % 3);
                                                Outline[i, j + 1] = (byte)(91 + (j + 1) % 3);
                                                Outline[i, j + 2] = (byte)(91 + (j + 2) % 3);
                                                posMove++;
                                                break;
                                            case 1:
                                                Outline[i, j] = (byte)(81 + j % 2);
                                                Outline[i, j + 1] = (byte)(81 + (j + 1) % 2);
                                                break;
                                            case 2:
                                                Outline[i, j] = 7;
                                                posMove--;
                                                break;
                                        }
                                    }
                                }

                                j += posMove;
                            }
                            else if (IsActive(-1, 0) && IsActive(0, 1) && IsActive(0, -1))
                            {
                                int posMove = 0;
                                Outline[i, j] = 10;
                                if (IsActive(-1, 1) && IsActive(0, 2) && IsActive(0, 0) && !IsActive(1, 1))
                                {
                                    Outline[i, j] = (byte)(111 + j % 2);
                                    Outline[i, j + 1] = (byte)(111 + (j + 1) % 2);
                                    posMove++;
                                    if (IsActive(-1, 2) && IsActive(0, 3) && IsActive(0, 1) && !IsActive(1, 2))
                                    {
                                        int pseudoRandomJ = ((j * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                Outline[i, j] = (byte)(121 + j % 3);
                                                Outline[i, j + 1] = (byte)(121 + (j + 1) % 3);
                                                Outline[i, j + 2] = (byte)(121 + (j + 2) % 3);
                                                posMove++;
                                                break;
                                            case 1:
                                                Outline[i, j] = (byte)(111 + j % 2);
                                                Outline[i, j + 1] = (byte)(111 + (j + 1) % 2);
                                                break;
                                            case 2:
                                                Outline[i, j] = 10;
                                                posMove--;
                                                break;
                                        }
                                    }
                                }

                                j += posMove;
                            }
                        }
                    }
                }

            for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                {
                    //Greedy Meshing
                    Tile? tile = Tiles[i, j];
                    if (tile != null)
                    {
                        bool IsActive(int a, int b)
                        {
                            if (i + a < 0 || j + b < 0) return false;
                            return Tiles[i + a, j + b] != null;
                        }

                        if (!IsActive(1, 0) || !IsActive(-1, 0) || !IsActive(0, 1) || !IsActive(0, -1))
                        {
                            if (!IsActive(0, -1) && !IsActive(0, 1) && IsActive(1, 0) && IsActive(-1, 0))
                            {
                                int posMove = 0;
                                Outline[i, j] = 200;
                                if (!IsActive(1, -1) && !IsActive(1, 1) && IsActive(2, 0) && IsActive(0, 0))
                                {
                                    Outline[i, j] = (byte)(201 + i % 2);
                                    Outline[i + 1, j] = (byte)(201 + (i + 1) % 2);
                                    posMove++;

                                    if (!IsActive(2, -1) && !IsActive(2, 1) && IsActive(3, 0) && IsActive(1, 0))
                                    {
                                        int pseudoRandomJ = ((i * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                Outline[i, j] = (byte)(203 + i % 3);
                                                Outline[i + 1, j] = (byte)(203 + (i + 1) % 3);
                                                Outline[i + 2, j] = (byte)(203 + (i + 2) % 3);
                                                posMove++;
                                                break;
                                            case 1:
                                                Outline[i, j] = (byte)(201 + i % 2);
                                                Outline[i + 1, j] = (byte)(201 + (i + 1) % 2);
                                                break;
                                            case 2:
                                                Outline[i, j] = 200;
                                                posMove--;
                                                break;
                                        }
                                    }
                                }

                                i += posMove;
                            }
                            //Up space
                            if (IsActive(-1, 0) && IsActive(1, 0) && IsActive(0, 1))
                            {
                                int posMove = 0;
                                Outline[i, j] = 1;
                                if (IsActive(0, 0) && IsActive(2, 0) && IsActive(1, 1) && !IsActive(1, -1))
                                {
                                    Outline[i, j] = (byte)(21 + i % 2);
                                    Outline[i + 1, j] = (byte)(21 + (i + 1) % 2);
                                    posMove++;

                                    if (IsActive(1, 0) && IsActive(3, 0) && IsActive(2, 1) && !IsActive(2, -1))
                                    {
                                        int pseudoRandomJ = ((i * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                Outline[i, j] = (byte)(31 + i % 3);
                                                Outline[i + 1, j] = (byte)(31 + (i + 1) % 3);
                                                Outline[i + 2, j] = (byte)(31 + (i + 2) % 3);
                                                posMove++;
                                                break;
                                            case 1:
                                                Outline[i, j] = (byte)(21 + i % 2);
                                                Outline[i + 1, j] = (byte)(21 + (i + 1) % 2);
                                                break;
                                            case 2:
                                                Outline[i, j] = 1;
                                                posMove--;
                                                break;
                                        }
                                    }
                                }

                                i += posMove;
                            }
                            //Down space
                            else if (IsActive(-1, 0) && IsActive(1, 0) && IsActive(0, -1))
                            {
                                int posMove = 0;
                                Outline[i, j] = 4;
                                if (IsActive(0, 0) && IsActive(2, 0) && IsActive(1, -1) && !IsActive(1, 1))
                                {
                                    Outline[i, j] = (byte)(51 + i % 2);
                                    Outline[i + 1, j] = (byte)(51 + (i + 1) % 2);
                                    posMove++;
                                    if (IsActive(1, 0) && IsActive(3, 0) && IsActive(2, -1) && !IsActive(2, 1))
                                    {
                                        int pseudoRandomJ = ((i * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                Outline[i, j] = (byte)(61 + i % 3);
                                                Outline[i + 1, j] = (byte)(61 + (i + 1) % 3);
                                                Outline[i + 2, j] = (byte)(61 + (i + 2) % 3);
                                                posMove++;
                                                break;
                                            case 1:
                                                Outline[i, j] = (byte)(51 + i % 2);
                                                Outline[i + 1, j] = (byte)(51 + (i + 1) % 2);
                                                break;
                                            case 2:
                                                Outline[i, j] = 4;
                                                posMove--;
                                                break;
                                        }
                                    }
                                }

                                i += posMove;
                            }
                            else
                            {
                                if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && !IsActive(0, -1))
                                {
                                    if (IsActive(1, 1))
                                        Outline[i, j] = 11;
                                    else
                                        Outline[i, j] = 40;
                                }
                                else if (IsActive(-1, 0) && IsActive(0, 1) && !IsActive(1, 0) && !IsActive(0, -1))
                                {
                                    if (IsActive(-1, 1))
                                        Outline[i, j] = 12;
                                    else
                                        Outline[i, j] = 41;
                                }
                                else if (IsActive(-1, 0) && IsActive(0, -1) && !IsActive(1, 0) && !IsActive(0, 1))
                                {
                                    if (IsActive(-1, -1))
                                        Outline[i, j] = 13;
                                    else
                                        Outline[i, j] = 42;
                                }
                                else if (IsActive(1, 0) && IsActive(0, -1) && !IsActive(-1, 0) && !IsActive(0, 1))
                                {
                                    if (IsActive(1, -1))
                                        Outline[i, j] = 14;
                                    else
                                        Outline[i, j] = 43;
                                }

                                else if (!IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && !IsActive(0, -1))
                                    Outline[i, j] = 15;
                                else if (!IsActive(-1, 0) && !IsActive(0, 1) && !IsActive(1, 0) && IsActive(0, -1))
                                    Outline[i, j] = 16;
                                else if (!IsActive(-1, 0) && !IsActive(0, -1) && IsActive(1, 0) && !IsActive(0, 1))
                                    Outline[i, j] = 17;
                                else if (!IsActive(1, 0) && !IsActive(0, -1) && IsActive(-1, 0) && !IsActive(0, 1))
                                    Outline[i, j] = 18;
                                else if (!IsActive(1, 0) && !IsActive(0, -1) && !IsActive(-1, 0) && !IsActive(0, 1))
                                    Outline[i, j] = (byte)(19 + i % 2);
                            }
                            //MiscConfig1

                            if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && IsActive(0, -1)
                                  && IsActive(1, -1) && !IsActive(1, 1))
                            {
                                Outline[i, j] = 44;
                            }
                            else if (!IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && IsActive(-1, -1) && !IsActive(-1, 1))

                            {
                                Outline[i, j] = 45;
                            }
                            else if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && IsActive(0, -1)
                                 && !IsActive(1, -1) && IsActive(1, 1))
                            {
                                Outline[i, j] = 46;
                            }
                            else if (!IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(-1, -1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 47;
                            }


                            //MiscConfig2

                            if (IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && !IsActive(0, -1)
                                 && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 70;
                            }
                            else if (IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && !IsActive(0, -1)
                                 && IsActive(1, 1) && !IsActive(-1, 1))

                            {
                                Outline[i, j] = 71;
                            }
                            else if (IsActive(1, 0) && !IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && IsActive(-1, -1) && !IsActive(1, -1))
                            {
                                Outline[i, j] = 72;
                            }
                            else if (IsActive(1, 0) && !IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(-1, -1) && IsActive(1, -1))
                            {
                                Outline[i, j] = 73;
                            }

                            //MiscConfig6

                            if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(1, -1) && !IsActive(1, 1))
                            {
                                Outline[i, j] = 86;
                            }
                            else if (!IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                 && !IsActive(-1, -1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 87;
                            }
                            else if (IsActive(1, 0) && !IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(-1, -1) && !IsActive(1, -1))
                            {
                                Outline[i, j] = 88;
                            }
                            else if (IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && !IsActive(0, -1)
                                && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 89;
                            }

                        }
                        else
                        {
                            if (!IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 74;
                            }
                            else if (!IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 75;
                            }
                            else if (!IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 76;
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 77;
                            }

                            else if (!IsActive(-1, -1) && IsActive(1, -1) && IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 78;
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 79;
                            }
                            else if (IsActive(-1, -1) && IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 80;
                            }
                            else if (IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 101;
                            }

                            else if (!IsActive(-1, -1) && IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 102;
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 83;
                            }
                            else if (!IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 84;
                            }
                            else if (IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 85;
                            }
                            else if (!IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 90;
                            }

                            else if (!IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                Outline[i, j] = 103;
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                Outline[i, j] = 104;
                            }
                        }
                    }
                }
        }
    }
}

