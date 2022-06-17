using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeFeature : ScriptableRendererFeature
{
    EdgeDetectNormalsAndDepth m_ScriptablePass;//后处理Pass

    public Shader shader;//用于后处理的Shader
    private Material _material = null;//根据Shader生成的材质

    /// <inheritdoc/>
    /// 初始化feature资源
    public override void Create()
    {
        m_ScriptablePass = new EdgeDetectNormalsAndDepth(RenderPassEvent.AfterRenderingTransparents);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    //Renderer中插入一个或多个ScriptableRenderPass
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //检测shader是否存在
        if (shader == null)
        {
            return;
        }

        //创建材质
        if (_material == null)
        {
            _material = CoreUtils.CreateEngineMaterial(shader);
        }
        
        //获取当前渲染相机的目标颜色，也就是主纹理
        var cameraColorTarget = renderer.cameraColorTarget;
        
        //设置调用后处理Pass
        m_ScriptablePass.Setup(cameraColorTarget,_material);

        //添加该Pass到渲染管线中
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


