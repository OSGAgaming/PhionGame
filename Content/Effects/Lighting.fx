#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0 : register(s0);
float4 colorMult;

texture lineOfSightMap;
sampler lineOfSight = sampler_state
{
    Texture = (lineOfSightMap);
};

texture Map;
sampler MapSampler = sampler_state
{
    Texture = (Map);
};

float4 PixelShaderLight(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);
    float4 los = tex2D(lineOfSight, coords);
    float4 map = tex2D(MapSampler, coords);

    return color + map * los;
}
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderLight();
    }
}