void ToonFunc_float(in float3 normal, in float ToonRampSmoothness, in float3 clipSpacePosition, in float3 worldPosition, in float4 toonRampTinting,
in float toonRampOffset, out float3 toonRampOutput, out float3 direction)
{
    //Come si deve comportare la shader se ci troviano nella preview
    #ifdef SHADERGRAPH_PREVIEW
        toonRampOutput = float3(0.5,0.5,0);
        direction = float3(0.5,0.5,0);
    #else
    //In una scena vera e propria
        #if SHADOW_SCREEN
            half4 shadowCoord = ComputeScreenPos(clipSpacePosition);
        #else
            half4 shadowCoord = TransformWorldToShadowCoord(worldPosition);
        #endif

        #if _MAIN_LIGHT_SHADOW_CASCADE || _MAIN_LIGHT_SHADOWS
            Light light = GetMainLight(shadowCoord);
        #else
            Light light = GetMainLight();
        #endif

        half4 d = dot(normal, light.direction) * 0.5 + 0.5;

        half toonRamp = smoothstep(toonRampOffset, toonRampOffset + ToonRampSmoothness, d);

        toonRamp *= light.shadowAttenuation;

        toonRampOutput = light.color*(toonRamp + toonRampTinting);

        direction = light.direction;
    #endif
}