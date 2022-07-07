using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Command : MonoBehaviour
{
    //��������
    private string _commandDescribe;
    public string ComandDescribe
    {
        set
        {
            _commandDescribe = value;
        }
        get
        {
            return _commandDescribe;
        }
    }

    //ִ������
    public virtual void Execute() { }
    //��������
    public virtual void Undo() { }

    public virtual void Redo() { }

    public virtual bool CheckCommand() { return true; }
    public virtual void DestroyModel() { }
}
