Shader "AdditionalPostProcess/EdgePostProcess"
{
    Properties
        {
            _MainTex ("Base (RGB)", 2D) = "white" {}
    		_EdgeOnly("EdgeOnly",Float) = 1.0
    		_EdgeColor("EdgeColor",Color) = (0,0,0,1)
    		_BackgroundColor("BackgroundColor",Color) = (1,1,1,1)
    			//采样距离
    		_SampleDistance("Sample Distance",Float) = 1.0
    			//灵敏度
    		_Sensitivity("Sensitivity",Vector) = (1,1,1,1)
    	}
    		SubShader
    		{
    
    			CGINCLUDE	
                #include "UnityCG.cginc"
    			sampler2D _MainTex;
    		    half4 _MainTex_TexelSize;//存储纹素的大小
    			fixed _EdgeOnly;
    			fixed4 _EdgeColor;
    			fixed4 _BackgroundColor;
    			float _SampleDistance;
    			half4 _Sensitivity;
    			sampler2D _CameraDepthNormalsTexture;//获取深度+纹理
    			struct v2f
    			{
    				float4 pos : SV_POSITION;
    				half2 uv[5] : TEXCOORD0;
    				/*定义了一个维度为5的纹理坐标数组，这个数组的第一个坐标储存了屏幕颜色图像的采样纹理，
    	我们对深度纹理的采样坐标进行了平台化差异处理，在必要情况下对它的竖坐标进行翻转，
    	数组中剩余的4个坐标则储存了使用Roberts算子需要采样的纹理坐标。
    	_SampleDistance 用来控制采样距离。*/
    			};
    			v2f vert(appdata_img v)
    			{
    				v2f o;
    				o.pos = UnityObjectToClipPos(v.vertex);
    			    half2 uv = v.texcoord;
    				o.uv[0] = uv;
    				//适应平台差异化
                   #if UNITY_UV_STARTS_AT_TOP
    				//对竖直方向进行翻转
    				if (_MainTex_TexelSize.y < 0)				
    					uv.y = 1 - uv.y;								
                    #endif
    				//使用roberts算子时需要采样的纹理坐标
    				o.uv[1] = uv + _MainTex_TexelSize.xy*half2(1, 1)*_SampleDistance;
    				o.uv[2] = uv + _MainTex_TexelSize.xy*half2(-1,-1)*_SampleDistance;
    				o.uv[3] = uv + _MainTex_TexelSize.xy*half2(-1,1)*_SampleDistance;
    				o.uv[4] = uv + _MainTex_TexelSize.xy*half2(1,-1)*_SampleDistance;
    				return o;
    			}
    			//计算对角线上两个纹理值的差值 
    			half CheckSame(half4 center, half4 sample) {
    
    				/*CheckSame首先对输入参数进行处理，得到两个采样点的法线和深度值
    				值得注意的是，这里并没有解码得到真正的法线值，而是直接使用了XY分量，这是因为我们只需要比较两个采样值之间的差异度，
    				而并不需要知道他们的真正法线*/
    				half2 centerNormal = center.xy;
    				float centerDepth = DecodeFloatRG(center.zw);
    				half2 sampleNormal = sample.xy;
    				float sampleDepth = DecodeFloatRG(sample.zw);
    				// difference in normals  法线的差
    				// do not bother decoding normals - there's no need here 不要费心去解码法线——这里没有必要
    				/*然后我们把两个采样点的对应值相减并取绝对值，再乘以灵敏度的参数*/
    				half2 diffNormal = abs(centerNormal - sampleNormal) * _Sensitivity.x;
    
    				/*把差异值的每个分量相加再和一个阀值比较，
    				如果他们的和小于阀值，则返回1，说明差异不明显，不存一条边界，否则返回0*/
    				int isSameNormal = (diffNormal.x + diffNormal.y) < 0.1;
    
    
    				// difference in depth  不同的深度
    				float diffDepth = abs(centerDepth - sampleDepth) * _Sensitivity.y;
    
    				// scale the required threshold by the distance   按距离缩放所需的阈值
    				int isSameDepth = diffDepth < 0.1 * centerDepth;
    
    				/*最后我们把法线和深度的检查结果相乘，作为组合后的返回值*/
    				// return:
    				// 1 - if normals and depth are similar enough  如果法线和深度足够相似
    				// 0 - otherwise
    				return isSameNormal * isSameDepth ? 1.0 : 0.0;
    			}
    
    		
    	
    			fixed4 fragRobertsCrossDepthAndNormal(v2f i) : SV_Target
    			{
    				//使用四个纹理坐标对深度+法线纹理进行采样
    				half4 sample1 = tex2D(_CameraDepthNormalsTexture,i.uv[1]);
    				half4 sample2 = tex2D(_CameraDepthNormalsTexture, i.uv[2]);
    				half4 sample3 = tex2D(_CameraDepthNormalsTexture, i.uv[3]);
    				half4 sample4 = tex2D(_CameraDepthNormalsTexture, i.uv[4]);
    				half edge = 1.0;
    				//计算对角线上两个纹理值的差值 CheckSame的返回值要么是0要么是1
    				//.返回0时表明这两点存在一条边界，反之则返回1
    				edge *= CheckSame(sample1,sample2);
    				edge *= CheckSame(sample3, sample4);
    				fixed4 withEdgeColor = lerp(_EdgeColor, tex2D(_MainTex, i.uv[0]), edge);
    				fixed4 onlyEdgeColor = lerp(_EdgeColor, _BackgroundColor, edge);
    				return lerp(withEdgeColor,onlyEdgeColor,_EdgeOnly);
    			   }
    				
    			ENDCG
    
        
           
    
            Pass
            {
    				ZTest Always Cull Off ZWrite Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment fragRobertsCrossDepthAndNormal
    
              
                ENDCG
            }
        }
    			Fallback Off
}
