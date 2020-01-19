// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SkillSlotShader" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_CD ("Cooldown (Normalized)", Float) = 0.00
	}
	SubShader 
	{
		Tags { "Queue" = "Transparent-256" "IgnoreProjector" = "True" "RenderType"="Transparent" }
		LOD 100
	
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Fog { Mode Off }
	
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			#include "UnityCG.cginc"
			
			struct vs_input 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			v2f vert(vs_input input)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(input.vertex);
				o.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
	
				return o;
			}

			float _CD;
			
			half4 frag(v2f i) : COLOR
			{
				float uOffset = i.uv.x - 0.5f;
				float vOffset = -(i.uv.y - 0.5f);
				
				float theta = atan2(uOffset, vOffset);
				
				half4 c = tex2D(_MainTex, i.uv);
				
				half4 fc = half4(c.rgb, c.a);
				
				half grey = pow(dot(c.rgb, half3(0.21, 0.72, 0.07)), 1.62);
				half4 gc = half4(grey, grey, grey, c.a);

				half4 final = theta + 3.1415926 - _CD * 6.2831852> 0 ? fc : gc;
				return 1.0 * final;
			}

			ENDCG
		}
	}
}
