Shader "Unlit/FogOfWar"
{
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha
		//Cull off

		Pass
		{
			CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
			//#pragma exclude_renderers d3d11
			#pragma vertex vert  
			#pragma fragment frag 

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			int _VectorsCount = 0;
			float4 _Vectors[5];
			float _Distances[5];
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			//float3 world;
			float4 frag (v2f i) : COLOR
			{
				float3 world = i.worldPos;
				for (int i = 0; i < _VectorsCount; i++)
				{
					float dstSqr = pow(_Vectors[i].x - world.x, 2) + pow(_Vectors[i].y - world.y, 2) + pow(_Vectors[i].z - world.z, 2);
					for (int j = 0; j < _VectorsCount; j++)
					{
						if (dstSqr < pow(_Distances[j], 2))
							return float4(0, 0, 0, 0);
					}
				}
				return float4(0, 0, 0, 1);
			}
			ENDCG
		}
	}
}
