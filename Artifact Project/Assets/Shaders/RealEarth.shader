// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "Atmosphere/RealEarth"
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
		SubShader
	{
		
		Pass
		{
			Tags{"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float4 _LightColor0;
			
			uniform sampler2D _MainTex;
			uniform sampler2D _SeconTex;
			uniform sampler2D _SpecTex;
			uniform sampler2D _BumpMap;
			uniform float4 _SpecColor;
			uniform float4 _BumpMap_ST;
			uniform float _Shininess;

			uniform float3 v3Translate;		// The objects world pos
			uniform float3 v3LightPos;		// The direction vector to the light source
			uniform float3 v3InvWavelength; // 1 / pow(wavelength, 4) for the red, green, and blue channels
			uniform float fOuterRadius;		// The outer (atmosphere) radius
			uniform float fOuterRadius2;	// fOuterRadius^2
			uniform float fInnerRadius;		// The inner (planetary) radius
			uniform float fInnerRadius2;	// fInnerRadius^2
			uniform float fKrESun;			// Kr * ESun
			uniform float fKmESun;			// Km * ESun
			uniform float fKr4PI;			// Kr * 4 * PI
			uniform float fKm4PI;			// Km * 4 * PI
			uniform float fScale;			// 1 / (fOuterRadius - fInnerRadius)
			uniform float fScaleDepth;		// The scale depth (i.e. the altitude at which the atmosphere's average density is found)
			uniform float fScaleOverScaleDepth;	// fScale / fScaleDepth
			uniform float fHdrExposure;		// HDR exposure

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 c0 : COLOR0;
				float3 c1 : COLOR1;
				float3 tangentWorld : TEXCOORD2;
				float3 normalWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;
			};

			float scale(float fCos)
			{
				float x = 1.0 - fCos;
				return fScaleDepth * exp(-0.00287 + x*(0.459 + x*(3.83 + x*(-6.80 + x*5.25))));
			}

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 v3CameraPos = _WorldSpaceCameraPos - v3Translate;	// The camera's current position
				float fCameraHeight = length(v3CameraPos);					// The camera's current height
				float fCameraHeight2 = fCameraHeight*fCameraHeight;			// fCameraHeight^2

				// Get the ray from the camera to the vertex and its length (which is the far point of the ray passing through the atmosphere)
				float3 v3Pos = mul(unity_ObjectToWorld, input.vertex).xyz - v3Translate;
				float3 v3Ray = v3Pos - v3CameraPos;
				float fFar = length(v3Ray);
				v3Ray /= fFar;

				// Calculate the closest intersection of the ray with the outer atmosphere (which is the near point of the ray passing through the atmosphere)
				float B = 2.0 * dot(v3CameraPos, v3Ray);
				float C = fCameraHeight2 - fOuterRadius2;
				float fDet = max(0.0, B*B - 4.0 * C);
				float fNear = 0.5 * (-B - sqrt(fDet));

				// Calculate the ray's starting position, then calculate its scattering offset
				float3 v3Start = v3CameraPos + v3Ray * fNear;
				fFar -= fNear;
				float fDepth = exp((fInnerRadius - fOuterRadius) / fScaleDepth);
				float fCameraAngle = dot(-v3Ray, v3Pos) / length(v3Pos);
				float fLightAngle = dot(v3LightPos, v3Pos) / length(v3Pos);
				float fCameraScale = scale(fCameraAngle);
				float fLightScale = scale(fLightAngle);
				float fCameraOffset = fDepth*fCameraScale;
				float fTemp = (fLightScale + fCameraScale);

				const float fSamples = 2.0;

				// Initialize the scattering loop variables
				float fSampleLength = fFar / fSamples;
				float fScaledLength = fSampleLength * fScale;
				float3 v3SampleRay = v3Ray * fSampleLength;
				float3 v3SamplePoint = v3Start + v3SampleRay * 0.5;

				// Now loop through the sample rays
				float3 v3FrontColor = float3(0.0, 0.0, 0.0);
				float3 v3Attenuate;
				for (int i = 0; i<int(fSamples); i++)
				{
					float fHeight = length(v3SamplePoint);
					float fDepth = exp(fScaleOverScaleDepth * (fInnerRadius - fHeight));
					float fScatter = fDepth*fTemp - fCameraOffset;
					v3Attenuate = exp(-fScatter * (v3InvWavelength * fKr4PI + fKm4PI));
					v3FrontColor += v3Attenuate * (fDepth * fScaledLength);
					v3SamplePoint += v3SampleRay;
				}

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				output.posWorld = mul(modelMatrix, input.vertex);
				output.c0 = v3FrontColor * (v3InvWavelength * fKrESun + fKmESun);
				output.c1 = v3Attenuate;
				output.normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				output.tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);
				output.binormalWorld = normalize(cross(output.normalWorld, output.tangentWorld)*input.tangent.w);

				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				float4 encodedNormal = tex2D(_BumpMap, _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw);
				float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 2.0 * encodedNormal.g - 1.0, 0.0);
				localCoords.z = sqrt(1.0 - dot(localCoords.z, localCoords.z));
				float3x3 local2WorldTranspose = float3x3(input.tangentWorld, input.binormalWorld, input.normalWorld);
				
				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 diffuseReflection = _LightColor0.rgb * tex2D(_MainTex, input.tex).rgb * max(0.0, dot(normalDirection, lightDirection));

				float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * tex2D(_MainTex, input.tex).rgb;
				float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
				float3 specularReflection = dot(normalDirection, lightDirection) < 0 ? float3(0.0, 0.0, 0.0) : _SpecColor.rgb * (1.0 - tex2D(_SpecTex, input.tex).r) * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess) * 1.5;

				half3 texel = (diffuseReflection + ambientLighting + specularReflection);
				half3 texel2 = tex2D(_SeconTex, input.tex).rgb;
				float3 col = input.c0 + 0.25 * input.c1;
				int colBool = 0;
				//Adjust color from HDR
				col = 1.0 - exp(col * -fHdrExposure);
				texel *= col.b;
				texel2 *= (.2 - col.b)*5;
				if (texel2.b < 0)
				{
					texel2.r = 0;
					texel2.b = 0;
					texel2.g = 0;
				}

				return float4(texel + col + texel2, 1.0);
			}

			ENDCG
		}

		Pass
		{
			Tags{"LightMode" = "ForwardAdd"}
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float4 _LightColor0;

			uniform sampler2D _MainTex;
			uniform sampler2D _SpecTex;
			uniform float _Shininess;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				output.posWorld = mul(modelMatrix, input.vertex);
				output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				float3 normalDirection = normalize(input.normalDir);
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				float inverseDistance = 1.0 / length(vertexToLightSource);
				float3 lightDirection = vertexToLightSource*inverseDistance;
				float attenuation = lerp(1.0, inverseDistance, _WorldSpaceLightPos0.w);
				float3 diffuseReflection = _LightColor0.rgb * tex2D(_MainTex, input.tex).rgb * max(0.0, dot(normalDirection, lightDirection))* attenuation;

				float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * tex2D(_MainTex, input.tex).rgb;
				float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
				float3 specularReflection =  attenuation * _LightColor0.rgb * max(0.0,(tex2D(_SpecTex, input.tex).rgb - .5)*-1) * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);

				return float4(diffuseReflection + specularReflection, 1.0);
			}

			ENDCG
		}

	}
	Fallback "Diffuse"
}