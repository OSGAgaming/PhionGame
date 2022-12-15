using QueefCord.Content.UI;
using QueefCord.Core.Helpers;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QueefCord.Core.IO
{
    class NullTileRemover : JsonConverter
    {
        public override bool CanConvert(Type objectType) => (objectType == typeof(Tile?[,]));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int w = TileManager.width;
            int h = TileManager.height;
            Tile?[,] tiles = (Tile?[,])value;

            serializer.Serialize(writer, tiles.Flatten().Where(n => n != null).Count());

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    Tile? tile = tiles[i, j];
                    if(tile != null)
                    {
                        serializer.Serialize(writer, i);
                        serializer.Serialize(writer, j);
                        serializer.Serialize(writer, tile);
                    }
                }
        }

        public override bool CanRead => false;

        /*
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int w = TileManager.width;
            int h = TileManager.height;

            Tile?[,] tiles = new Tile?[TileManager.width, TileManager.height];

            int no = reader.ReadAsInt32().Value;

            for(int i = 0; i < no; i++)
            {
                tiles[reader.ReadAsInt32().Value, reader.ReadAsInt32().Value] = reader.read().Value;
            }
        }
        */
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    class DefaultItemRemover : JsonConverter
    {
        public override bool CanConvert(Type objectType) => (objectType == typeof(SlotInfo[]));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, ((SlotInfo[])value).Where(c => c.item != null).ToArray());

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => throw new NotImplementedException();
    }
}