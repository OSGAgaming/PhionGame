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

namespace QueefCord.Core.Tiles
{
    public partial class TileManager : IUpdate, ISerializable
    {
        public static TileManager Instance;
        public Dictionary<string, TileSet> TileSets = new Dictionary<string, TileSet>();

        internal const int width = 1000;
        internal const int height = 1000;

        public static int drawResolution = 16;
        public static int frameResolution = 16;

        public static int ActiveTileSet = 0;
        public static short ActiveAtlas = 0;

        public static bool PlaceMode;

        public TileManager()
        {
            AddTileSets();
            Instance = this;
        }

        public void AddTileSet(string id, string FramingMethod, bool solid = false)
        {
            TileSet tileSet = new TileSet(FramingMethod);

            tileSet.Index = TileSets.Count;
            tileSet.Solid = solid;
            tileSet.ID = id;

            TileSets.Add(id, tileSet);
        }
        
        public void Update(GameTime gameTime)
        {
            foreach (TileSet set in TileSets.Values)
            {
                if (!SceneHolder.CurrentScene.DistinctElements.Contains(set))
                    SceneHolder.CurrentScene.AddEntity(set);
            }

            if (PlaceMode)
            {
                if (GameInput.Instance["Tab"].IsJustPressed())
                {
                    ActiveTileSet++;
                    ActiveTileSet %= TileSets.Count;

                    Logger.NewText("TileSet switched to: " + TileSets.Keys.ToArray()[ActiveTileSet]);
                }

                if (GameInput.Instance["L"].IsJustPressed())
                {
                    ActiveAtlas = (short)((ActiveAtlas + 1) % TileTexturePaths.Count);
                    Logger.NewText("Atlas switched to: " + TileTexturePaths[ActiveAtlas]);
                }
            }

            if (GameInput.Instance["Q"].IsJustPressed())
            {
                PlaceMode = !PlaceMode;
                Logger.NewText("Place mode is " + (PlaceMode ? "on" : "off"));
            }

        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(width);
            bw.Write(height);

            bw.Write(TileSets.Count);

            foreach (KeyValuePair<string, TileSet> tileset in TileSets)
                tileset.Value.Save(bw);
        }

        public IComponent Load(BinaryReader br)
        {
            int width = br.ReadInt32();
            int height = br.ReadInt32();

            int setCount = br.ReadInt32();

            TileManager tm = new TileManager();

            for (int i = 0; i < setCount; i++)
            {
                TileSet ts = TileSet.Load(br);
                tm.TileSets.Add(ts.ID, ts);
            }

            return tm;
        }
    }
}
