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
using QueefCord.Core.Tiles;

namespace QueefCord.Content.Entities
{
    public class ChunkAllocator : IEntityModifier
    {
        public List<Point> Chunks = new List<Point>();
        private bool Static = false;

        private Entity2D Entity;

        public ChunkAllocator(Entity2D e2D, bool Static)
        {
            UpdateChunks(e2D);
            this.Static = Static;
            Entity = e2D;
        }

        public List<Point> UpdateChunks(Entity entity)
        {
            if (!(entity is IUpdate updateable)) return null;

            Chunks.Clear();
            foreach(var p in World.CurrentWorld.ActiveChunks)
            {
                p.Value.Entities.Remove(updateable);
            }

            int Left = (int)entity.Transform.Position.X / (World.CurrentWorld.ChunkSize.X * TileManager.drawResolution);
            int Up = (int)entity.Transform.Position.Y / (World.CurrentWorld.ChunkSize.Y * TileManager.drawResolution);

            int Right = Left;
            int Down = Up;

            if (entity is Entity2D e)
            {
                Right = (int)(entity.Transform.Position.X + e.Size.X) / (World.CurrentWorld.ChunkSize.X * TileManager.drawResolution);
                Down = (int)(entity.Transform.Position.Y + e.Size.Y) / (World.CurrentWorld.ChunkSize.Y * TileManager.drawResolution);
            }

            for (int i = Left; i <= Right; i++)
            {
                for (int j = Up; j <= Down; j++)
                {
                    Chunks.Add(new Point(i, j));
                }
            }

            foreach (var chunks in Chunks)
                World.CurrentWorld.ActiveChunks[chunks].Entities.Add(updateable);

            return Chunks;
        }

        public void Update(in EntityCore entity, GameTime gameTime)
        {
            if (Static)
            {
                if (Chunks.Count == 0) UpdateChunks(entity);
            }
            else
            {
                UpdateChunks(entity);
            }
        }

        public void Dispose()
        {
            foreach(var chunk in Chunks)
            {
                World.CurrentWorld.ActiveChunks[chunk].Entities.Remove(Entity);
            }
        }
    }
}
