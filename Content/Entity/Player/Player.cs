using QueefCord.Content.Graphics;
using QueefCord.Content.UI;
using QueefCord.Core.DataStructures;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace QueefCord.Content.Entities
{
    public class Player : Entity2D, IUpdate, IDraw, IMappable
    {
        public string Layer => "Default";
        public static Player LocalPlayer;
        public RigidBody rigidBody;

        private Texture2D PlayerTexture;

        public Player()
        {
            PlayerTexture = Assets<Texture2D>.Get("Textures/Player/PlayerTest");

            AddMechanic(new PlayerMovement(0.3f, 8));
            AddMechanic(new PlayerInventory());
            AddMechanic(new BaseNPCStats(100, 20, 5));
            AddMechanic(new PlayerAnimation(8, 24, 48, 48, 5));
            AddMechanic(new RigidBody(0.3f, new Vector2(0.85f, 0.94f),0.87f));
            AddMechanic(new EntityCollision(this));

            Get<EntityCollision>().AddCollisionBox(PlayerTexture.Bounds);

            Size = new Vector2(48, 48);
            Trim = new Point(-19, -16);

            rigidBody = Get<RigidBody>();

            LocalPlayer = this;
        }

        public void DrawToMiniMap(SpriteBatch sb) => Utils.DrawBoxFill(new RectangleF(Transform.Position / MiniMap.Resolution, Size / MiniMap.Resolution), Color.Red, 1 - Center.Y / 100000f);

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(PlayerTexture, Transform.Position, PlayerTexture.Bounds, Color.White, 0);

            Get<PlayerInventory>().Draw(sb);
        }
    }
}
