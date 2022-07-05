using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Command : MonoBehaviour
{
    //ÃüÁîÃèÊö
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

    //Ö´ÐÐÃüÁî
    public virtual void Execute() { }
    //³·ÏúÃüÁî
    public virtual void Undo() { }
}
