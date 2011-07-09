
 #include "outSideDeclare.inc"
///////// VERTEX SHADING /////////////////////

/*********** Generic Vertex Shader ******/

vertexOutput std_dp_VS(appdata IN) 
{
    vertexOutput OUT = (vertexOutput)0;
    OUT.WorldNormal = mul(IN.Normal,WorldITXf).xyz;
    OUT.WorldTangent = mul(IN.Tangent,WorldITXf).xyz;
    OUT.WorldBinormal = mul(IN.Binormal,WorldITXf).xyz;
    float4 Po = float4(IN.Position.xyz,1);
    float4 Pw = mul(Po,WorldXf);	// convert to "world" space

    float4 Lw = mul(gLamp0DirPos,WorldXf);	// convert to "world" space

	/*
    if (Lw.w == 0) {
	OUT.LightVec = -normalize(Lw.xyz);
    } 
	else 
	{
	// we are still passing a (non-normalized) vector
	//OUT.LightVec = Lw.xyz - Pw.xyz;
	OUT.LightVec = normalize(Lw.xyz);
    }*/
	OUT.LightVec = normalize(Lw.xyz);
	
	
	
#ifdef FLIP_TEXTURE_Y
    OUT.UV = float2(IN.UV.x,(1.0-IN.UV.y));
	OUT.UVRepeat=tile*OUT.UV ;
#else /* !FLIP_TEXTURE_Y */
    OUT.UV = IN.UV.xy;
	OUT.UVRepeat=tile*OUT.UV ;
#endif /* !FLIP_TEXTURE_Y */


    OUT.WorldView = normalize(ViewIXf[3].xyz - Pw.xyz);
    OUT.HPosition = mul(Po,WvpXf);
	OUT.pos=OUT.HPosition.xyz;
    return OUT;
}

///////// PIXEL SHADING //////////////////////
float getAlpha(vertexOutput IN)
{
	clip(tex2D(gAlphaSampler, IN.UV).r - 0.6f);
	float result=tex2D(gAlphaSampler, IN.UV).r;
	
	return result;
}
float4 blinn_PS(vertexOutput IN) : COLOR {
    float3 Ln = normalize(IN.LightVec);
    float3 Vn = normalize(IN.WorldView);
    float3 Nn = normalize(IN.WorldNormal);
    float3 Tn = normalize(IN.WorldTangent);
    float3 Bn = normalize(IN.WorldBinormal);
    float3 bump = gBump * (tex2D(gNormalSampler,IN.UVRepeat).rgb - float3(0.5,0.5,0.5));
    
    Nn = Nn + bump.x*Tn + bump.y*Bn;
    Nn = normalize(Nn);
    float3 Hn = normalize(Vn + Ln);
    float hdn = dot(Hn,Nn);
    float3 Rv = reflect(-Ln,Nn);
    float rdv = dot(Rv,Vn);
    rdv = max(rdv,0.001);
    float ldn=dot(Ln,Nn);
    ldn = max(ldn,0.0);

    float ndv = dot(Nn,Vn);
    float hdv = dot(Hn,Vn);
	
    float eSq = gEccentricity*gEccentricity;
    float distrib = eSq / (rdv * rdv * (eSq - 1.0) + 1.0);
    distrib = distrib * distrib;
    
    float Gb = 2.0 * hdn * ndv / hdv;
    float Gc = 2.0 * hdn * ldn / hdv;
    float Ga = min(1.0,min(Gb,Gc));
    float fresnelHack = 1.0 - pow(ndv,5.0);
    
    hdn = distrib * Ga * fresnelHack / ndv;
	
	
	
    float3 diffContrib = ldn * gLamp0Color;
    float3 specContrib = hdn * gKs * gLamp0Color*diffContrib;
	
	float3 diffuseColor = tex2D(gColorSampler,IN.UVRepeat).rgb;
		
    gAmbiColor*=tex2D(gAmbTexSampler,IN.UV).rgb*gAm;
	
	
	float3 R = reflect(Vn,Nn);
    float3 reflColor = gKr * texCUBE(gEnvSampler,R.xyz).rgb;
    	
	float3 result=specContrib+reflColor;
	
	result+=gDiff*diffuseColor*diffContrib+diffuseColor*gAmbiColor;
    result*=pow(tex2D(gOccTexSampler,IN.UV).rgb,gOccScale);
	
	float alpha=getAlpha(IN);
    return float4(result,alpha);
}



float4 AmbientPS(vertexOutput IN) : COLOR
{
	float3 diffuseColor = tex2D(gColorSampler,IN.UVRepeat).rgb;
	gAmbiColor*=tex2D(gAmbTexSampler,IN.UV).rgb*gAm;
	
	float4 result= float4(0,0,0,1);
	result.rgb=diffuseColor*gAmbiColor;
	result.a =getAlpha(IN);
	return result;
}



float4 CalculateSingleLight(Light light,vertexOutput IN)
{
		
		IN.LightVec= light.position - IN.pos;
     	float lightDist = length(IN.LightVec);
     	
     	//calculate the intensity of the light with exponential falloff
     	float baseIntensity =pow(saturate((light.range - lightDist) / light.range),light.falloff);
	
	
    float3 Ln = normalize(IN.LightVec);
    float3 Vn = normalize(IN.WorldView);
    float3 Nn = normalize(IN.WorldNormal);
    float3 Tn = normalize(IN.WorldTangent);
    float3 Bn = normalize(IN.WorldBinormal);
    float3 bump = gBump * (tex2D(gNormalSampler,IN.UVRepeat).rgb - float3(0.5,0.5,0.5));
    
    Nn = Nn + bump.x*Tn + bump.y*Bn;
    Nn = normalize(Nn);
    float3 Hn = normalize(Vn + Ln);
    float hdn = dot(Hn,Nn);
    float3 Rv = reflect(-Ln,Nn);
    float rdv = dot(Rv,Vn);
    rdv = max(rdv,0.001);
    float ldn=dot(Ln,Nn);
    ldn = max(ldn,0.0);

    float ndv = dot(Nn,Vn);
    float hdv = dot(Hn,Vn);
	
    float eSq = gEccentricity*gEccentricity;
    float distrib = eSq / (rdv * rdv * (eSq - 1.0) + 1.0);
    distrib = distrib * distrib;
    
    float Gb = 2.0 * hdn * ndv / hdv;
    float Gc = 2.0 * hdn * ldn / hdv;
    float Ga = min(1.0,min(Gb,Gc));
    float fresnelHack = 1.0 - pow(ndv,5.0);
    
    hdn = distrib * Ga * fresnelHack / ndv;
	
	
    float3 diffContrib = ldn * gLamp0Color;
    float3 specContrib = hdn * gKs * gLamp0Color*diffContrib;
	float3 diffuseColor = tex2D(gColorSampler,IN.UVRepeat).rgb;
		
    gAmbiColor*=tex2D(gAmbTexSampler,IN.UV).rgb*gAm;
	
	
	float3 R = reflect(Vn,Nn);
    float3 reflColor = gKr * texCUBE(gEnvSampler,R.xyz).rgb;
    float3 result=float3(0.0f,0.0f,0.0f);
	result=specContrib*light.color ;//+ reflColor*baseIntensity;
	result*=baseIntensity;
	
	result+=gDiff*diffuseColor*diffContrib*light.color;
	result*=baseIntensity;
	return float4(result,1);
}


float4 MultipleLightPS(vertexOutput IN) : COLOR
{
	float4 color = float4(0,0,0,0);
	for(int i=0; i< numLightsPerPass; i++)
    {
       color += CalculateSingleLight(lights[i], IN);
    }
    color.a =getAlpha(IN);
    return color;

}
float4 blackPS(vertexOutput IN): COLOR
{
	float4 color=float4(0,0,0,1);
	return color;
	
	
}

///////////////////////////////////////
/// TECHNIQUES ////////////////////////
///////////////////////////////////////

technique Main
{
    pass singlePass 
	{
        VertexShader = compile vs_3_0 std_dp_VS();
		
        PixelShader = compile ps_3_0 blinn_PS();
    }
	
}


technique MultiLights
{
	
	pass ambientPass
	{
		
		VertexShader = compile vs_3_0 std_dp_VS();
		
        PixelShader = compile ps_3_0 AmbientPS();
		
		
	}
	
	 pass singlePass 
	{
        VertexShader = compile vs_3_0 std_dp_VS();
		
        PixelShader = compile ps_3_0 blinn_PS();
    }
	
	
	pass lightPass
	{
		
		VertexShader = compile vs_3_0 std_dp_VS();
		
        PixelShader = compile ps_3_0 MultipleLightPS();//blackPS();//MultipleLightPS();
		
	}
	
}
