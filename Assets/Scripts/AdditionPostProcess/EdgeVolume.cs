using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeVolume : VolumeComponent,IPostProcessComponent
{
    [Tooltip("开关")] public BoolParameter _Switch = new BoolParameter(false);
    
    [Range(0f, 1f), Tooltip("只渲染描边或在材质上描边")]
    public FloatParameter edgesOnly = new ClampedFloatParameter(0f, 0f, 1.0f);
    
    [Tooltip("描边颜色")]
    public ColorParameter edgeColor = new ColorParameter(Color.black);
    
    [Tooltip("背景颜色")]
    public ColorParameter backgroundColor = new ColorParameter(Color.white);
    
    [Tooltip("采样距离，数值越大描边越宽")]
    public FloatParameter sampleDistance = new ClampedFloatParameter(1f,-10f,10);

    [Tooltip("领域深度灵敏度")] public FloatParameter sensitivityDepth = new ClampedFloatParameter(1f, -10f, 10);
    
    [Tooltip("领域法线灵敏度")]
    public FloatParameter sensitivityNormals = new ClampedFloatParameter(1f, -10f, 10);
    
    public bool IsActive()
    {
        return _Switch.value;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
