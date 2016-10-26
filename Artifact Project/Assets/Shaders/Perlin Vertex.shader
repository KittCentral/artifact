Shader "Custom/Perlin Vertex" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BackTex("Back (RGB)", 2D) = "white" {}
		_WaterTex("Water (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert

	sampler2D _MainTex;
	sampler2D _BackTex;
	sampler2D _WaterTex;

	struct Input 
	{
		float3 worldPos;
		float2 uv_MainTex;
		half4 color : COLOR;
	};

	void surf(Input IN, inout SurfaceOutput o) 
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		half4 d = tex2D(_BackTex, IN.uv_MainTex);
		half4 w = tex2D(_WaterTex, IN.uv_MainTex);
		o.Albedo = (IN.worldPos.y<300?c.rgb + d.rgb :(800-IN.worldPos.y)/500*c.rgb + d.rgb) * IN.color.rgb;
		if (IN.worldPos.y < -907)
			o.Albedo = w.rgb;
		o.Alpha = c.a * IN.color.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
