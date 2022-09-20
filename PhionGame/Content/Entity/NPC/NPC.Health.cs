using QueefCord.Content.UI;
using QueefCord.Core.Entities;
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
    public class BaseNPCStats : IEntityModifier
    {
        public int MaxHealth { get; set; }
        public int Health { get; set; }

        public int MaxMana { get; set; }
        public int Mana { get; set; }

        public int MaxExperience { get; set; }
        public int Experience { get; set; }

        public int MaxImmunityFrames { get; set; } = 30;
        public int ImmunityFrames { get; set; }


        public BaseNPCStats(int MaxHealth, int MaxMana = 0, int MaxExperience = 0)
        {
            this.MaxHealth = MaxHealth;
            Health = MaxHealth;

            this.MaxMana = MaxMana;
            Mana = MaxMana;

            this.MaxExperience = MaxExperience;
            Experience = MaxExperience;
        }

        public void TakeDamage(int dmg)
        {
            if (ImmunityFrames == 0)
                Health -= dmg;

            ImmunityFrames = MaxImmunityFrames;
        }

        public void Update(in Entity entity, GameTime gameTime)
        {
            if (ImmunityFrames > 0) ImmunityFrames--;

            Health = Math.Clamp(Health, 0, MaxHealth);
            Mana = Math.Clamp(Mana, 0, MaxMana);
            Experience = Math.Clamp(Experience, 0, MaxExperience);

            if (Health <= 0)
            {
                if (entity is NPC n)
                    n.OnDeath(gameTime);

                SceneHolder.CurrentScene.RemoveEntity(entity);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
