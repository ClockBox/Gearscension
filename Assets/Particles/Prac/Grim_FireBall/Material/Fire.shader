// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fire"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0,0,0)
		_Fire_Texture02("Fire_Texture02", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform sampler2D _Fire_Texture02;
		uniform float4 _Color0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord17 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float cos19 = cos( 0.8 * _Time.y );
			float sin19 = sin( 0.8 * _Time.y );
			float2 rotator19 = mul( uv_TexCoord17 - float2( 1,0.31 ) , float2x2( cos19 , -sin19 , sin19 , cos19 )) + float2( 1,0.31 );
			float2 uv_TexCoord14 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 panner13 = ( uv_TexCoord14 + 1.0 * _Time.y * float2( 0,-0.47 ));
			float4 lerpResult18 = lerp( tex2D( _TextureSample1, rotator19 ) , tex2D( _Fire_Texture02, panner13 ) , 0.5514706);
			float4 lerpResult25 = lerp( lerpResult18 , _Color0 , float4( 0.4044118,0.4044118,0.4044118,0 ));
			o.Emission = lerpResult25.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
1017;211;1373;1004;1833.147;898.4054;1.946433;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1240.187,-127.1836;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1198.589,175.3033;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RotatorNode;19;-911.1673,337.8936;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0.31;False;2;FLOAT;0.8;False;1;FLOAT2
Node;AmplifyShaderEditor.PannerNode;13;-986.9203,-129.2748;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.47;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;22;-715.6197,456.5244;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Assets/Prac/FireBall_Particle/Fire_Texture02.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;21;-705.201,-162.3408;Float;True;Property;_Fire_Texture02;Fire_Texture02;3;0;Assets/Prac/FireBall_Particle/Fire_Texture02.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;24;-272.1077,475.7762;Float;False;Property;_Color0;Color 0;2;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;18;-275.394,174.3508;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.5514706;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;25;-11.2858,329.7939;Float;True;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.4044118,0.4044118,0.4044118,0;False;1;COLOR
Node;AmplifyShaderEditor.PannerNode;16;-952.3222,81.21212;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.35,-0.87;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;178.3978,22.58201;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Fire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;17;0
WireConnection;13;0;14;0
WireConnection;22;1;19;0
WireConnection;21;1;13;0
WireConnection;18;0;22;0
WireConnection;18;1;21;0
WireConnection;25;0;18;0
WireConnection;25;1;24;0
WireConnection;16;0;17;0
WireConnection;0;2;25;0
ASEEND*/
//CHKSM=B2C21950A39E95DE05DB8C1376C11612FB8C8039