﻿Shader "Custom/MultiDecal" {
	Properties {
		_BaseTint ("Base Tint", Color) = (1,1,1,1)
		_Base ("Skin (RGB)", 2D) = "white" {}
		_EyesTint ("Eyes Tint", Color) = (1,1,1,1)
		_Eyes ("Eyes (RGB)", 2D) = "white" {}
		_PantsTint ("Pants Tint", Color) = (1,1,1,1)
		_Pants ("Pants (RGB)", 2D) = "white" {}
		_ChestTint ("Chest Tint", Color) = (1,1,1,1)
		_Chest ("Chest (RGB)", 2D) = "white" {}
		_HandsTint ("Hands Tint", Color) = (1,1,1,1)
		_Hands ("Hands (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Base;
		sampler2D _Eyes;
		sampler2D _Pants;
		sampler2D _Chest;
		sampler2D _Hands;

		struct Input {
			float2 uv_Base;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _BaseTint;
		fixed4 _EyesTint;
		fixed4 _PantsTint;
		fixed4 _ChestTint;
		fixed4 _HandsTint;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 ca = tex2D (_Base, IN.uv_Base) * _BaseTint;
			fixed4 cb = tex2D (_Eyes, IN.uv_Base) * _EyesTint;
			fixed4 cc = tex2D (_Pants, IN.uv_Base) * _PantsTint;
			fixed4 cd = tex2D (_Chest, IN.uv_Base) * _ChestTint;
			fixed4 ce = tex2D (_Hands, IN.uv_Base) * _HandsTint;
			
			// First layer
			o.Albedo = cb.rgb * cb.a + ca.rgb * ca.a * (1 - cb.a);
			o.Alpha = cb.a + ca.a * (1 - cb.a);
			
			// Second layer
			o.Albedo = cc.rgb * cc.a + o.Albedo * o.Alpha * (1 - cc.a);
			o.Alpha = cc.a + o.Alpha * (1 - cc.a);
			
			// Third layer
			o.Albedo = cd.rgb * cd.a + o.Albedo * o.Alpha * (1 - cd.a);
			o.Alpha = cd.a + o.Alpha * (1 - cd.a);
			
			// Fourth layer
			o.Albedo = ce.rgb * ce.a + o.Albedo * o.Alpha * (1 - ce.a);
			o.Alpha = ce.a + o.Alpha * (1 - ce.a);
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
