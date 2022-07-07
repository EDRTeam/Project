using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;
using System;

public class ModelChangeCommand : Command
{
    protected GameObject _Target;//要操作的目标物体
    protected GameObject _Brush;//要操作的副物体

    protected GameObject newModle;//操作后的模型
    protected CSG csgModle;

    public ModelChangeCommand(GameObject target, GameObject brush)
    {
        if (target != null && brush != null)
        {
            _Target = target;
            _Brush = brush;
        }
    }

    protected void CreateNewObj()
    {
        if (_Target != null && _Brush != null)
        {
            if (newModle != null)
            {

                Destroy(newModle);
            }
            //生成操作后的模型
            newModle = csgModle.PerformCSG();
            newModle.name = "newModle";
            newModle.transform.SetParent(_Target.transform.parent);
            newModle.AddComponent<BoxCollider>();
            newModle.AddComponent<Outline>();
            newModle.GetComponent<Outline>().OutlineWidth = 8f;
            newModle.GetComponent<Outline>().enabled = false;
            _Target.SetActive(false);
            _Brush.SetActive(false);
            //DestroyImmediate(_Target);
            //Destroy(_Target);
            // Destroy(_Brush);
            //Target = newModle;
        }
        //Brush.SetActive(true);
    }
    protected CSG getCsg(CSG.Operation OperationType)
    {
        CSG csg;
        csg = new CSG();
        csg.Brush = _Brush;
        csg.Target = _Target;
        csg.OperationType = OperationType;
        csg.customMaterial = new Material(Shader.Find("Standard")); // 材质
        csg.useCustomMaterial = false; // 使用上面的材质来填充切口
        csg.hideGameObjects = true; // 操作后隐藏目标和画笔对象
        csg.keepSubmeshes = true; // 保持原始的网格和材质
        return csg;
    }

    public override void Undo()
    {
        //Debug.Log("1");
        //Debug.Log(this.ToString());
        base.Undo();
        if (_Target != null)
        {
            _Target.SetActive(true);
        }
        if (_Brush != null)
        {
            _Brush.SetActive(true);
        }
        if (newModle != null)
        {
            newModle.SetActive(false);
        }
             
    }

    public override void Redo()
    {
        base.Redo();
        if (_Target != null)
        {
            _Target.SetActive(false);
        }
        if (_Brush != null)
        {
            _Brush.SetActive(false);
        }
        if (newModle != null)
        {
            newModle.SetActive(true);
        }

    }

    public override string ToString()
    {
        string s = _Target.ToString() + "/" + _Brush.ToString();
        return s;
    }
}

public sealed class SubtractCommand : ModelChangeCommand
{

    public SubtractCommand(GameObject target, GameObject brush) : base(target, brush)
    {

    }

    public override void Execute()
    {
        base.Execute();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Subtract);//求差集的结果
        CreateNewObj();
    }
}

public sealed class UnionCommand : ModelChangeCommand
{
    public UnionCommand(GameObject target, GameObject brush) : base(target, brush)
    {

    }

    public override void Execute()
    {
        base.Execute();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Union);//求并集的结果
        CreateNewObj();
    }
}

public sealed class IntersectionCommand : ModelChangeCommand
{
    public IntersectionCommand(GameObject target, GameObject brush) : base(target, brush)
    {

    }

    public override void Execute()
    {
        base.Execute();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Intersection);//求交集的结果
        CreateNewObj();
    }
}

