using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/*实际的渲染工作，该类最重要的是要有RenderPassEvent，它里面有Pass执行的时间点
     用来控制每个pass的执行顺序在 ScriptableRenderPass 里，
     可以在构造函数里面去指定了Event 在什么时间点执行
     （默认的话Event是在不透明物体之后画，即 AfterRenderingOpaque）。*/
//private EdgeDetectNormalsAndDepth edgePass;
public class EdgeDetectNormalsAndDepth : ScriptableRenderPass
{
    //标签名，用于后续帧调试器中显示缓冲区名称
    private const string CommandBufferTag = "EdgeDetectNormalsAndDepth Pass";

    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    private static readonly int EdgeOnlyId = Shader.PropertyToID("_EdgeOnly");
    private static readonly int EdgeColorId = Shader.PropertyToID("_EdgeColor");
    private static readonly int BackgroundColorId = Shader.PropertyToID("_BackgroundColor");
    private static readonly int SampleDistanceId = Shader.PropertyToID("_SampleDistance");
    private static readonly int SensitivityId = Shader.PropertyToID("_Sensitivity");
    private static readonly int CameraDepthNormalsTexture = Shader.PropertyToID("_CameraDepthNormalsTexture");
    
    private EdgeVolume edgeVolume;              //显示在volume面板上的值
    private Material edgeM = null;              //用于后处理的材质
    private RenderTargetIdentifier m_Source;            //主纹理信息
    
    private RenderTextureDescriptor m_Descriptor;       //当前帧的渲染纹理描述
    
    //目标相机信息
    private RenderTargetHandle m_Destination;
    
    //临时的渲染目标
    private RenderTargetHandle edge_TemporaryColorTexture01;
    
    
    //构造函数
    public EdgeDetectNormalsAndDepth(RenderPassEvent evt)
    {
        renderPassEvent = evt;
    }
    
    //一个入口函数，用于后续渲染管线功能脚本写入参数
    public void Setup(RenderTargetIdentifier _ColorAttachment,Material _material)
    {
        this.m_Source = _ColorAttachment;
        edgeM = _material;
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    /*核心方法，定义执行规则，包含渲染逻辑，设置渲染状态，
     绘制渲染器或绘制程序网格，调度计算等等。*/
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        //环境准备 确定材质是否创建
        if (edgeM == null)
        {
            Debug.LogError("Marerial not created");
            return;
        }
        //后处理没有生效
        if (!renderingData.cameraData.postProcessEnabled) return;
        
        //从volume框架中获取所有堆栈，从堆栈中查找对应的属性参数组件
        var stack = VolumeManager.instance.stack;
        edgeVolume = stack.GetComponent<EdgeVolume>();

        if (edgeVolume == null)
        {
            return;
        }
        if(!edgeVolume.IsActive()) return;
        
        //从命令缓冲区池中获取一个带标签的渲染命令，该标签名可以在后续帧调试器中见到
        var cmd = CommandBufferPool.Get(CommandBufferTag);

        Render(cmd, ref renderingData);
        
        //执行命令缓冲区
        context.ExecuteCommandBuffer(cmd);
        //释放命令缓存
        CommandBufferPool.Release(cmd);
        //释放临时RT
        cmd.ReleaseTemporaryRT(edge_TemporaryColorTexture01.id);
    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ref var cameraData = ref renderingData.cameraData;
        var source = m_Source;
        
        if (edgeVolume.IsActive() && !cameraData.isSceneViewCamera)
        {
            // 写入参数
            edgeM.SetFloat(EdgeOnlyId, edgeVolume.edgesOnly.value);
            edgeM.SetColor(EdgeColorId, edgeVolume.edgeColor.value);
            edgeM.SetColor(BackgroundColorId, edgeVolume.backgroundColor.value);
            edgeM.SetFloat(SampleDistanceId, edgeVolume.sampleDistance.value);
            edgeM.SetVector(SensitivityId, new Vector4(edgeVolume.sensitivityNormals.value, edgeVolume.sensitivityDepth.value, 0.0f, 1.0f));
            
            var desc = cameraData.cameraTargetDescriptor;
            desc.width = cameraData.cameraTargetDescriptor.width;
            desc.height = cameraData.cameraTargetDescriptor.height;
            desc.depthBufferBits = 0;
            desc.msaaSamples = 1;
            
            cmd.GetTemporaryRT(edge_TemporaryColorTexture01.id, desc, FilterMode.Bilinear);

            // 通过材质，将计算结果存入临时缓冲区
            cmd.Blit(source, edge_TemporaryColorTexture01.Identifier(), edgeM);
            // 再从临时缓冲区存入主纹理
            cmd.Blit(edge_TemporaryColorTexture01.Identifier(), source);
        }
    }
}
