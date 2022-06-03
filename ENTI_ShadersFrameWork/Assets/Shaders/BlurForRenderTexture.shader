Shader "Unlit/BlurToRenderTexture"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "" {}

		_BloomColor("BloomColor", Color) = (1,1,1,1)
		_BloomGlow("BloomIntensity", Range(0, 3)) = 1

		_intensity("Intensity",Float) = 0
		_blend("Blend",Float) = 0
		_deviation("Deviation",Float) = 0
		_dir("Dir",Vector) = (0,0,0,1)
		_iterations("Iterations",Int) = 0

	}
		SubShader
		 {
			 Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

			 Pass
			 {
				 HLSLPROGRAM
				 #pragma vertex vert
				 #pragma fragment frag
				 #define E 2.71828182846
				 #include "UnityCG.cginc"

				 struct appdata
				 {
					 float4 vertex : POSITION;
					 float2 uv : TEXCOORD0;
					 float3 normal : NORMAL;
				 };

				 struct v2f
				 {
					 float2 uv : TEXCOORD0;
					 float4 vertex : SV_POSITION;
					 float3 worldNormal : TEXCOORD1;
					 float3 wPos : TEXCOORD2;
				 };


				 v2f vert(appdata v)
				 {
					 v2f o;
					 o.vertex = UnityObjectToClipPos(v.vertex);
					 //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					 o.uv = v.uv;
					 o.worldNormal = UnityObjectToWorldNormal(v.normal);
					 o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					 return o;
				 }

				 Texture2D _MainTex;
				 SamplerState sampler_MainTex;

				 float4 _BloomColor;
				 half _BloomGlow;

				 float _intensity;
				 float _blend;
				 float _deviation;
				 float2 _dir;
				 int _iterations;
				 //float4 samplera = TEXTURE2D_SAMPLER2D(_mainTexture, sampler_MainTex);

				 fixed4 frag(v2f i) : SV_Target
				 {
					 fixed4 finalColor = (0,0,0,0);
					 float PI = 3.14159265359;
					 //finalColor += clamp(float4(_pointLightIntensity * (difuseComp + specularComp), 1), 0, 1);

					 float4 originalColor = _MainTex.Sample(sampler_MainTex, i.uv);
					 float4 color = originalColor;
					 float sum = _iterations;

					 for (float index = -1; index < _iterations; index++)
					 {
						 float offset = ((index / _iterations - 1) - 0.5) * _intensity;
						 float2 uv = i.uv + float2(_dir.x * offset, _dir.y * offset);
						 float devSquared = _deviation * _deviation;
						 float gauss = (1 / sqrt(2 * PI * devSquared)) * pow(E, -((offset * offset) / (2 * devSquared)));
						 sum += gauss;
						 color += _MainTex.Sample(sampler_MainTex, uv) * gauss;
					 }

					 color = color / sum;

					 //Bloom
					 float4 bloomColor = _MainTex.Sample(sampler_MainTex, i.uv);
					 bloomColor *= _BloomColor;
					 bloomColor *= _BloomGlow;

					 finalColor = lerp(originalColor, color + bloomColor, _blend);

					 //pointLight

					 return finalColor;
				 }
				 ENDHLSL
			 }
		 }
}
