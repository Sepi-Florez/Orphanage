// Upgrade NOTE: replaced 'PositionFog()' with transforming position into clip space.
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "Half Lambert" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _WrapAmount ("Wrap Amount", Range (1.0, 0.5)) = 0.5
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
 
Category {
    Tags { "RenderType"="Opaque" }
    LOD 200
    /* Upgrade NOTE: commented out, possibly part of old style per-pixel lighting: Blend AppSrcAdd AppDstAdd */
    Fog { Color [_AddFog] }
   
    #warning Upgrade NOTE: SubShader commented out; uses Unity 2.x per-pixel lighting. You should rewrite shader into a Surface Shader.
/*SubShader {
        // Ambient pass
        Pass {
            Name "BASE"
            Tags {"LightMode" = "Always"}
            Color [_PPLAmbient]
            SetTexture [_MainTex] {
                constantColor [_Color] Combine texture * primary DOUBLE, texture * constant
            }
        }
        // Pixel lights
        Pass {
            Name "PPL"
            Tags { "LightMode" = "Pixel" }
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv,normal,lightDir)
#pragma exclude_renderers d3d11
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_builtin
                #pragma fragmentoption ARB_fog_exp2
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
               
                struct v2f {
                    float4 pos : SV_POSITION;
                    LIGHTING_COORDS
                    float2  uv;
                    float3  normal;
                    float3  lightDir;
                };
               
                uniform float4 _MainTex_ST;
               
                v2f vert (appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos (v.vertex);
                    o.normal = v.normal;
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.lightDir = ObjSpaceLightDir( v.vertex );
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }
               
                uniform sampler2D _MainTex;
                uniform float _WrapAmount;
               
                // Wrapped Lambertian (diffuse) lighting model
                inline half4 WrappedLight( half3 lightDir, half3 normal, half4 color, half atten, half wrapAmount)
                {
                    #ifndef USING_DIRECTIONAL_LIGHT
                    lightDir = normalize(lightDir);
                    #endif
                   
                    half diffuse = dot( normal, lightDir )*wrapAmount + (1 - wrapAmount);
                   
                    half4 c;
                    c.rgb = color.rgb * _ModelLightColor0.rgb * (diffuse * atten * 2);
                    c.a = 0; // diffuse passes by default don't contribute to overbright
                    return c;
                }
               
                float4 frag (v2f i) : COLOR
                {
                    float3 normal = i.normal;
                   
                    half4 texcol = tex2D( _MainTex, i.uv );
                   
                    return WrappedLight( i.lightDir, normal, texcol, LIGHT_ATTENUATION(i), _WrapAmount);
                }
            ENDCG
        }
    }*/
}
 
Fallback "VertexLit"
 
}