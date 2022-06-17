using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/*ʵ�ʵ���Ⱦ��������������Ҫ����Ҫ��RenderPassEvent����������Passִ�е�ʱ���
     ��������ÿ��pass��ִ��˳���� ScriptableRenderPass �
     �����ڹ��캯������ȥָ����Event ��ʲôʱ���ִ��
     ��Ĭ�ϵĻ�Event���ڲ�͸������֮�󻭣��� AfterRenderingOpaque����*/
//private EdgeDetectNormalsAndDepth edgePass;
public class EdgeDetectNormalsAndDepth : ScriptableRenderPass
{
    //��ǩ�������ں���֡����������ʾ����������
    private const string CommandBufferTag = "EdgeDetectNormalsAndDepth Pass";

    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    private static readonly int EdgeOnlyId = Shader.PropertyToID("_EdgeOnly");
    private static readonly int EdgeColorId = Shader.PropertyToID("_EdgeColor");
    private static readonly int BackgroundColorId = Shader.PropertyToID("_BackgroundColor");
    private static readonly int SampleDistanceId = Shader.PropertyToID("_SampleDistance");
    private static readonly int SensitivityId = Shader.PropertyToID("_Sensitivity");
    private static readonly int CameraDepthNormalsTexture = Shader.PropertyToID("_CameraDepthNormalsTexture");
    
    private EdgeVolume edgeVolume;              //��ʾ��volume����ϵ�ֵ
    private Material edgeM = null;              //���ں���Ĳ���
    private RenderTargetIdentifier m_Source;            //��������Ϣ
    
    private RenderTextureDescriptor m_Descriptor;       //��ǰ֡����Ⱦ��������
    
    //Ŀ�������Ϣ
    private RenderTargetHandle m_Destination;
    
    //��ʱ����ȾĿ��
    private RenderTargetHandle edge_TemporaryColorTexture01;
    
    
    //���캯��
    public EdgeDetectNormalsAndDepth(RenderPassEvent evt)
    {
        renderPassEvent = evt;
    }
    
    //һ����ں��������ں�����Ⱦ���߹��ܽű�д�����
    public void Setup(RenderTargetIdentifier _ColorAttachment,Material _material)
    {
        this.m_Source = _ColorAttachment;
        edgeM = _material;
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    /*���ķ���������ִ�й��򣬰�����Ⱦ�߼���������Ⱦ״̬��
     ������Ⱦ������Ƴ������񣬵��ȼ���ȵȡ�*/
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        //����׼�� ȷ�������Ƿ񴴽�
        if (edgeM == null)
        {
            Debug.LogError("Marerial not created");
            return;
        }
        //����û����Ч
        if (!renderingData.cameraData.postProcessEnabled) return;
        
        //��volume����л�ȡ���ж�ջ���Ӷ�ջ�в��Ҷ�Ӧ�����Բ������
        var stack = VolumeManager.instance.stack;
        edgeVolume = stack.GetComponent<EdgeVolume>();

        if (edgeVolume == null)
        {
            return;
        }
        if(!edgeVolume.IsActive()) return;
        
        //������������л�ȡһ������ǩ����Ⱦ����ñ�ǩ�������ں���֡�������м���
        var cmd = CommandBufferPool.Get(CommandBufferTag);

        Render(cmd, ref renderingData);
        
        //ִ���������
        context.ExecuteCommandBuffer(cmd);
        //�ͷ������
        CommandBufferPool.Release(cmd);
        //�ͷ���ʱRT
        cmd.ReleaseTemporaryRT(edge_TemporaryColorTexture01.id);
    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ref var cameraData = ref renderingData.cameraData;
        var source = m_Source;
        
        if (edgeVolume.IsActive() && !cameraData.isSceneViewCamera)
        {
            // д�����
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

            // ͨ�����ʣ���������������ʱ������
            cmd.Blit(source, edge_TemporaryColorTexture01.Identifier(), edgeM);
            // �ٴ���ʱ����������������
            cmd.Blit(edge_TemporaryColorTexture01.Identifier(), source);
        }
    }
}
