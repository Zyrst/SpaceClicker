Shader "Custom/PlanetGlow" {
    Properties
     {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _Color ("Tint", Color) = (1,1,1,1)
         
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
 
         _ColorMask ("Color Mask", Float) = 15

		 _Noise ("Noise", Range(0.0,1.0) ) = 0
		 _Noise2 ("Noise2", Range(0.0,1.0) ) = 0
     }
 
     SubShader
     {
         Tags
         { 
             "Queue"="Overlay" 
             "IgnoreProjector"="True" 
             "RenderType"="Transparent" 
             "PreviewType"="Plane"
             "CanUseSpriteAtlas"="True"
         }
         
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
 
         Cull Off
         Lighting Off
         ZWrite Off
         ZTest Off
         Blend SrcAlpha OneMinusSrcAlpha
         ColorMask [_ColorMask]
 
         Pass
         {
         CGPROGRAM
             #pragma vertex vert alpha
             #pragma fragment frag
             #include "UnityCG.cginc"
             
             struct appdata_t
             {
                 float4 vertex   : POSITION;
                 float4 color    : COLOR;
                 float2 texcoord : TEXCOORD0;
             };
 
             struct v2f
             {
                 float4 vertex   : SV_POSITION;
                 fixed4 color    : COLOR;
                 half2 texcoord  : TEXCOORD0;
				 float noise	 : NOISE;
				 float noise2	 : NOISE2;
             };
             
             fixed4 _Color;
			 float _Noise;
			 float _Noise2;
 
             v2f vert(appdata_t IN)
             {
                 v2f OUT;
                 OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                 OUT.texcoord = IN.texcoord;

 #ifdef UNITY_HALF_TEXEL_OFFSET
                 OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
 #endif
                 OUT.color = IN.color * _Color;
				 OUT.noise = _Noise;
				 OUT.noise2 = _Noise2;
                 return OUT;
             }
 
             sampler2D _MainTex;

			 float rand(half2 co){
				return frac(sin(dot(co.xy ,half2(12.9898,78.233))) * 43758.5453);
			}	
 
             fixed4 frag(v2f IN) : SV_Target
             {
				half4 color = IN.color;

				half2 xy = IN.texcoord.xy;

				half co = length(xy - (0.5));

								// ↓↓↓↓ inga hopp här inte 
				co = (co * co) * step(co, 0.5);
				
				color.a = (1 - co - 0.8) * 2 * step(0.0000001, co);

				color.a *= abs(fmod(IN.noise2, 2) - 1) * 2;

				return color;
             }
         ENDCG
	}
	}
}