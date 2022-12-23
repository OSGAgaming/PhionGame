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
    public class Player : KinematicEntity2D, IUpdate, IDraw, IMappable
    {
        public string Layer => "Default";
        public SlotInfo[] Items => BagItems.Concat(HotbarItems).ToArray();

        [JsonIgnore]
        public static Player LocalPlayer;

        public SlotInfo[] BagItems;
        public SlotInfo[] HotbarItems;
        public SlotInfo[] CraftItems;

        public int InventorySpace = 25;
        public int HotbarSpace = 5;
        public int CraftSpace = 4;

        private Texture2D PlayerTexture;
        public Player()
        {
            PlayerTexture = Assets<Texture2D>.Get("Textures/Player/PlayerTest");

            AddMechanic(new PlayerMovement(0.2f, 6));
            AddMechanic(new PlayerItemUsage());
            AddMechanic(new BaseNPCStats(100, 20, 5));
            AddMechanic(new PlayerAnimation(8, 24, 48, 48, 5));
            AddMechanic(new RigidBody(0.13f, 0.98f));
            AddMechanic(new EntityCollision(this));

            Get<EntityCollision>().AddCollisionBox(PlayerTexture.Bounds);

            Friction = Vector2.One * 0.9f;
            Size = new Vector2(48, 48);
            Trim = new Point(-19, -16);

            BagItems = new SlotInfo[InventorySpace];
            HotbarItems = new SlotInfo[HotbarSpace];
            CraftItems = new SlotInfo[CraftSpace];

            LocalPlayer = this;
        }

        public override void PostSave(BinaryWriter bw)
        {
            bw.Write(BagItems.Length);
            bw.Write(HotbarItems.Length);

            for(int i = 0; i < BagItems.Length; i++)
            {
                bw.Write(BagItems[i].item == null);

                if (BagItems[i].item == null) continue;

                bw.Write(BagItems[i].item.GetType());
                bw.Write(BagItems[i].stack);
            }

            for (int i = 0; i < HotbarItems.Length; i++)
            {
                bw.Write(HotbarItems[i].item == null);

                if (HotbarItems[i].item == null) continue;

                bw.Write(HotbarItems[i].item.GetType());
                bw.Write(HotbarItems[i].stack);
            }
        }

        public override void PostLoad(BinaryReader br, Entity2D e)
        {
            int bagItems = br.ReadInt32();
            int hotbarItems = br.ReadInt32();

            Player p = e as Player;

            for (int i = 0; i < bagItems; i++)
            {
                if (br.ReadBoolean()) continue;

                p.BagItems[i].item = Activator.CreateInstance(br.ReadType()) as IStoreable;
                p.BagItems[i].stack = br.ReadInt32();
            }

            for (int i = 0; i < hotbarItems; i++)
            {
                if (br.ReadBoolean()) continue;

                p.HotbarItems[i].item = Activator.CreateInstance(br.ReadType()) as IStoreable;
                p.HotbarItems[i].stack = br.ReadInt32();
            }
        }
        public void DrawToMiniMap(SpriteBatch sb) => Utils.DrawBoxFill(new RectangleF(Transform.Position / MiniMap.Resolution, Size / MiniMap.Resolution), Color.Red, 1 - Center.Y / 100000f);

        public void Draw(SpriteBatch sb)
        {
            Color color = Color.Lerp(Color.IndianRed, Color.White, 1 - Get<BaseNPCStats>().ImmunityFrames / 30f);

            sb.Draw(PlayerTexture, Transform.Position, PlayerTexture.Bounds, color, 0);

            Get<PlayerItemUsage>().Draw(sb);
        }
    }
}
