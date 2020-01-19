// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

Shader "LightmappingEffects/BlendDiffuse" {

    Properties {

 	    _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex("Main Texture", 2D) = "white" {}
        _BlendTex("Blended Texture", 2D) = "white" {}
		_BlendTex2("Second Blended Texture", 2D) = "white" {}
		_Adjust("Blend Range", Range (0, 1)) = 0
    }

SubShader {

    Pass {

        CGPROGRAM

        #pragma vertex vert

        #pragma fragment frag

        #include "UnityCG.cginc"

        

        sampler2D _MainTex;
        sampler2D _BlendTex;
        sampler2D _BlendTex2;
         
		fixed4 _Color;
		
        float4 _MainTex_ST; //scale & position of _MainTex
		float	_Adjust;
        

        // sampler2D unity_Lightmap;//Beast lightmap
		// sampler2D unity_LightmapInd;//Beast lightmap
        // float4 unity_LightmapST; //scale & position of Beast lightmap

        

        

        // vertex input: position, UV0, UV1

        struct appdata {

            float4 vertex   : POSITION;

            float2 texcoord : TEXCOORD0;

            float2 texcoord1: TEXCOORD1; 

        };

        

        struct v2f {

            float4 pos  : SV_POSITION;

            float2 txuv : TEXCOORD0;

            float2 lmuv : TEXCOORD1;

        };

        

        v2f vert (appdata v) {

            v2f o;

            o.pos   = UnityObjectToClipPos( v.vertex );

            o.txuv  = TRANSFORM_TEX(v.texcoord.xy,_MainTex);

            o.lmuv  = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

            return o;

        }

        

        half4 frag( v2f i ) : COLOR {

            half4 col   = tex2D(_MainTex, i.txuv.xy);
            half4 blendCol = tex2D(_BlendTex,i.lmuv.xy);
            half4 lm    = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmuv.xy);
			half4 lm2   =  UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd,unity_Lightmap, i.lmuv.xy);
			
            col.rgb     = col.rgb * lerp(DecodeLightmap(lm),DecodeLightmap(blendCol),_Adjust) * _Color; 

            return col;

        }

        ENDCG

        }

    }
    
    fallback "Diffuse"

}