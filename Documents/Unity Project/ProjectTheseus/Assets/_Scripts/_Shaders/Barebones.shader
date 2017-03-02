Shader "Unlit/Barebones"
{
	Properties
	{
		_Colour("Colour", Color) = (1,1,1,1)
	}
	SubShader
	{
		Cull off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform fixed4 _Colour;

			struct vertexInput
			{
				float4 vertex : POSITION;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
			};
			
			vertexOutput vert (vertexInput v)
			{
				vertexOutput c;
				c.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return c;
			}
			
			fixed4 frag (vertexOutput i) : COLOR
			{
				return _Colour;
			}
			ENDCG
		}
	}
}
