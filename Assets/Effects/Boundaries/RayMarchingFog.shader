Shader "TA Shaders/RayMarchingFog"
{   
    //HLSLINCLUDE
    //#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

    //#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

    //#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

    //#include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"

    //#include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    //#include "HLSLSupport.cginc"
    //ENDHLSL 
    SubShader
    {
        Cull Off
        ZWrite Off
        Ztest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            //#include Unity.cginc
            sampler2D _CameraDepthTexture;
            //TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            //TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
            sampler2D _MainTex;

            float _Intensity;

            float4x4 _InvProjectionM;
            float4x4 _InvViewM;

            float _Accuracy = 0.1;
            int _Steps = 32;

            float _MapL;
            float _MapR;
            float _MapF;
            float _MapB;
            float _FogDist;
            float _FogThickness;
            float _FogDensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct attributes
            {
                uint vertexID : SV_VertexID;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                
            };

            float SDF(float3 pos)//get signed distance field at pos
            {
                return _FogDist;
            } 

            float SPF(float3 pos)//get potential field at pos
            {
                return _FogDensity;
            }

            float4 GetWorldPosition(float2 uv, float depth)//using texcoord and depth to calculate world position.
            {
                float4 viewPos = mul(_InvProjectionM, float4(uv*2-1, depth, 1.0));
                viewPos.xyz /= viewPos.w;

                float4 worldPos = mul(_InvViewM, float4(viewPos.xyz, 1));

                return worldPos;
            }

            float3 GetGradient(float3 pos)//get gradient of field at some point. Gradient is used as normal.
            {
                float3 grad;
                /*for(int i = 0; i< _Number; i++)
                {
                    float r = length(pos - _Blobs[i].xyz);
                    grad.x += WyvillDeri(r, _REffect[i]) * _Density[i] * (pos.x - _Blobs[i].x) /r;
                    grad.y += WyvillDeri(r, _REffect[i]) * _Density[i] * (pos.y - _Blobs[i].y) /r;
                    grad.z += WyvillDeri(r, _REffect[i]) * _Density[i] * (pos.z - _Blobs[i].z) /r;
                }*/
                return grad;
            }

            float RayMarching(float3 start, float3 dir)//dir should be normalized!
            {
                float3 current = start;
                float sd = 0;
                float sp = 0;

                sd = SDF(current);
                current += sd * dir;

                for(int i = 0; i <= _Steps; i++)
                {
                    sp += SPF(current);
                    current += _FogThickness/_Steps * dir;
                    if(sp >= 1)return 1;
                }
                return sp;
            }

            v2f vert (appdata i) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                //o.vertex = GetFullScreenTriangleVertexPosition(i.vertexID);
                //o.uv = GetFullScreenTriangleTexCoord(i.vertexID);
                return o;
            }

            float4 frag (v2f i ) : SV_Target
            {
                float depth = tex2D(_CameraDepthTexture, i.uv);
                float4 originColor = tex2D(_MainTex, i.uv);
                float3 worldPos = GetWorldPosition(i.uv, depth).xyz;
                float3 rayOrigin = _WorldSpaceCameraPos;

                float3 dir = normalize(worldPos - rayOrigin);//get a ray direction.
                
                float march = RayMarching(rayOrigin, dir);

                return lerp((1,1,0,1), (0,0,0,0), _Intensity);// + march;
            }
            ENDCG
        }
    }
}
