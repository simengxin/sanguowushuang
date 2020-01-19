// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FogOfWar" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "black" {}
}

CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform sampler2D _CameraDepthTexture;
	uniform sampler2D _FogTex;
	
	uniform float4 _MainTex_TexelSize;

	uniform float4x4 _FrustumCornersWS;
	uniform float4 _CameraWS;
	uniform float4 _SceneSize;
	uniform float4 _FogColor;
	 
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
		float2 uv_depth : TEXCOORD1;
		float4 interpolatedRay : TEXCOORD2;
	};
	
	v2f vert( appdata_img v )
	{
		v2f o;
		half index = v.vertex.z;
		v.vertex.z = 0.1;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.uv_depth = v.texcoord.xy;
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1-o.uv.y;
		#endif				
		
		o.interpolatedRay = _FrustumCornersWS[(int)index];
		o.interpolatedRay.w = index;
		
		return o;
	}
	
	half4 frag (v2f i) : COLOR
	{
		float dpth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv_depth)));
		float4 wsPos = (_CameraWS + dpth * i.interpolatedRay);
		
		float u = 0.5f + (wsPos.x - _SceneSize.x) * _SceneSize.z;
		float v = 0.5f + (wsPos.z - _SceneSize.y) * _SceneSize.w;
		
		half4 c0 = tex2D(_MainTex, i.uv );
		float f = tex2D(_FogTex, float2(u, v)).w;
		
		f = f * (1-dpth) / (1-dpth);
		
		//half4 c1 = (1 - f) * _FogColor + f;// * c0;
		half4 c1 = (1 - f) * _FogColor + f * c0;
		c1.w = 1;
		
		return c1;
	}

ENDCG

SubShader {

	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#pragma exclude_renderers flash
		
		ENDCG
	}
}

Fallback off

}