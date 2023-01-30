using QueefCord.Content.Entities;
using QueefCord.Content.Maths;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
using QueefCord.Core.Entities;
using QueefCord.Core.Helpers;
using QueefCord.Core.Input;
using QueefCord.Core.Interfaces;
using QueefCord.Core.IO;
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
using System.Security.Cryptography;
using QueefCord.Core.Graphics;
using QueefCord.Core.Entities.EntitySystems;

namespace QueefCord.Core.Tiles
{
    public partial class TileManager : IUpdate
    {
        public Rectangle ByteToFrame(byte n)
        {
            Rectangle ManhattanToRect(int x, int y) => new Rectangle(x, y, 1, 1).Multiply(frameResolution);
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
            Rectangle ManhattanToRect(int x, int y) => new Rectangle(x, y, 1, 1).Multiply(frameResolution);
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

        public void RenderOutline(SpriteBatch sb, string ID)
        {
            int res = drawResolution;

            Point TL = (LayerHost.GetLayer(ParentWorld.Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            LayerHost.GetLayer(ParentWorld.Layer).MapHost.Maps.DrawToBatchedMap("TileTextureMap",
             (SpriteBatch sb) =>
             {
                 for (int i = Math.Max(TL.X - 1, 0); i < Math.Min(TL.X + BR.X + 1, ParentWorld.TileBounds.X); i++)
                     for (int j = Math.Max(TL.Y - 1, 0); j < Math.Min(TL.Y + BR.Y + 2, ParentWorld.TileBounds.Y); j++)
                     {
                         Tile tile = GetTile(i, j, ID);
                         if (!tile.Active && tile.Top != 255
                         && i == Math.Clamp(i, 1, ParentWorld.TileBounds.X)
                         && j == Math.Clamp(j, 1, ParentWorld.TileBounds.Y))
                         {

                             Tile tileR = GetTile(i + 1, j, ID);
                             Tile tileL = GetTile(i - 1, j, ID);
                             Tile tileU = GetTile(i, j - 1, ID);
                             Tile tileD = GetTile(i, j + 1, ID);

                             Texture2D texTop = null;

                             if (tileR.Active && TileTop.ContainsKey(tileR.id))
                                 texTop = TileTop[tileR.id];
                             if (tileL.Active && TileTop.ContainsKey(tileL.id))
                                 texTop = TileTop[tileL.id];

                             if (tileU.Active && TileTop.ContainsKey(tileU.id))
                                 texTop = TileTop[tileU.id];
                             if (tileD.Active && TileTop.ContainsKey(tileD.id))
                                 texTop = TileTop[tileD.id];


                             if (texTop == null) continue;

                             Rectangle r = new Rectangle(i * res, j * res, res, res);
                             sb.Draw(texTop, r, ByteToFrameTop(GetTile(i, j, ID).Top), Color.White, 0.05f);
                         }

                         if (tile.Active)
                         {
                             if (!TileOutlines.ContainsKey(tile.id)) continue;

                             Texture2D tex = TileOutlines[tile.id];

                             if (tile.Outline != 0)
                             {
                                 Rectangle r = new Rectangle(i * res, j * res, res, res);
                                 Rectangle s = ByteToFrame(tile.Outline);


                                 sb.Draw(tex, r, s, Color.White, 0.05f);
                             }
                         }
                     }
             });
        }

        public void ConfigureTop(int x, int y, string ID)
        {
            int res = drawResolution;

            Point TL = (LayerHost.GetLayer(ParentWorld.Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            void SetTop(int x, int y, byte n) => this.SetTop(x, y, ID, n);

            for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                {
                    bool IsActive(int a, int b)
                    {
                        if (i + a < 0 || j + b < 0) return false;

                        return GetTile(i + a, j + b, ID).Active;
                    }

                    Tile tile = GetTile(i, j, ID);

                    if (!tile.Active)
                    {
                        if (IsActive(1, 0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1))
                        {
                            if ((j * j * 27 % 11 % 7) < 2)
                            {
                                SetTop(i, j, 255);
                                continue;
                            }

                            if (IsActive(1, 0))
                            {
                                SetTop(i, j, (byte)(30 + (j * 17) % 4));
                                if (!IsActive(0, 1) && IsActive(1, 1) && (j * 17) % 7 < 1)
                                {
                                    SetTop(i, j, 34);
                                    SetTop(i, j + 1, 35);
                                    j++;
                                }
                            }

                            if (IsActive(-1, 0))
                            {
                                SetTop(i, j, (byte)(20 + (j * 17) % 4));
                                if (!IsActive(0, 1) && IsActive(-1, 1) && (j * 27) % 7 < 1)
                                {
                                    SetTop(i, j, 24);
                                    SetTop(i, j + 1, 25);
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

                        return GetTile(i + a, j + b, ID).Active;
                    }

                    Tile tile = GetTile(i, j, ID);

                    if (!tile.Active)
                    {
                        if (IsActive(1, 0) || IsActive(-1, 0) || IsActive(0, 1) || IsActive(0, -1))
                        {
                            if (((i + j) * 27 % 11 % 7) < 2)
                            {
                                SetTop(i, j, 255);
                                continue;
                            }

                            if (IsActive(0, 1))
                            {
                                SetTop(i, j, (byte)((i * 17) % 4));
                                if (!IsActive(1, 0) && IsActive(1, 1) && (i * 17) % 7 < 1)
                                {
                                    SetTop(i, j, 4);
                                    SetTop(i + 1, j, 5);
                                    i++;
                                }
                            }

                            if (IsActive(0, -1))
                            {
                                SetTop(i, j, (byte)(10 + (i * 17) % 4));
                                if (!IsActive(1, 0) && IsActive(1, -1) && (i * 27) % 7 < 1)
                                {
                                    SetTop(i, j, 14);
                                    SetTop(i + 1, j, 15);
                                    i++;
                                }
                            }
                        }
                    }
                }
        }

        public void ConfigureOutline(int x, int y, string ID)
        {
            int res = drawResolution;

            Point TL = (LayerHost.GetLayer(ParentWorld.Layer).Camera.Transform.Position / res).ToPoint();
            Point BR = (Renderer.BackBufferSize.ToVector2() / res).ToPoint();

            ConfigureTop(x, y, ID);
            void SetOutline(int x, int y, byte n) => this.SetOutline(x, y, ID, n);

            for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                {
                    SetOutline(i, j, 0);
                }

            for (int i = Math.Max(TL.X - 1, 0); i < x + 3; i++)
                for (int j = Math.Max(TL.Y - 1, 0); j < y + 3; j++)
                {
                    //Greedy Meshing
                    Tile tile = GetTile(i, j, ID);
                    if (tile.Active)
                    {
                        bool IsActive(int a, int b)
                        {
                            if (i + a < 0 || j + b < 0) return false;
                            return GetTile(i + a, j + b, ID).Active;
                        }

                        if (!IsActive(1, 0) || !IsActive(-1, 0) || !IsActive(0, 1) || !IsActive(0, -1))
                        {
                            if (IsActive(0, -1) && IsActive(0, 1) && !IsActive(1, 0) && !IsActive(-1, 0))
                            {
                                int posMove = 0;
                                SetOutline(i, j, 206);
                                if (IsActive(0, 0) && IsActive(0, 2) && !IsActive(1, 1) && !IsActive(-1, 1))
                                {
                                    SetOutline(i, j, (byte)(207 + j % 2));
                                    SetOutline(i, j + 1, (byte)(207 + (j + 1) % 2));
                                    posMove++;

                                    if (IsActive(0, 1) && IsActive(0, 3) && !IsActive(1, 2) && !IsActive(-1, 2))
                                    {
                                        int pseudoRandomJ = ((j * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                SetOutline(i, j, (byte)(209 + j % 3));
                                                SetOutline(i, j + 1, (byte)(209 + (j + 1) % 3));
                                                SetOutline(i, j + 2, (byte)(209 + (j + 2) % 3));
                                                posMove++;
                                                break;
                                            case 1:
                                                SetOutline(i, j, (byte)(207 + j % 2));
                                                SetOutline(i, j + 1, (byte)(207 + (j + 1) % 2));
                                                break;
                                            case 2:
                                                SetOutline(i, j, 206);
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
                                SetOutline(i, j, 7);
                                if (IsActive(0, 0) && IsActive(0, 2) && IsActive(1, 1) && !IsActive(-1, 1))
                                {
                                    SetOutline(i, j, (byte)(81 + j % 2));
                                    SetOutline(i, j + 1, (byte)(81 + (j + 1) % 2));
                                    posMove++;
                                    if (IsActive(0, 1) && IsActive(0, 3) && IsActive(1, 2) && !IsActive(-1, 2))
                                    {
                                        int pseudoRandomJ = ((j * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                SetOutline(i, j, (byte)(91 + j % 3));
                                                SetOutline(i, j + 1, (byte)(91 + (j + 1) % 3));
                                                SetOutline(i, j + 2, (byte)(91 + (j + 2) % 3));
                                                posMove++;
                                                break;
                                            case 1:
                                                SetOutline(i, j, (byte)(81 + j % 2));
                                                SetOutline(i, j + 1, (byte)(81 + (j + 1) % 2));
                                                break;
                                            case 2:
                                                SetOutline(i, j, 7);
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
                                SetOutline(i, j, 10);
                                if (IsActive(-1, 1) && IsActive(0, 2) && IsActive(0, 0) && !IsActive(1, 1))
                                {
                                    SetOutline(i, j, (byte)(111 + j % 2));
                                    SetOutline(i, j + 1, (byte)(111 + (j + 1) % 2));
                                    posMove++;
                                    if (IsActive(-1, 2) && IsActive(0, 3) && IsActive(0, 1) && !IsActive(1, 2))
                                    {
                                        int pseudoRandomJ = ((j * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                SetOutline(i, j, (byte)(121 + j % 3));
                                                SetOutline(i, j + 1, (byte)(121 + (j + 1) % 3));
                                                SetOutline(i, j + 2, (byte)(121 + (j + 2) % 3));
                                                posMove++;
                                                break;
                                            case 1:
                                                SetOutline(i, j, (byte)(111 + j % 2));
                                                SetOutline(i, j + 1, (byte)(111 + (j + 1) % 2));
                                                break;
                                            case 2:
                                                SetOutline(i, j, 10);
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
                    Tile tile = GetTile(i, j, ID);
                    if (tile.Active)
                    {
                        bool IsActive(int a, int b)
                        {
                            if (i + a < 0 || j + b < 0) return false;
                            return GetTile(i + a, j + b, ID).Active;
                        }

                        if (!IsActive(1, 0) || !IsActive(-1, 0) || !IsActive(0, 1) || !IsActive(0, -1))
                        {
                            if (!IsActive(0, -1) && !IsActive(0, 1) && IsActive(1, 0) && IsActive(-1, 0))
                            {
                                int posMove = 0;
                                SetOutline(i, j, 200);
                                if (!IsActive(1, -1) && !IsActive(1, 1) && IsActive(2, 0) && IsActive(0, 0))
                                {
                                    SetOutline(i, j, (byte)(201 + i % 2));
                                    SetOutline(i + 1, j, (byte)(201 + (i + 1) % 2));
                                    posMove++;

                                    if (!IsActive(2, -1) && !IsActive(2, 1) && IsActive(3, 0) && IsActive(1, 0))
                                    {
                                        int pseudoRandomJ = ((i * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                SetOutline(i, j, (byte)(203 + i % 3));
                                                SetOutline(i + 1, j, (byte)(203 + (i + 1) % 3));
                                                SetOutline(i + 2, j, (byte)(203 + (i + 2) % 3));
                                                posMove++;
                                                break;
                                            case 1:
                                                SetOutline(i, j, (byte)(201 + i % 2));
                                                SetOutline(i + 1, j, (byte)(201 + (i + 1) % 2));
                                                break;
                                            case 2:
                                                SetOutline(i, j, 200);
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
                                SetOutline(i, j, 1);
                                if (IsActive(0, 0) && IsActive(2, 0) && IsActive(1, 1) && !IsActive(1, -1))
                                {
                                    SetOutline(i, j, (byte)(21 + i % 2));
                                    SetOutline(i + 1, j, (byte)(21 + (i + 1) % 2));
                                    posMove++;

                                    if (IsActive(1, 0) && IsActive(3, 0) && IsActive(2, 1) && !IsActive(2, -1))
                                    {
                                        int pseudoRandomJ = ((i * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                SetOutline(i, j, (byte)(31 + i % 3));
                                                SetOutline(i + 1, j, (byte)(31 + (i + 1) % 3));
                                                SetOutline(i + 2, j, (byte)(31 + (i + 2) % 3));
                                                posMove++;
                                                break;
                                            case 1:
                                                SetOutline(i, j, (byte)(21 + i % 2));
                                                SetOutline(i + 1, j, (byte)(21 + (i + 1) % 2));
                                                break;
                                            case 2:
                                                SetOutline(i, j, 1);
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
                                SetOutline(i, j, 4);
                                if (IsActive(0, 0) && IsActive(2, 0) && IsActive(1, -1) && !IsActive(1, 1))
                                {
                                    SetOutline(i, j, (byte)(51 + i % 2));
                                    SetOutline(i + 1, j, (byte)(51 + (i + 1) % 2));
                                    posMove++;
                                    if (IsActive(1, 0) && IsActive(3, 0) && IsActive(2, -1) && !IsActive(2, 1))
                                    {
                                        int pseudoRandomJ = ((i * 17) % 7) % 3;
                                        switch (pseudoRandomJ)
                                        {
                                            case 0:
                                                SetOutline(i, j, (byte)(61 + i % 3));
                                                SetOutline(i + 1, j, (byte)(61 + (i + 1) % 3));
                                                SetOutline(i + 2, j, (byte)(61 + (i + 2) % 3));
                                                posMove++;
                                                break;
                                            case 1:
                                                SetOutline(i, j, (byte)(51 + i % 2));
                                                SetOutline(i + 1, j, (byte)(51 + (i + 1) % 2));
                                                break;
                                            case 2:
                                                SetOutline(i, j, 4);
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
                                        SetOutline(i, j, 11);
                                    else
                                        SetOutline(i, j, 40);
                                }
                                else if (IsActive(-1, 0) && IsActive(0, 1) && !IsActive(1, 0) && !IsActive(0, -1))
                                {
                                    if (IsActive(-1, 1))
                                        SetOutline(i, j, 12);
                                    else
                                        SetOutline(i, j, 41);
                                }
                                else if (IsActive(-1, 0) && IsActive(0, -1) && !IsActive(1, 0) && !IsActive(0, 1))
                                {
                                    if (IsActive(-1, -1))
                                        SetOutline(i, j, 13);
                                    else
                                        SetOutline(i, j, 42);
                                }
                                else if (IsActive(1, 0) && IsActive(0, -1) && !IsActive(-1, 0) && !IsActive(0, 1))
                                {
                                    if (IsActive(1, -1))
                                        SetOutline(i, j, 14);
                                    else
                                        SetOutline(i, j, 43);
                                }

                                else if (!IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && !IsActive(0, -1))
                                    SetOutline(i, j, 15);
                                else if (!IsActive(-1, 0) && !IsActive(0, 1) && !IsActive(1, 0) && IsActive(0, -1))
                                    SetOutline(i, j, 16);
                                else if (!IsActive(-1, 0) && !IsActive(0, -1) && IsActive(1, 0) && !IsActive(0, 1))
                                    SetOutline(i, j, 17);
                                else if (!IsActive(1, 0) && !IsActive(0, -1) && IsActive(-1, 0) && !IsActive(0, 1))
                                    SetOutline(i, j, 18);
                                else if (!IsActive(1, 0) && !IsActive(0, -1) && !IsActive(-1, 0) && !IsActive(0, 1))
                                    SetOutline(i, j, (byte)(19 + i % 2));
                            }
                            //MiscConfig1

                            if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && IsActive(0, -1)
                                  && IsActive(1, -1) && !IsActive(1, 1))
                            {
                                SetOutline(i, j, 44);
                            }
                            else if (!IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && IsActive(-1, -1) && !IsActive(-1, 1))

                            {
                                SetOutline(i, j, 45);
                            }
                            else if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && IsActive(0, -1)
                                 && !IsActive(1, -1) && IsActive(1, 1))
                            {
                                SetOutline(i, j, 46);
                            }
                            else if (!IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(-1, -1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 47);
                            }


                            //MiscConfig2

                            if (IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && !IsActive(0, -1)
                                 && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 70);
                            }
                            else if (IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && !IsActive(0, -1)
                                 && IsActive(1, 1) && !IsActive(-1, 1))

                            {
                                SetOutline(i, j, 71);
                            }
                            else if (IsActive(1, 0) && !IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && IsActive(-1, -1) && !IsActive(1, -1))
                            {
                                SetOutline(i, j, 72);
                            }
                            else if (IsActive(1, 0) && !IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(-1, -1) && IsActive(1, -1))
                            {
                                SetOutline(i, j, 73);
                            }

                            //MiscConfig6

                            if (IsActive(1, 0) && IsActive(0, 1) && !IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(1, -1) && !IsActive(1, 1))
                            {
                                SetOutline(i, j, 86);
                            }
                            else if (!IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                 && !IsActive(-1, -1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 87);
                            }
                            else if (IsActive(1, 0) && !IsActive(0, 1) && IsActive(-1, 0) && IsActive(0, -1)
                                && !IsActive(-1, -1) && !IsActive(1, -1))
                            {
                                SetOutline(i, j, 88);
                            }
                            else if (IsActive(1, 0) && IsActive(0, 1) && IsActive(-1, 0) && !IsActive(0, -1)
                                && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 89);
                            }

                        }
                        else
                        {
                            if (!IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 74);
                            }
                            else if (!IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 75);
                            }
                            else if (!IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 76);
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 77);
                            }

                            else if (!IsActive(-1, -1) && IsActive(1, -1) && IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 78);
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 79);
                            }
                            else if (IsActive(-1, -1) && IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 80);
                            }
                            else if (IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 101);
                            }

                            else if (!IsActive(-1, -1) && IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 102);
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 83);
                            }
                            else if (!IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 84);
                            }
                            else if (IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 85);
                            }
                            else if (!IsActive(-1, -1) && !IsActive(1, -1) && !IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 90);
                            }

                            else if (!IsActive(-1, -1) && IsActive(1, -1) && !IsActive(1, 1) && IsActive(-1, 1))
                            {
                                SetOutline(i, j, 103);
                            }
                            else if (IsActive(-1, -1) && !IsActive(1, -1) && IsActive(1, 1) && !IsActive(-1, 1))
                            {
                                SetOutline(i, j, 104);
                            }
                        }
                    }
                }
        }
    }
}
