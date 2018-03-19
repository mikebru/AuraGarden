Shader "Unlit/VertexWireframe"
{
	Properties
	{
		_Color ("_Color", Color) = (1,1,1,1)
		_FillColor ("FillColor", Color) = (0,0,0,0)
		_WireThickness ("Wire Thickness", RANGE(0, 800)) = 100
		[MaterialToggle] UseDiscard("Discard Fill", Float) = 1

		_Value1( "Value 1", Float ) = 0
		_Value2( "Value 2", Float ) = 0
		_Value3( "Value 3", Float ) = 0
 }

	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			// Wireframe shader based on the the following
			// http://developer.download.nvidia.com/SDK/10/direct3d/Source/SolidWireframe/Doc/SolidWireframe.pdf

			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			#pragma multi_compile _ USEDISCARD_ON
			#include "UnityCG.cginc"



			//User defined variables
			float _WireThickness;
			uniform float _Value1;
			uniform float _Value2;
			uniform float _Value3;


			// Base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
			};

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2g
			{
				float4 projectionSpaceVertex : SV_POSITION;
				float4 worldSpacePosition : TEXCOORD1;
			};

			struct g2f
			{
				float4 projectionSpaceVertex : SV_POSITION;
				float4 worldSpacePosition : TEXCOORD0;
				float4 dist : TEXCOORD1;
			};

			
			v2g vert (appdata v)
			{
				v2g o;
//				UNITY_SETUP_INSTANCE_ID(v);
//				UNITY_INITIALIZE_OUTPUT(v2g, o);
				o.projectionSpaceVertex = UnityObjectToClipPos(v.vertex);
				o.worldSpacePosition = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			[maxvertexcount(3)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triangleStream)
			{
				float2 p0 = i[0].projectionSpaceVertex.xy / i[0].projectionSpaceVertex.w;
				float2 p1 = i[1].projectionSpaceVertex.xy / i[1].projectionSpaceVertex.w;
				float2 p2 = i[2].projectionSpaceVertex.xy / i[2].projectionSpaceVertex.w;

				float2 edge0 = p2 - p1;
				float2 edge1 = p2 - p0;
				float2 edge2 = p1 - p0;

				// To find the distance to the opposite edge, we take the
				// formula for finding the area of a triangle Area = Base/2 * Height, 
				// and solve for the Height = (Area * 2)/Base.
				// We can get the area of a triangle by taking its cross product
				// divided by 2.  However we can avoid dividing our area/base by 2
				// since our cross product will already be double our area.
				float area = abs(edge1.x * edge2.y - edge1.y * edge2.x);
				float wireThickness = 800 - _WireThickness;

				g2f o;
				o.worldSpacePosition = i[0].worldSpacePosition;
				o.projectionSpaceVertex = i[0].projectionSpaceVertex;
				o.dist.xyz = float3( (area / length(edge0)), 0.0, 0.0) * o.projectionSpaceVertex.w * wireThickness;
				o.dist.w = 1.0 / o.projectionSpaceVertex.w;
				triangleStream.Append(o);

				o.worldSpacePosition = i[1].worldSpacePosition;
				o.projectionSpaceVertex = i[1].projectionSpaceVertex;
				o.dist.xyz = float3(0.0, (area / length(edge1)), 0.0) * o.projectionSpaceVertex.w * wireThickness;
				o.dist.w = 1.0 / o.projectionSpaceVertex.w;
				triangleStream.Append(o);

				o.worldSpacePosition = i[2].worldSpacePosition;
				o.projectionSpaceVertex = i[2].projectionSpaceVertex;
				o.dist.xyz = float3(0.0, 0.0, (area / length(edge2))) * o.projectionSpaceVertex.w * wireThickness;
				o.dist.w = 1.0 / o.projectionSpaceVertex.w;
				triangleStream.Append(o);
			}

			uniform fixed4 _Color;
			uniform fixed4 _FillColor;




			// Vertex function
			fragmentInput vert( vertexInput i )
			{
				fragmentInput o;

				// VERTEX ANIMATION ///////////////////////////////////////////////////////////////

				// Fat mesh
				// i.vertex.xyz += i.normal * _Value1;
				
				// Waving mesh
				i.vertex.x += sin( ( i.vertex.y + _Time * _Value3 ) * _Value2 ) * _Value1;

				// Bubbling mesh
				//i.vertex.xyz += i.normal * ( sin( (i.vertex.x + _Time * _Value3) * _Value2 ) + cos( (i.vertex.z + _Time * _Value3) * _Value2 )  ) * _Value1;

				//////////////////////////////////////////////////////////// EO VERTEX ANIMATION //

				// COLOR
				// o.color = i.texcoord;								// UV
				o.color = float4( i.normal, 1 ) * 0.5 + 0.5;		// Normals

				// This line must be after the vertex manipulation
				o.pos = UnityObjectToClipPos( i.vertex );
				return o;
			}



			fixed4 frag (g2f i, fragmentInput o) : SV_Target
			{
				float minDistanceToEdge = min(i.dist[0], min(i.dist[1], i.dist[2])) * i.dist[3];

				// Early out if we know we are not on a line segment.
				if(minDistanceToEdge > 0.9)
				{
				    #ifdef USEDISCARD_ON
					discard;
					#else
					return _FillColor;
					#endif
				}

				return _Color;
			}

			// Fragment function
		//	float4 frag( fragmentInput i ) : Color
		//	{
		//		return i.color;
			//}

			ENDCG
		}
	}
}
