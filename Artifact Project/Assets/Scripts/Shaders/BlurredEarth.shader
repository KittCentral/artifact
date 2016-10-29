Shader "Custom/BlurredEarth" 
{
	Properties 
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_SeconTex("Back (RGB)", 2D) = "black" {}
		_SpecTex("Specular Image", 2D) = "black" {}
		_SpecColor("Specular Color", Color) = (1,1,1,1)
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Shininess("Shininess", Float) = 10
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline Width", Range(.002, 0.5)) = .005
	}

	CGINCLUDE
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members vpos)
#pragma exclude_renderers d3d11 xbox360
		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : POSITION;
			float3 vpos;
			float4 color : COLOR;
		};

		uniform float _Outline;
		uniform float4 _OutlineColor;

		v2f vert(appdata v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			float3 norm = mul((float3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);
			o.pos.xy += offset * o.pos.z * _Outline;
			o.vpos = v.vertex.xyz;
			o.color = _OutlineColor;
			return o;
		}
	ENDCG

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		UsePass "Shaders/RealEarth"
		
		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : COLOR
			{
				float3 gradientCenter = float3(0,0,0);
				float3 pos = normalize(i.vpos.xyz - gradientCenter.xyz);
				float4 c = float4(i.color.rgb, pos.z*i.color.a);
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
