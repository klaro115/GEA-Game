// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/GroundSplat" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture 0 (RGB)", 2D) = "white" {}
		_SecTex ("Texture 1 (RGB)", 2D) = "white" {}
		_SplatMap ("SplatMap (A)", 2D) = "white" {}

		_SplatScale ("Splat Scale", float) = 0.1
		_ScrollRate ("Scroll rate", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma vertex vert
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecTex;
		sampler2D _SplatMap;

		struct VertexInput
		{
			float2 texcoord0 : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;
			float3 normal : NORMAL;
			float4 color : COLOR;
			float4 vertex : VERTEX;
		};
		struct Input
		{
			float2 uv_MainTex;
			float4 worldPos : COLOR;
		};

		fixed4 _Color;
		float _SplatScale;
		float _ScrollRate;

		Input vert(inout VertexInput IN)
		{
			Input output;
			output.uv_MainTex = IN.texcoord0;
			output.worldPos = IN.vertex * _SplatScale;
			return output;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float4 localPos = mul(unity_ObjectToWorld, IN.worldPos);

			float scrollOffset = _Time * _ScrollRate;
			float2 surfaceUv = IN.uv_MainTex;
			surfaceUv.y += scrollOffset;
			localPos.y += scrollOffset;

			fixed4 tex0 = tex2D(_MainTex, surfaceUv);
			fixed4 tex1 = tex2D(_SecTex, surfaceUv);
			float splatValue = tex2D(_SplatMap, localPos.xy).r;
			splatValue = clamp(splatValue, 0.0, 1.0);

			//fixed4 surfaceColor = lerp(tex0, tex1, splatValue);
			fixed4 surfaceColor = fixed4(localPos.xyz, 1);

			fixed4 c = surfaceColor * _Color;
			o.Albedo = c.rgb;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
