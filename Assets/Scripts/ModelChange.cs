using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;
using UnityEngine.UI;
public class ModelChange : MonoBehaviour
{
    public GameObject Target;//要操作的目标物体
    public GameObject Brush;//要操作的副物体
    GameObject newModle;//操作后的模型
    CSG csgModle;
    void Start()
    {
    }
    //求并集
    public void getUnion()
    {
        //getTarget();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Union);//求交集的结果
        CreateNewObj();
    }
    //求差集
    public void getSubtract()
    {
        //getTarget();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Subtract);//求差集的结果
        CreateNewObj();
    }
    //求交集
    public void getIntersection()
    {
        //getTarget();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Intersection);//求交集的结果
        CreateNewObj();
    }
    //点击按钮后点击选取目标
    void getTarget()
    {

    }
    //几何布尔运算
    CSG getCsg(CSG.Operation OperationType)
    {
        CSG csg;
        csg = new CSG();
        csg.Brush = Brush;
        csg.Target = Target;
        csg.OperationType = OperationType;
        csg.customMaterial = new Material(Shader.Find("Standard")); // 材质
        csg.useCustomMaterial = false; // 使用上面的材质来填充切口
        csg.hideGameObjects = true; // 操作后隐藏目标和画笔对象
        csg.keepSubmeshes = true; // 保持原始的网格和材质
        return csg;
    }

    void Update()
    {
    }
    void CreateNewObj()
    {
        if (newModle != null)
        {
            Destroy(newModle);
        }
        //生成操作后的模型
        newModle = csgModle.PerformCSG();
        newModle.name = "newModle";
        newModle.transform.SetParent(transform.parent);
        csgModle.Target = newModle;
        //Brush.SetActive(true);
    }    
}
