Shader "Custom/basicShader" 
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		_Detail("Detail", 2D) = "gray" {}

		_BumpMap("Normal", 2D) = "bump" {}
			
		[Gamma] _Metallic("Metallic", 2D) ="gray"{}
		_Glossiness("Smoothness", 2D) = "gray" {}

		_Glossiness("Emission", 2D) = "gray" {}
	}
	SubShader
	{
	Tags { "RenderType" = "Opaque" }
	LOD 200

	CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Detail;
		sampler2D _Emission;
		float4 _RimColor;
		float _RimPower;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
			float2 uv_Detail;
		};

		sampler2D _Metallic;
		sampler2D _Glossiness;
		fixed4 _Color;

		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Albedo *= tex2D(_Detail, IN.uv_Detail).rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Metallic = tex2D(_Metallic, IN.uv_MainTex).rgb;
			o.Smoothness = tex2D(_Glossiness, IN.uv_MainTex).rgb;
			o.Emission = tex2D(_Glossiness, IN.uv_MainTex).rgb;
			o.Alpha = c.a;
			}
		ENDCG
	}
	FallBack "Diffuse"
}
