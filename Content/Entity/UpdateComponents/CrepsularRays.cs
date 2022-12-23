
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using QueefCord.Core.Entities;
using QueefCord.Core.Graphics;
using QueefCord.Core.Interfaces;
using QueefCord.Core.Resources;
using System.Collections.Generic;
using QueefCord.Core.Helpers;
using QueefCord.Content.Graphics;
using QueefCord.Content.Entities;
using QueefCord.Core.Input;

public class CrepsularRays : Entity, IUpdate
{
    public List<IDraw> OcclusionMaps = new List<IDraw>();
    public Vector2 sunPos = Vector2.Zero;

    private float SunMovement => 0.0005f;
    public float Density => .5f;
    public float Decay => .9f;
    public float Weight => 1.0f;
    public float Exposure => .3f;
    public float BloomThreshold => 0.04f;

    public void Update(GameTime gameTime)
    {
        if (GameInput.Instance["Right"].IsDown())
            sunPos.X += SunMovement;
        if (GameInput.Instance["Left"].IsDown())
            sunPos.X -= SunMovement;
        if (GameInput.Instance["Down"].IsDown())
            sunPos.Y += SunMovement;
        if (GameInput.Instance["Up"].IsDown())
            sunPos.Y -= SunMovement;

        CrepsularRaysMap rayMap = (LayerHost.GetLayer("Default").MapHost.Maps.Get("CrepsularRaysMap") as CrepsularRaysMap);
        CrepsularSource sourceMap = (LayerHost.GetLayer("Default").MapHost.Maps.Get("CrepsularSource") as CrepsularSource);

        sourceMap.LightScreenSourcePos = sunPos;

        rayMap.LightScreenSourcePos = sunPos;
        rayMap.Density = Density;
        rayMap.Decay = Decay;
        rayMap.Weight = Weight;
        rayMap.Exposure = Exposure;
        rayMap.BloomThreshold = BloomThreshold;

        foreach (IDraw map in  OcclusionMaps)
        {
            LayerHost.GetLayer("Default").MapHost.Maps.DrawToBatchedMap("CrepsularOcclusionMap",
            (SpriteBatch sb) =>
            {
                map.Draw(sb);
            });
        }
        
    }
}