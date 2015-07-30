Shader "Custom/MultiDecal" {
	Properties {
		_BaseTint ("Base Tint", Color) = (1,1,1,1)
		_Base ("Base (RGB)", 2D) = "white" {}
		_Layer1Tint ("Decal 1 Tint", Color) = (1,1,1,1)
		_Layer1 ("Decal 1 (RGB)", 2D) = "white" {}
		_Layer2Tint ("Decal 2 Tint", Color) = (1,1,1,1)
		_Layer2 ("Decal 2 (RGB)", 2D) = "white" {}
		_Layer3Tint ("Decal 3 Tint", Color) = (1,1,1,1)
		_Layer3 ("Decal 3 (RGB)", 2D) = "white" {}
		_Layer4Tint ("Decal 4 Tint", Color) = (1,1,1,1)
		_Layer4 ("Decal 4 (RGB)", 2D) = "white" {}
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
		sampler2D _Layer1;
		sampler2D _Layer2;
		sampler2D _Layer3;
		sampler2D _Layer4;

		struct Input {
			float2 uv_Base;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _BaseTint;
		fixed4 _Layer1Tint;
		fixed4 _Layer2Tint;
		fixed4 _Layer3Tint;
		fixed4 _Layer4Tint;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 ca = tex2D (_Base, IN.uv_Base) * _BaseTint;
			fixed4 cb = tex2D (_Layer1, IN.uv_Base) * _Layer1Tint;
			fixed4 cc = tex2D (_Layer2, IN.uv_Base) * _Layer2Tint;
			fixed4 cd = tex2D (_Layer3, IN.uv_Base) * _Layer3Tint;
			fixed4 ce = tex2D (_Layer4, IN.uv_Base) * _Layer4Tint;
			
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
