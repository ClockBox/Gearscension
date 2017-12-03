// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Glow"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Float3("Float 3", Range( -0.6989688 , 1.5)) = -0.73
		_Float1("Float 1", Float) = 0
		_Float2("Float 2", Float) = 1.05
		_Bias("Bias", Range( -0.6989688 , 1.5)) = -0.73
		_Thickness("Thickness", Float) = 1.05
		_Gradiant("Gradiant", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Float0("Float 0", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color0;
		uniform float _Bias;
		uniform float _Thickness;
		uniform float _Gradiant;
		uniform float _Float0;
		uniform float _Float3;
		uniform float _Float2;
		uniform float _Float1;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( _Bias + _Thickness * pow( 1.0 - fresnelNDotV1, _Gradiant ) );
			float3 temp_cast_1 = (fresnelNode1).xxx;
			o.Emission = temp_cast_1;
			o.Alpha = _Float0;
			float fresnelNDotV8 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode8 = ( _Float3 + _Float2 * pow( 1.0 - fresnelNDotV8, _Float1 ) );
			clip( fresnelNode8 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
522;425;1373;1004;1318.968;-97.15098;1.163219;True;True
Node;AmplifyShaderEditor.RangedFloatNode;2;-872.1577,218.8158;Float;False;Property;_Bias;Bias;0;0;-0.73;-0.6989688;1.5;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;-826.762,311.6825;Float;False;Property;_Thickness;Thickness;0;0;1.05;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-708.723,712.8608;Float;False;Property;_Float1;Float 1;0;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-776.1187,541.9941;Float;False;Property;_Float3;Float 3;0;0;-0.73;-0.6989688;1.5;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-730.723,634.8608;Float;False;Property;_Float2;Float 2;0;0;1.05;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-804.762,389.6826;Float;False;Property;_Gradiant;Gradiant;0;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.FresnelNode;8;-340.6998,477.5234;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.FresnelNode;1;-518.7,224.1518;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;6;-420.9618,-55.23064;Float;False;Property;_Color0;Color 0;3;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;7;-436.0839,115.7626;Float;False;Property;_Float0;Float 0;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Glow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;False;Transparent;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;1;11;0
WireConnection;8;2;10;0
WireConnection;8;3;9;0
WireConnection;1;1;2;0
WireConnection;1;2;4;0
WireConnection;1;3;5;0
WireConnection;0;0;6;0
WireConnection;0;2;1;0
WireConnection;0;9;7;0
WireConnection;0;10;8;0
ASEEND*/
//CHKSM=FE195AFD6202E2A5EDA03656DD0327CEF3F7D484