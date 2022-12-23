#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float BloomThreshold;

float2 halfPixel;
sampler TextureSampler : register(s0);


float4 BrightPassPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    texCoord -= halfPixel;
    
    float4 c = tex2D(TextureSampler, texCoord);
    
    return saturate((c - BloomThreshold) / (1 - BloomThreshold));
}

technique BloomExtract
{
    pass P0
    {
        PixelShader = compile ps_3_0 BrightPassPS();
    }
}