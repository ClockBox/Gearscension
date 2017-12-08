// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "IceBall_shader"
{
	Properties
	{
		_iceball_iceball_AlbedoTransparency("iceball_iceball_AlbedoTransparency", 2D) = "white" {}
		_iceball_iceball_Normal("iceball_iceball_Normal", 2D) = "bump" {}
		_IceBallTexture_basecolor("IceBallTexture_basecolor", 2D) = "white" {}
		_IceBallTexture_normal("IceBallTexture_normal", 2D) = "white" {}
		_iceball_iceball_MetallicSmoothness("iceball_iceball_MetallicSmoothness", 2D) = "white" {}
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

		uniform sampler2D _iceball_iceball_Normal;
		uniform float4 _iceball_iceball_Normal_ST;
		uniform sampler2D _IceBallTexture_normal;
		uniform sampler2D _iceball_iceball_AlbedoTransparency;
		uniform float4 _iceball_iceball_AlbedoTransparency_ST;
		uniform sampler2D _IceBallTexture_basecolor;
		uniform sampler2D _iceball_iceball_MetallicSmoothness;
		uniform float4 _iceball_iceball_MetallicSmoothness_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_iceball_iceball_Normal = i.uv_texcoord * _iceball_iceball_Normal_ST.xy + _iceball_iceball_Normal_ST.zw;
			float2 uv_TexCoord5 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 panner4 = ( uv_TexCoord5 + 1.0 * _Time.y * float2( 1,0 ));
			float4 lerpResult9 = lerp( float4( UnpackNormal( tex2D( _iceball_iceball_Normal, uv_iceball_iceball_Normal ) ) , 0.0 ) , tex2D( _IceBallTexture_normal, panner4 ) , float4( 0.5147059,0.5109213,0.5109213,0 ));
			o.Normal = lerpResult9.rgb;
			float2 uv_iceball_iceball_AlbedoTransparency = i.uv_texcoord * _iceball_iceball_AlbedoTransparency_ST.xy + _iceball_iceball_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _iceball_iceball_AlbedoTransparency, uv_iceball_iceball_AlbedoTransparency ).rgb;
			float4 lerpResult6 = lerp( tex2D( _IceBallTexture_basecolor, panner4 ) , float4(0,0,0,0) , float4( 0.6029412,0.6029412,0.6029412,0 ));
			o.Emission = lerpResult6.rgb;
			float2 uv_iceball_iceball_MetallicSmoothness = i.uv_texcoord * _iceball_iceball_MetallicSmoothness_ST.xy + _iceball_iceball_MetallicSmoothness_ST.zw;
			float4 tex2DNode10 = tex2D( _iceball_iceball_MetallicSmoothness, uv_iceball_iceball_MetallicSmoothness );
			o.Metallic = tex2DNode10.r;
			o.Smoothness = tex2DNode10.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
111;236;1373;1004;200.8354;155.2914;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1200.584,224.8068;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;4;-930.5839,240.8069;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;3;-527.3822,183.464;Float;True;Property;_IceBallTexture_basecolor;IceBallTexture_basecolor;2;0;Assets/Prac/IceBAll/Materials/IceBallTexture_basecolor.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;8;-686.614,45.53107;Float;True;Property;_IceBallTexture_normal;IceBallTexture_normal;3;0;Assets/Prac/IceBAll/Materials/IceBallTexture_normal.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;7;-565.7928,496.6745;Float;False;Constant;_Color0;Color 0;3;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-512.3577,-158.0245;Float;True;Property;_iceball_iceball_Normal;iceball_iceball_Normal;1;0;Assets/Prac/IceBAll/Materials/iceball_iceball_Normal.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-318.5,-324;Float;True;Property;_iceball_iceball_AlbedoTransparency;iceball_iceball_AlbedoTransparency;0;0;Assets/Prac/IceBAll/Materials/iceball_iceball_AlbedoTransparency.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;9;-96.77967,-79.53637;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;COLOR;0.5147059,0.5109213,0.5109213,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;6;-214.8855,422.416;Float;True;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0;False;2;COLOR;0.6029412,0.6029412,0.6029412,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;10;-226.4274,137.0699;Float;True;Property;_iceball_iceball_MetallicSmoothness;iceball_iceball_MetallicSmoothness;4;0;Assets/Prac/IceBAll/Materials/iceball_iceball_MetallicSmoothness.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;264,-196;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;IceBall_shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;3;1;4;0
WireConnection;8;1;4;0
WireConnection;9;0;2;0
WireConnection;9;1;8;0
WireConnection;6;0;3;0
WireConnection;6;1;7;0
WireConnection;0;0;1;0
WireConnection;0;1;9;0
WireConnection;0;2;6;0
WireConnection;0;3;10;0
WireConnection;0;4;10;0
ASEEND*/
//CHKSM=BBA808BD6D22D06CEDADFFDA85E27E017DCA6107