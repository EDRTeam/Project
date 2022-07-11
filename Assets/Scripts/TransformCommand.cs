using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformCommand : Command
{
    private GameObject go;

    private Vector3 startPos;

    private Vector3 endPos;

    public TransformCommand(GameObject go)
    {
        this.go = go;
        startPos = go.transform.localPosition;   //都是freeExpPos的子物体 
    }

    public override void Execute()
    {
        base.Execute();
        endPos = go.transform.localPosition;
    }

    public override void Redo()
    {
        base.Redo();
        go.transform.localPosition = endPos;
    }
    public override void Undo()
    {
        base.Undo();
        go.transform.localPosition = startPos;
    }
    public override bool CheckCommand()
    {
        //创建指令时，判断移动距离  过小生成指令
        if (Math.Abs(Vector3.Distance(endPos, startPos)) < 0.1)
        {
            return false;
        }
        //如果物体已丢失  移除指令  取消撤销重做
        if (go == null)
        {
            //ModelChange.instance.M_RedoList.Remove(this);
            Destroy(this);
            return false;
        }
        return base.CheckCommand();
    }

    public override string ToString()
    {
        return "移动"+go.name;
    }
}

public class RotationCommand : Command
{
    private GameObject go;

    //private Quaternion startRot;
    //private Quaternion endRot;

    private Vector3 startRot;
    private Vector3 endRot;

    private Slider XRot;
    private Slider YRot;
    private Slider ZRot;

    public RotationCommand(GameObject go ,Slider x, Slider y, Slider z, Vector3 rot)
    {
        this.go = go;
        XRot = x;
        YRot = y;
        ZRot = z;
        startRot = rot;
    }

    public override void Execute()
    {
        base.Execute();
        endRot = new Vector3(XRot.value, YRot.value, ZRot.value);
    }
    public override void Redo()
    {
        base.Redo();
        XRot.value = endRot.x;
        YRot.value = endRot.y;
        ZRot.value = endRot.z;

        go.transform.rotation = Quaternion.Euler(endRot);
    }
    public override void Undo()
    {
        base.Undo();
        XRot.value = startRot.x;
        YRot.value = startRot.y;
        ZRot.value = startRot.z;

        go.transform.rotation = Quaternion.Euler(startRot);
    }

    public override bool CheckCommand()
    {
        if (endRot == startRot)
        {
            return false;
        }
        //如果物体已丢失  移除指令  取消撤销重做
        if (go == null)
        {
            //ModelChange.instance.M_RedoList.Remove(this);
            Destroy(this);
            return false;
        }
        return base.CheckCommand();
    }

    public override string ToString()
    {
        return "旋转:" + go.name + endRot + "/" + startRot;
    }
}

public class ScaleCommand : Command
{
    private GameObject go;

    //private Quaternion startRot;
    //private Quaternion endRot;

    private Vector3 startSca;
    private Vector3 endSca;

    private Slider XSca;
    private Slider YSca;
    private Slider ZSca;

    public ScaleCommand(GameObject go, Slider x, Slider y, Slider z)
    {
        this.go = go;
        XSca = x;
        YSca = y;
        ZSca = z;
        startSca = new Vector3(XSca.value, YSca.value, ZSca.value);
    }

    public override void Execute()
    {
        base.Execute();
        endSca = new Vector3(XSca.value, YSca.value, ZSca.value);
    }
    public override void Redo()
    {
        base.Redo();
        XSca.value = endSca.x;
        YSca.value = endSca.y;
        ZSca.value = endSca.z;
        go.transform.localScale = endSca;
    }
    public override void Undo()
    {
        base.Undo();
        XSca.value = startSca.x;
        YSca.value = startSca.y;
        ZSca.value = startSca.z;
        go.transform.localScale = startSca;
    }

    public override bool CheckCommand()
    {
        if (endSca == startSca)
        {
            return false;
        }
        //如果物体已丢失  移除指令  取消撤销重做
        if (go == null)
        {
            //ModelChange.instance.M_RedoList.Remove(this);
            Destroy(this);
            return false;
        }
        return base.CheckCommand();
    }

    public override string ToString()
    {
        return "缩放" + go.name;
    }
}