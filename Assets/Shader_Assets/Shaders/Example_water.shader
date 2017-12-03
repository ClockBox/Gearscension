// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Example_water"
{
	Properties
	{
		[Header(Refraction)]
		_Water("Water", Color) = (0.41609,0.7647059,0.5627491,0)
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_Base("Base", Color) = (0.3148789,0.3365946,0.7647059,0)
		_Bottem("Bottem", Color) = (0.41609,0.7647059,0.5627491,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Distance("Distance", Float) = 0
		_Transparent("Transparent", Float) = 0.8
		_Refeaction("Refeaction", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float eyeDepth;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample1;
		uniform float4 _Base;
		uniform float4 _Water;
		uniform float4 _Bottem;
		uniform float _Distance;
		uniform float _Transparent;
		uniform sampler2D _GrabTexture;
		uniform float _ChromaticAberration;
		uniform float _Refeaction;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
			float2 uv_TexCoord19 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float2 panner11 = ( uv_TexCoord19 + _Time.y * float2( 0.06,0.02 ));
			float2 panner10 = ( uv_TexCoord19 + 1.0 * _Time.y * float2( -0.085688,-0.06 ));
			float4 lerpResult17 = lerp( tex2Dlod( _TextureSample0, float4( panner11, 0, 0.0) ) , tex2Dlod( _TextureSample1, float4( panner10, 0, 0.0) ) , 0.5);
			v.normal = lerpResult17.rgb;
		}

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.00000000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( _GrabTexture, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			color.rgb = color.rgb + Refraction( i, o, _Refeaction, _ChromaticAberration ) * ( 1 - color.a );
			color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_TexCoord19 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 panner11 = ( uv_TexCoord19 + _Time.y * float2( 0.06,0.02 ));
			float2 panner10 = ( uv_TexCoord19 + 1.0 * _Time.y * float2( -0.085688,-0.06 ));
			float4 lerpResult17 = lerp( tex2D( _TextureSample0, panner11 ) , tex2D( _TextureSample1, panner10 ) , 0.5);
			o.Normal = lerpResult17.rgb;
			o.Albedo = _Base.rgb;
			float cameraDepthFade40 = (( i.eyeDepth -_ProjectionParams.y - 0.0 ) / _Distance);
			float4 lerpResult29 = lerp( _Water , _Bottem , cameraDepthFade40);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV31 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode31 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV31, 1.0 ) );
			float4 lerpResult39 = lerp( lerpResult29 , ( ( _Bottem * fresnelNode31 ) * 2.0 ) , fresnelNode31);
			o.Emission = lerpResult39.rgb;
			o.Smoothness = 1.0;
			o.Alpha = _Transparent;
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha finalcolor:RefractionF fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float3 customPack1 : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.z = customInputData.eyeDepth;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.eyeDepth = IN.customPack1.z;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
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
690;277;1751;1004;633.0042;81.85242;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-1554.788,277.6427;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;20;-1519.301,446.2845;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;30;-1123.527,-454.8626;Float;False;Property;_Bottem;Bottem;2;0;0.41609,0.7647059,0.5627491,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;28;-1228.189,-170.1939;Float;False;Property;_Distance;Distance;5;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.FresnelNode;31;-1122.065,-40.06912;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;38;-705.2434,23.36207;Float;False;Constant;_Float0;Float 0;5;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.PannerNode;10;-1220.352,501.2651;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.085688,-0.06;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ColorNode;25;-1015.678,-662.1758;Float;False;Property;_Water;Water;0;0;0.41609,0.7647059,0.5627491,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CameraDepthFade;40;-1022.825,-169.6351;Float;False;2;0;FLOAT;1.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PannerNode;11;-1233.351,229.9712;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.06,0.02;False;1;FLOAT;0.5;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-798.6287,-162.8301;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;16;-973.6753,397.252;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;Assets/Water/T_Water_N.TGA;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-968.4006,181.7164;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Assets/Water/T_Water_N.TGA;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;29;-692.8027,-406.9509;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-643.2465,-165.4032;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;17;-594.423,293.756;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.5;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;9;-127.1569,-439.2177;Float;False;Property;_Base;Base;1;0;0.3148789,0.3365946,0.7647059,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;18.99585,199.1476;Float;False;Property;_Transparent;Transparent;7;0;0.8;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;55;-220.0042,65.14758;Float;False;Property;_Refeaction;Refeaction;8;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;52;-26.67554,4.579681;Float;False;Constant;_Float1;Float 1;6;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.CameraDepthFade;27;-1265.386,-820.6243;Float;False;2;0;FLOAT;1.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DepthFade;26;-1016.374,-246.8935;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;39;-342.5868,-269.4711;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;254.2696,-46.99999;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Example_water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;False;Transparent;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;19;0
WireConnection;40;0;28;0
WireConnection;11;0;19;0
WireConnection;11;1;20;2
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;16;1;10;0
WireConnection;15;1;11;0
WireConnection;29;0;25;0
WireConnection;29;1;30;0
WireConnection;29;2;40;0
WireConnection;34;0;32;0
WireConnection;34;1;38;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;26;0;28;0
WireConnection;39;0;29;0
WireConnection;39;1;34;0
WireConnection;39;2;31;0
WireConnection;0;0;9;0
WireConnection;0;1;17;0
WireConnection;0;2;39;0
WireConnection;0;4;52;0
WireConnection;0;8;55;0
WireConnection;0;9;54;0
WireConnection;0;12;17;0
ASEEND*/
//CHKSM=4C5D0B7CE8C6122C1FBEE0F52EC46954BE3E88A0