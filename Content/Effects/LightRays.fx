#include "PPVertexShader.fxh"

#define NUM_SAMPLES 128

sampler s0 : register(s0);

float2 lightScreenPosition;
float BloomThreshold = 0.02f;

float2 halfPixel;

float Density = .5f;
float Decay = .95f;
float Weight = 1.0f;
float Exposure = 5;
float2 screenScale;

texture scene;
sampler2D Scene
{
    Texture = (scene);
    AddressU = Clamp;
    AddressV = Clamp;
};

texture orgSceneFore;
sampler2D orgSceneForeS
{
    Texture = (orgSceneFore);
    AddressU = Clamp;
    AddressV = Clamp;
};


float4 lightRayPS(float2 texCoord : TEXCOORD0) : COLOR0
{
	// Find light pixel position
	
    float2 TexCoord = texCoord - halfPixel;

    float2 DeltaTexCoord = (TexCoord - lightScreenPosition);
    DeltaTexCoord *= (1.0f / NUM_SAMPLES * Density);

    float3 col = tex2D(s0, TexCoord / screenScale);
    float IlluminationDecay = 1.0;
    float3 Sample;
	
    for (int i = 0; i < NUM_SAMPLES; ++i)
    {
        TexCoord -= DeltaTexCoord;
        Sample = tex2D(s0, TexCoord / screenScale);
        Sample *= IlluminationDecay * Weight;
        col += Sample;
        IlluminationDecay *= Decay;
    }
    col = saturate((col - BloomThreshold) / (1 - BloomThreshold));
    float4 c = tex2D(orgSceneForeS, texCoord) + float4(col * Exposure, 1);
    
    return c;
	
}

technique LightRayFX
{
    pass p0
    {
        PixelShader = compile ps_3_0 lightRayPS();
    }
}