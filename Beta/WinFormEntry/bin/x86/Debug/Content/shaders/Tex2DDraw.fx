uniform extern float4x4 WorldViewProj : WORLDVIEWPROJECTION;
uniform extern texture UserTexture;
uniform extern float4 Col; 
struct VS_OUTPUT
{
    float4 position  : POSITION;
    float4 textureCoordinate : TEXCOORD0;
};

sampler textureSampler = sampler_state
{
    Texture = <UserTexture>;
    mipfilter = LINEAR; 
};
 
VS_OUTPUT Transform(
    float4 Position  : POSITION, 
    float4 TextureCoordinate : TEXCOORD0 )
{
    VS_OUTPUT Out = (VS_OUTPUT)0;

    Out.position = mul(Position, WorldViewProj);
    Out.textureCoordinate = TextureCoordinate;

    return Out;
}

float4 ApplyTexture(VS_OUTPUT vsout) : COLOR
{
	clip(tex2D(textureSampler, vsout.textureCoordinate).a - 0.40f);

    return tex2D(textureSampler, vsout.textureCoordinate).rgba;
}
float4 ApplyColRed(VS_OUTPUT vsout) : COLOR
{

	float4  result;
	result=tex2D(textureSampler, vsout.textureCoordinate).rgba;
	result=Col;
    return result;
}

technique TransformAndTexture
{
    pass P0
    {
        vertexShader = compile vs_2_0 Transform();
        pixelShader  = compile ps_2_0 ApplyTexture();
    }
}
technique TransformAndColor
{
    pass P0
    {
        vertexShader = compile vs_2_0 Transform();
        pixelShader  = compile ps_2_0 ApplyColRed();
    }
}