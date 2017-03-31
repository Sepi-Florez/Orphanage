Shader "Unlit/Colour"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EmissionTex ("Emission", 2D) = "white" {}
		_EmissionCol ("Emission Colour",Color) = (1,1,1,1)
		_EmissionMult ("Emission Multiplier", Range(1,5)) = 1.0
	}
	SubShader
	{
		Pass
		{
			Tags{"Lightmode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform fixed4 _EmissionCol;
			uniform float _EmissionMult;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos =  mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = normalize(mul(v.normal, unity_WorldToObject).xyz);

				return o;
			}

			sampler2D _EmissionTex;

			fixed4 _LightColor0;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float dif = max(0.0, dot(i.normal, normalize(_WorldSpaceLightPos0.xyz)));
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 emi = tex2D(_EmissionTex, i.uv) * _EmissionCol * _EmissionMult;
				fixed4 c = fixed4(col.rgb * dif * _LightColor0.rgb + emi, 1);
				return fixed4(c);
				//return fixed4(col.rgb * dif * _LightColor0.rgb, 1);
			}
			ENDCG
		}
	}
}