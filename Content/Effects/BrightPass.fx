#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#include "PPVertexShader.fxh"
sampler s0 : register(s0);

float2 lightScreenPosition;

float2 screenRes = float2(4, 3);

float4x4 matVP;

float2 halfPixel;

float SunSize = 1500;
float2 screenScale;

texture scene;
sampler2D Scene
{
    Texture = (scene);
    AddressU = Clamp;
    AddressV = Clamp;
};

texture flare;
sampler Flare = sampler_state
{
    Texture = (flare);
    AddressU = CLAMP;
    AddressV = CLAMP;
};

float4 LightSourceMaskPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    texCoord -= halfPixel;

	// Get the scene
    float4 col = 0;
	
	// Find the suns position in the world and map it to the screen space.
    float2 coord;
		
    float size = SunSize / 1;
					
    float2 center = lightScreenPosition;

    coord = .5 - ((texCoord - center) * screenRes) / size * .5f;
		
    col += (pow(tex2D(Flare, coord), 2) * 1) * 2;
	
    return float4((col * (1 - tex2D(Scene, texCoord * screenScale).a)).rgb, 1);
}

technique LightSourceMask
{
    pass p0
    {
        PixelShader = compile ps_3_0 LightSourceMaskPS();
    }
}