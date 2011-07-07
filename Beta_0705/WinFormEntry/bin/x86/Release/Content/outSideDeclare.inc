//#define FLIP_TEXTURE_Y


// #define OBJECT_SPACE_LIGHTS /* Define if LIGHT_COORDS is "Object" */
struct Light 
{
    float4 color;
    float4 position;
    float falloff;
    float range;
};
Light lights[32];


int numLightsPerPass = 32;
// transform object vertices to world-space:
float4x4 WorldXf : World < string UIWidget="None"; >;
// transform object normals, tangents, & binormals to world-space:
float4x4 WorldITXf : WorldInverseTranspose < string UIWidget="None"; >;
// transform object vertices to view space and project them in perspective:
float4x4 WvpXf : WorldViewProjection < string UIWidget="None"; >;
// provide tranform from "view" or "eye" coords back to world-space:
float4x4 ViewIXf : ViewInverse < string UIWidget="None"; >;

//// TWEAKABLE PARAMETERS ////////////////////

// "DirPos" Lamp 0 /////////
float4 gLamp0DirPos : POSITION < // or direction, if W==0
    string Object = "Light0";
    string UIName =  "Lamp 0 Position/Direction";
    string Space = ("World");
> = {-512.0f,219.0f,132.25f,0.0};
float3 gLamp0Color : COLOR <
    string UIName =  "Lamp 0";
    string Object = "Light0";
    string UIWidget = "Color";
> = {1.0f,1.0f,1.0f};


//toggle diffuse
float gDiff<
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 1.0;
    string UIName =  "Diff";
> = 1.0;

// Ambient Light
float3 gAmbiColor : AMBIENT <
    string UIName =  "Ambient Color";
    string UIWidget = "Color";
> = {1.0f,1.0f,1.0f};

//amb multiplyer
float gAm <
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.05;
    string UIName =  "Ambient scale ";
> = 0.4;





//tile for repeat
float tile <
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 10.0;
    float UIStep = 1;
    string UIName =  "tiles";
> = 3;


float gKs <
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.05;
    string UIName =  "Specular";
> = 0.4;

float gEccentricity <
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.0001;
    string UIName =  "Highlight Eccentricity";
> = 0.3;



float gBump <
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 3.0;
    float UIStep = 0.01;
    string UIName =  "Bumpiness";
> = 1.0; 

float gKr <
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.01;
    string UIName =  "Reflection Strength";
> = 0.5;



float gOccScale<
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 5.0;
    float UIStep = 0.01;
    string UIName =  "occ multiply";
> = 1.0; 






//////// COLOR & TEXTURE /////////////////////

texture gColorTexture : DIFFUSE <
    string ResourceName = "default_color.dds";
    string UIName =  "Diffuse Texture";
    string ResourceType = "2D";
>;

sampler2D gColorSampler = sampler_state {
    Texture = <gColorTexture>;

    MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;

    AddressU = Wrap;
    AddressV = Wrap;
}; 

texture gAlpha<
    string ResourceName = "default_color.dds";
    string UIName =  "Alpha Texture";
    string ResourceType = "2D";
>;;
sampler gAlphaSampler = sampler_state 
{
    texture = <gAlpha>;
    
	MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;

    AddressU = Wrap;
    AddressV = Wrap;
};



//ambient tex

texture gAmbTex <
    string ResourceName = "default_color.dds";
    string UIName =  "Amb Texture";
    string ResourceType = "2D";
>;

sampler2D gAmbTexSampler = sampler_state {
    Texture = <gAmbTex>;
    MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
}; 



//occ tex
texture gOccTex <
    string ResourceName = "default_color.dds";
    string UIName =  "Occ Texture";
    string ResourceType = "2D";
>;

sampler2D gOccTexSampler = sampler_state {
    Texture = <gOccTex>;
    MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
}; 


texture gNormalTexture  <
    string ResourceName = "default_bump_normal.dds";
    string UIName =  "Normal-Map Texture";
    string ResourceType = "2D";
>;

sampler2D gNormalSampler = sampler_state {
    Texture = <gNormalTexture>;
    MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
}; 

texture gEnvTexture : ENVIRONMENT <
    string ResourceName = "default_reflection.dds";
    string UIName =  "Environment";
    string ResourceType = "Cube";
>;

samplerCUBE gEnvSampler = sampler_state {
    Texture = <gEnvTexture>;
    MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
    AddressW = Clamp;
};



/* data from application vertex buffer */
struct appdata {
    float3 Position	: POSITION;
    float4 UV		: TEXCOORD0;
    float4 Normal	: NORMAL;
    float4 Tangent	: TANGENT0;
    float4 Binormal	: BINORMAL0;
};

/* data passed from vertex shader to pixel shader */
struct vertexOutput {
    float4 HPosition	: POSITION;
    float2 UV		: TEXCOORD0;
	float2 UVRepeat : TEXCOORD1;
    // The following values are passed in "World" coordinates since
    //   it tends to be the most flexible and easy for handling
    //   reflections, sky lighting, and other "global" effects.
    float3 LightVec	: TEXCOORD2;
    float3 WorldNormal	: TEXCOORD3;
    float3 WorldTangent	:TEXCOORD4 ;
    float3 WorldBinormal : TEXCOORD5;
    float3 WorldView	: TEXCOORD6;
	float3 pos:TEXCOORD7;
};