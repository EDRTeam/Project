using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;
using System;

public class ModelChangeCommand : Command
{
    protected GameObject _Target;//Ҫ������Ŀ������
    protected GameObject _Brush;//Ҫ�����ĸ�����

    protected GameObject newModle;//�������ģ��
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
            //���ɲ������ģ��
            newModle = csgModle.PerformCSG();
            newModle.name = "newModle";
            newModle.transform.SetParent(_Target.transform.parent);
            newModle.AddComponent<BoxCollider>();
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
        csg.customMaterial = new Material(Shader.Find("Standard")); // ����
        csg.useCustomMaterial = false; // ʹ������Ĳ���������п�
        csg.hideGameObjects = true; // ����������Ŀ��ͻ��ʶ���
        csg.keepSubmeshes = true; // ����ԭʼ������Ͳ���
        return csg;
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
        csgModle = getCsg(CSG.Operation.Subtract);//���Ľ��
        CreateNewObj();
    }

    public override void Undo()
    {
        Debug.Log("1");
        Debug.Log(this.ToString());
        base.Undo();
        _Target.SetActive(true);
        _Brush.SetActive(true);
        //var brush = Instantiate(_BrushInf, _BrushInf.transform.parent);
        Destroy(newModle);
    }

    public override string ToString()
    {
        string s = _Target.ToString() + "/" + _Brush.ToString();
        return s;
    }
}