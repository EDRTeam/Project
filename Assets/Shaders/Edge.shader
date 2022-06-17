Shader "AdditionalPostProcess/Edge"
{
    Properties{
        _MainTex("Base (RGB)",2D)="white"{}                     //基础纹理
        _EdgeOnly("Edge Only",Float)=1.0                        //描边强度
        _EdgeColor("Edge Color",Color)=(0,0,0,1)                //描边颜色
        _BackgroundColor("Background Color",Color)=(1,1,1,1)    //背景颜色
        _SampleDistance("Sample Distance",Float)=1.0            //采样距离
        _Sensitivity("Sensitivity",Vector)=(1,1,1,1)            //xy对应法线和深度检测的灵敏度
    }
    SubShader{
        Tags {"RenderPipeline" = "UniversalPipeline"}
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        //CGINCLUDE
        //#include "UnityCG.cginc"

        CBUFFER_START(UnityPerMaterial)
        half4 _MainTex_ST;
        half4 _MainTex_TexelSize;
        half _EdgeOnly;
        half4 _EdgeColor;
        half4 _BackgroundColor;
        float _SampleDistance;
        half4 _Sensitivity;
        CBUFFER_END

        //声明深度法线纹理，注意该名称是指定的
        TEXTURE2D_X_FLOAT(_CameraDepthNormalsTexture);
        SAMPLER(sampler_CameraDepthNormalsTexture);

        //声明纹理
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        /*
        sampler2D _MainTex;
        half4 _MainTex_TexelSize;               //  存储纹素的大小
        fixed _EdgeOnly;
        fixed4 _EdgeColor;
        fixed4 _BackgroundColor;
        float _SampleDistance;                  
        half4 _Sensitivity;
        sampler2D _CameraDepthNormalsTexture;   //获取深度+法线纹理
        */
        
        struct a2v{
            float4 vertex: POSITION;
            float4 texcoord: TEXCOORD0;
        };

        struct v2f
        {
            float4 pos: SV_POSITION;
            
            /*
            定义了一个维度为5的纹理坐标数组，这个数组的第一个坐标储存了屏幕颜色图像的采样纹理，
	        我们对深度纹理的采样坐标进行了平台化差异处理，在必要情况下对它的竖坐标进行翻转，
	        数组中剩余的4个坐标则储存了使用Roberts算子需要采样的纹理坐标。
	        */
            half2 uv[5]: TEXCOORD0;
        };
        
        v2f vert(a2v v)
        {
            v2f o;
            o.pos = TransformObjectToHClip(v.vertex.xyz);
            
            // 对纹理进行采样，并放入第一个坐标中
            half2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            o.uv[0] = uv;
            
            // 检测DirectX平台
            #if UNITY_UV_STARTS_AT_TOP
                // 检测Unity是否已自动翻转了坐标
                if (_MainTex_TexelSize.y < 0)
                    uv.y = 1 - uv.y;
            #endif
            
            // 对纹理领域进行采样，并用_SampleDistance控制采样距离
            o.uv[1] = uv + _MainTex_TexelSize.xy * half2(1, 1) * _SampleDistance;
            o.uv[2] = uv + _MainTex_TexelSize.xy * half2(-1, -1) * _SampleDistance;
            o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 1) * _SampleDistance;
            o.uv[4] = uv + _MainTex_TexelSize.xy * half2(1, -1) * _SampleDistance;
            
            return o;
        }
        // 解码深度
        inline float DecodeFloatRG(float2 enc)
        {
            float2 kDecodeDot = float2(1.0, 1 / 255.0);
            return dot(enc, kDecodeDot);
        }
        
        // 解码法线
        float3 DecodeNormal(float4 enc)
        {
            float kScale = 1.7777;
            float3 nn = enc.xyz * float3(2 * kScale, 2 * kScale, 0) + float3(-kScale, -kScale, 1);
            float g = 2.0 / dot(nn.xyz, nn.xyz);
            float3 n;
            n.xy = g * nn.xy;
            n.z = g - 1;
            return n;
        }
        
        inline  void  DecodeDepthNormal(float4 enc, out float depth, out float3 normal)
        {
            depth = DecodeFloatRG(enc.zw);
            normal = DecodeNormal(enc);
        }
        
        half CheckSame(half4 center, half4 sample)
        {
            
            // 获取法线近似值
            half2 centerNormal = center.xy;
            // DecodeFloatRG：解码RG颜色到float
            float centerDepth = DecodeFloatRG(center.zw);
            half2 sampleNormal = sample.xy;
            float sampleDepth = DecodeFloatRG(sample.zw);
            
            // 法线差异 = 两个采样点对应值相减后乘以灵敏度
            half2 diffNormal = abs(centerNormal - sampleNormal) * _Sensitivity.x;
            // 将分量相加与阈值比较来判断是否存在边界
            int isSameNormal = (diffNormal.x + diffNormal.y) < 0.1;
            
            // 深度差异 = 两个采样点对应值相减后乘以灵敏度
            float diffDepth = abs(centerDepth - sampleDepth) * _Sensitivity.y;
            // 将深度差异与阈值比较得到是否存在边界结果
            int isSameDepth = diffDepth < 0.1 * centerDepth;
            
            // 将深度与法线结果相乘得到最终结果
            // return:
            // 1 - 如果法线和深度足够相似，即不存在边界
            // 0 - 否则
            return isSameNormal * isSameDepth ? 1.0: 0.0;
        }
        
        
        half4 fragRobertsCrossDepthAndNormal(v2f i): SV_Target
        {
            // 使用纹理坐标，对深度+法线纹理进行采样
            half4 sample1 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[1]);
            half4 sample2 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[2]);
            half4 sample3 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[3]);
            half4 sample4 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[4]);
            half4 sample = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[0]);
            /*
            float depth;
            float3 normal;
            DecodeDepthNormal(sample, depth, normal);
            //输出深度，从深度法线纹理中获取到的深度亮度值相比直接从深度纹理获取到的不够高，需要将获取到的深度值*1000以提升亮度
            return depth*1000;
            //输出法线
            return float4(normal, 1);
            */
            half edge = 1.0;
            
            // 计算对角线两个纹理的插值     CheckSame：返回0或1
            edge *= CheckSame(sample1, sample2);
            edge *= CheckSame(sample3, sample4);
            
            // 插值描边颜色
            half4 withEdgeColor = lerp(_EdgeColor, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[0]), edge);
            // 插值背景颜色
            half4 onlyEdgeColor = lerp(_EdgeColor, _BackgroundColor, edge);
            
            // 插值描边与背景颜色，由描边强度控制
            return lerp(withEdgeColor, onlyEdgeColor, _EdgeOnly);
        }
        
        
        ENDHLSL
        
        Pass
        {
            Tags { "RenderPipeline" = "UniversalPipeline" }
            ZTest Always Cull Off ZWrite Off
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment fragRobertsCrossDepthAndNormal
            
            ENDHLSL
            
        }
    }
    FallBack Off
}
/*
        v2f vert(a2v v){
            v2f o;
            o.pos = TransformObjectToHClip(v.vertex.xyz);
            //o.pos=UnityObjectToClipPos(v.vertex);
            half2 uv=v.texcoord;
            o.uv[0]=uv;

            // 进行平台差异化处理
            // 必要时对竖直方向进行翻转
            #if UNITY_UV_STARTS_AT_TOP
            if(_MainTex_TexelSize.y<0)
                uv.y=1-uv.y;
            #endif
            
            o.uv[1]=uv+_MainTex_TexelSize.xy*half2(1,1)*_SampleDistance;
            o.uv[2]=uv+_MainTex_TexelSize.xy*half2(-1,-1)*_SampleDistance;
            o.uv[3]=uv+_MainTex_TexelSize.xy*half2(-1,1)*_SampleDistance;
            o.uv[4]=uv+_MainTex_TexelSize.xy*half2(1,-2)*_SampleDistance;
            
            return o;
        }

        // 比较对角线上两个纹理值的差值，返回值为0或1
        // 返回0说明存在边界否则1
        half CheckSame(half4 center, half4 sample){
            /*CheckSame首先对输入参数进行处理，得到两个采样点的法线和深度值
			值得注意的是，这里并没有解码得到真正的法线值，而是直接使用了XY分量，这是因为我们只需要比较两个采样值之间的差异度，
			而并不需要知道他们的真正法线*/
            half2 centerNormal=center.xy;
            float centerDepth=DecodeFloatRG(center.zw);
            half2 sampleNormal=sample.xy;
            float sampleDepth=DecodeFloatRG(sample.zw);

            // difference in normals
            // do not bother decoding normals - there's no need here
            half2 diffNormal=abs(centerNormal-sampleNormal)*_Sensitivity.x;

            /*把差异值的每个分量相加再和一个阈值比较
             *如果他们的和小雨阈值则返回1，说明差异不明显，不是边界
             *否则返回0*/
            int isSameNormal=(diffNormal.x+diffNormal.y)<0.1;
            
            // difference in depth
            float diffDepth=abs(centerDepth-sampleDepth)*_Sensitivity.y;
            
            // scale the required threshold by the distance
            int isSameDepth=diffDepth<0.1*centerDepth;

            
            // return:
            // 1 - if normals and depth are similar enough
            // o - otherwise
            return isSameNormal*isSameDepth?1.0:0.0;
        }

        fixed4 fragRobertsCrossDepthAndNormal(v2f i):SV_Target{
            //用四个纹理坐标对深度+法线纹理进行采样
            half4 sample1=tex2D(_CameraDepthNormalsTexture,i.uv[1]);
            half4 sample2=tex2D(_CameraDepthNormalsTexture,i.uv[2]);
            half4 sample3=tex2D(_CameraDepthNormalsTexture,i.uv[3]);
            half4 sample4=tex2D(_CameraDepthNormalsTexture,i.uv[4]);

            //计算对角线上两个纹理值的差值
            half edge=1.0;
            edge*=CheckSame(sample1,sample2);
            edge*=CheckSame(sample3,sample4);

            fixed4 withEdgeColor=lerp(_EdgeColor,tex2D(_MainTex,i.uv[0]),edge);
            fixed4 onlyEdgeColor=lerp(_EdgeColor,_BackgroundColor,edge);

            return lerp(withEdgeColor,onlyEdgeColor,_EdgeOnly);
        }  

        ENDCG

        Pass{
            ZTest Always
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragRobertsCrossDepthAndNormal
            ENDCG
        }
    }
    Fallback Off
}*/
