Shader "ToonShader"
{
	Properties
	{
		[NoScaleOffset]_Color("Color", Color) = (1,1,1,1)
		[NoScaleOffset]_MainTex("Albedo", 2D) = "white" {}

		_Detail("Detail", 2D) = "gray" {}

		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		_Parallax("Height", 2D) = "black" {}

		[NoScaleOffset]_Metallic("Metallic", 2D) = "white"{}
		[NoScaleOffset]_Glossiness("Roughness", 2D) = "gray" {}

		[NoScaleOffset]_EmissionColor("EmissionColor", Color) = (0,0,0)
		[NoScaleOffset]_Emmision("Emission", 2D) = "black" {}

		[NoScaleOffset]_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		[NoScaleOffset]_RimPower("Rim Power", Range(0.5,8.0)) = 3.0

		_Ramp("Toon Ramp", 2D) = "gray" {}
	}
	SubShader
	{
	Tags{ "RenderType" = "Opaque" }
	LOD 200

	CGPROGRAM
		#pragma surface surf ToonRamp fullforwardshadows
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0

		float4 _RimColor;

		sampler2D _MainTex;
		sampler2D _Detail;
		sampler2D _Normal;
		sampler2D _Parallax;
		sampler2D _Metallic;
		sampler2D _Glossiness;
		sampler2D _Emission;
		sampler2D _Ramp;

		#pragma lighting ToonRamp //exclude_path:prepass
		inline half4 LightingToonRamp(SurfaceOutputStandard s, half3 lightDir, half atten)
		{
#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
#endif
			half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
			half3 ramp = tex2D(_Ramp, float2(d, d)).rgb;

			half4 c;
			c.rgb = s.Albedo * (_LightColor0.rgb) * ramp * (atten * 2);
			c.a = 0;
			return c;
		}

		fixed4 _EmissionColor;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_Detail;
			float2 uv_Normal;
			float2 uv_Parallax;
			float2 uv_Metallic;
			float2 uv_Emission;
			float3 viewDir;
		};
		float _RimPower;

		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutputStandard  o)
		{
			float2 offset = ParallaxOffset(tex2D(_Parallax, IN.uv_Parallax).r, 0.1, IN.viewDir);

			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Albedo *= tex2D(_Detail, IN.uv_Detail).rgb;

			//o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));

			o.Metallic = tex2D(_Metallic, IN.uv_Metallic).rgb;
			o.Smoothness = tex2D(_Glossiness, IN.uv_Metallic).rgb;

			fixed4 e = tex2D(_Emission, IN.uv_Emission) * _EmissionColor;
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = e.rgb + (_RimColor.rgb * pow(rim, _RimPower));

			o.Alpha = c.a;
		}
	ENDCG
	}
		FallBack "Diffuse"
}