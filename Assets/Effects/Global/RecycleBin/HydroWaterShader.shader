Shader "Custom/HydroWaterShader"
{
//博客抄来的Shader，看起来非常合理，但是为甚么TMD跑不了啊？！
Properties { 

	_BaseColor ("Base color", COLOR)  = ( .54, .95, .99, 0.5) 
	_SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
    _Shininess ("Shininess", Float) = 10
	_depthOffset("Depth Offset", float) = 1




		_WavePos("Wave Position",Vector)=(0,0,0,0)
		_WaveHeight("Wave Height",Float) = 1
		_WaveFrequency("Wave Frequency",Float) = 1

}


CGINCLUDE 

	#include "UnityCG.cginc" 
	#include "UnityLightingCommon.cginc" // for _LightColor0

	sampler2D_float _CameraDepthTexture;
  
	uniform float4 _BaseColor;  
    uniform float _Shininess;
	 
	fixed _depthOffset;
	float4 _WavePos;
	float _WaveHeight;
	float _WaveFrequency;
 
	
	struct v2g
	{
		float4 pos : SV_POSITION;
		float3 vertex:TEXCOORD0;
		float4 scrPos:TEXCOORD1;
		float depth : SV_Depth;
	}; 
 
	struct g2f
	{
		float4 pos : SV_POSITION;
		fixed3 worldNormal : NORMAL;
		float3 vertex:TEXCOORD0;
		float4 scrPos:TEXCOORD1;
		//定义fog贴图坐标
		UNITY_FOG_COORDS(2)
			float depth:SV_Depth;
	};

	v2g vert(appdata_full v)
	{
		v2g o;
		//初始化v2g内的数据
		UNITY_INITIALIZE_OUTPUT(v2g, o);
		//世界坐标和相机的距离向量
		//o.viewInterpolator.xyz = worldSpaceVertex - _WorldSpaceCameraPos;
		//o.worldPos = mul(unity_ObjectToWorld, (v.vertex)).xyz;
		//模型坐标转化为世界坐标
		float3 worldSpaceVertex = mul(unity_ObjectToWorld,(v.vertex)).xyz;
		//通过世界坐标计算波动发起点的距离
		float dis = distance(worldSpaceVertex, _WavePos);
		//计算顶点y坐标
		v.vertex.y = _WaveHeight * sin((_Time.y* _WaveFrequency + dis)*6.28f);

		//顶点坐标转换到裁剪空间
		o.pos = UnityObjectToClipPos(v.vertex);
		o.vertex = v.vertex;

		//ComputeScreenPos COMPUTE_EYEDEPTH 必须在vert内调用 
		o.scrPos = ComputeScreenPos(o.pos);
		//计算当前深度（注意因为ZWrite关闭所以该深度无法在深度图读取）
		COMPUTE_EYEDEPTH(o.depth);
		//UnityWorldSpaceViewDir 得出从顶点到摄像机的向量 等价_WorldSpaceCameraPos - worldPos
       // float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos)); 

		return o;
	}

	 [maxvertexcount(3)]
	 void geom(triangle v2g input[3], inout TriangleStream<g2f> outStream)
	 {
		fixed3 normal=normalize(UnityObjectToWorldNormal(cross((input[1].vertex - input[0].vertex),(input[2].vertex-input[0].vertex))));
		 for (int i = 0; i<3; i++) 
		 {
			 g2f o = (g2f)0;
			 o.pos = input[i].pos;
			 o.vertex = input[i].vertex;
			 o.depth = input[i].depth;
			 o.worldNormal = normal;
			 o.scrPos = input[i].scrPos;
			 //从顶点数据中输出雾效数据
			 UNITY_TRANSFER_FOG(o, o.pos);
			 //-----将一个顶点添加到输出流列表 
			 outStream.Append(o);
		 }
		 //outStream.RestartStrip();
	 }
	 
	 half4 calculateBaseColor(g2f input)
         {
			float3 worldPos= mul(unity_ObjectToWorld, (input.vertex)).xyz;
            float3 viewDirection = normalize(_WorldSpaceCameraPos - worldPos);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - worldPos;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            float3 ambientLighting = 
               UNITY_LIGHTMODEL_AMBIENT.rgb * _BaseColor.rgb;
 
            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _BaseColor.rgb
               * max(0.0, dot(input.worldNormal, lightDirection));
 
            float3 specularReflection;
            if (dot(input.worldNormal, lightDirection) < 0.0)
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else  
            {
               specularReflection = attenuation * _LightColor0.rgb  * _SpecColor.rgb 
				   * pow(max(0.0, dot(reflect(-lightDirection, input.worldNormal), viewDirection)), _Shininess);
            }
            return half4(ambientLighting + diffuseReflection  + specularReflection, _BaseColor.a);
         }

	half4 frag( g2f i ) : SV_Target
	{ 
 
        half4 baseColor = calculateBaseColor(i);
       
		//float4 screenPos = ComputeScreenPos(i.pos);
		//求深度
        half depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos));
			//线性化深度
        depth = LinearEyeDepth(depth);
		

        float diffValue =1-saturate(depth - i.depth);

		//应用fog
		UNITY_APPLY_FOG(i.fogCoord, baseColor);



		//return float4(i.deepth -_deepthOffset, i.deepth -_deepthOffset, i.deepth -_deepthOffset, 1);
		//return float4(depth - _deepthOffset, depth - _deepthOffset, depth - _deepthOffset, 1);
		//return float4(diffValue - _deepthOffset, diffValue - _deepthOffset, diffValue - _deepthOffset, 1);
		//return  calculateBaseColor(i);
		return baseColor+ diffValue *_depthOffset;
	}
	
ENDCG

Subshader
{
	Tags {"RenderType"="Transparent" "Queue"="Transparent"}
	
	Lod 500
	ColorMask RGB
	
	//GrabPass { "_RefractionTex" }
	
	Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Cull Off
		
			CGPROGRAM
		
			#pragma target 3.0
		
			#pragma vertex vert
			#pragma geometry geom 
			#pragma fragment frag
			#pragma multi_compile_fog
		
			//#pragma multi_compile WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF 
		
			ENDCG
	}
}


Fallback "Transparent/Diffuse"
}