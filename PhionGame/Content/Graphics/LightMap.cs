using QueefCord.Core.Graphics;
using QueefCord.Core.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace QueefCord.Content.Graphics
{
    public class LightMap : MapPass 
    {
        protected override string MapEffectName => "Effects/Lighting";
        public override float Priority => 1;

        internal override void OnApplyShader()
        {
            MapEffect?.Parameters["lineOfSightMap"].SetValue(LayerParent.MapHost.Maps.Get("LineOfSightMap").MapTarget);
            MapEffect?.CurrentTechnique.Passes[0].Apply();
        }
    }
    public class LineOfSightMap : MapPass { public override float Priority => 0; }

}