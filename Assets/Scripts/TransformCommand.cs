using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCommand : Command
{
    private GameObject go;

    private Vector3 startPos;
    private Vector3 startSca;
    private Quaternion startRot;

    private Vector3 endPos;
    private Vector3 endSca;
    private Quaternion endRot;

    public TransformCommand(GameObject go)
    {
        this.go = go;
        startPos = go.transform.localPosition;   //都是freeExpPos的子物体 
        startSca = go.transform.localScale;
        startRot = go.transform.rotation;
    }

    public override void Execute()
    {
        base.Execute();
        endPos = go.transform.localPosition;
        endSca = go.transform.localScale;
        endRot = go.transform.rotation;
    }

    public override void Redo()
    {
        base.Redo();
        go.transform.localPosition = endPos;
        go.transform.localScale = endSca;
        go.transform.rotation = endRot;
    }
    public override void Undo()
    {
        base.Undo();
        go.transform.localPosition = startPos;
        go.transform.localScale = startSca;
        go.transform.rotation = startRot;
    }
    public override bool CheckCommand()
    {
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
