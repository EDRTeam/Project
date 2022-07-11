using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    public Transform Target
    {
        set
        {
            _target = value;
        }
        get
        {
            return _target;
        }
    }

    public Slider XSca;
    public Slider YSca;
    public Slider ZSca;

    public InputField xIf;
    public InputField yIf;
    public InputField zIf;

    private Command command;
    private List<Command> commandList = new List<Command>();

    public void Init(Transform target)
    {
        _target = target;
        
        if (_target != null)
        {
            Vector3 scale = _target.localScale;
            //Debug.Log(_target.gameObject.name);
            //Debug.Log("设置滑条前" + _target.localScale);
            XSca.value = scale.x;

            //Debug.Log("设置滑条x" + _target.localScale);
            //Debug.Log(_target.gameObject.name);
            YSca.value = scale.y;
            //Debug.Log("设置滑条y" + _target.localScale);
            //Debug.Log(_target.gameObject.name);

            ZSca.value = scale.z;

            //Debug.Log("设置滑条后" + _target.localScale);
            ModelChange.instance.CleanList();

            command = new ScaleCommand(_target.gameObject, XSca, YSca, ZSca);

        }
        else
        {
            XSca.value = 1;
            YSca.value = 1;
            ZSca.value = 1;
            
        }
    }
    public void UpdateObjectScale()
    {
        //判断物体切换 更新
        if (_target != null)
        {
            //Debug.Log("缩放");
            Vector3 newScale = new Vector3(XSca.value, YSca.value, ZSca.value);
            _target.localScale = newScale;
        }
    }

    public void AddCommand()
    {
        //Debug.Log(_target.localScale);
        command.Execute();
        if (command.CheckCommand()&&_target != null){
            commandList.Add(command);
            ModelChange.instance.M_UndoList = commandList;
            commandList.Clear();
        }
    }

    public void CreateCommand()
    {
        if (_target != null)
        {
            Vector3 scale = _target.localScale;
            ModelChange.instance.CleanList();
            command = new ScaleCommand(_target.gameObject, XSca, YSca, ZSca);
        }
    }

    public void InputFtoChange()
    {
        if (_target != null)
        {
            XSca.value = Clamp(XSca.maxValue, float.Parse(xIf.text));
            YSca.value = Clamp(YSca.maxValue, float.Parse(yIf.text));
            ZSca.value = Clamp(ZSca.maxValue, float.Parse(zIf.text));
        }
    }

    private float Clamp(float max, float input)
    {
        if (input > max)
        {
            return max;
        }
        return input;
    }
}
