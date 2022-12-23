#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0 : register(s0);

float2 screenScale;

texture LightTexture;
sampler LightSampler = sampler_state
{
    Texture = (LightTexture);
};

texture TileTexture;
sampler TileSampler = sampler_state
{
    Texture = (TileTexture);
};

texture Map;
sampler MapSampler = sampler_state
{
    Texture = (Map);
};

float4 PixelShaderLight(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);

    float4 tile = tex2D(TileSampler, coords);
    float4 map = tex2D(MapSampler, coords);
    float4 light = tex2D(LightSampler, coords / screenScale);

    float4 tileLight = float4((map * tile).rgb * light.r, tile.a * map.a);
    return color * (1 - tileLight.a) + tileLight;
}
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderLight();
    }
}