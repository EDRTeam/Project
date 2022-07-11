using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class RotationController : MonoBehaviour {

    public Slider XRot;
    public Slider YRot;
    public Slider ZRot;

    public InputField xIf;
    public InputField yIf;
    public InputField zIf;

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

    private Command command;
    private List<Command> commandList = new List<Command>();

    public void Init(Transform target)
    {
        _target = target;

        if (_target != null)
        {
            Vector3 rotation = GetInspectorRotationValueMethod(_target);
            XRot.value = rotation.x;
            YRot.value = rotation.y;
            ZRot.value = rotation.z;

            ModelChange.instance.CleanList();
            command = new RotationCommand(_target.gameObject, XRot, YRot, ZRot, rotation);

        }
        else
        {
            XRot.value = 0;
            YRot.value = 0;
            ZRot.value = 0;
        }

        //command = new TransformCommand(_target.gameObject);
    }

    //public Transform ControlledObject;
    public void UpdateObjectRotation(Transform ControlledObject)
    {
        Vector3 newRotation = new Vector3(XRot.value, YRot.value, ZRot.value);
        ControlledObject.rotation = Quaternion.Euler(newRotation);
    }

    public void UpdateObjectRotation()
    {
        if (_target != null)
        {
            Vector3 newRotation = new Vector3(XRot.value, YRot.value, ZRot.value);
            //Debug.Log(newRotation);
            _target.rotation = Quaternion.Euler(newRotation);
        }
    }

    public void AddCommand()
    {
        command.Execute();
        //Debug.Log(command.CheckCommand());
        if (command.CheckCommand()&&_target!=null)
        {
            commandList.Add(command);
            ModelChange.instance.M_UndoList = commandList;
            commandList.Clear();
        }
    }

    public Vector3 GetInspectorRotationValueMethod(Transform transform)
    {
        // 获取原生值
        System.Type transformType = transform.GetType();
        PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
        object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
        MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
        object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        string temp = value.ToString();
        //将字符串第一个和最后一个去掉
        temp = temp.Remove(0, 1);
        temp = temp.Remove(temp.Length - 1, 1);
        //用‘，’号分割
        string[] tempVector3;
        tempVector3 = temp.Split(',');
        //将分割好的数据传给Vector3
        Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
        return vector3;
    }

    public void CreateCommand()
    {
        if (_target != null)
        {
            Vector3 rotation = GetInspectorRotationValueMethod(_target);
            ModelChange.instance.CleanList();
            command = new RotationCommand(_target.gameObject, XRot, YRot, ZRot, rotation);
        }
    }

    public void InputFtoChange()
    {
        if (_target != null)
        {
            XRot.value = Clamp(XRot.maxValue, float.Parse(xIf.text));
            YRot.value = Clamp(YRot.maxValue, float.Parse(yIf.text));
            ZRot.value = Clamp(ZRot.maxValue, float.Parse(zIf.text));
        }
    }

    private float Clamp(float max, float input)
    {
        if (input > max){
            return max;
        }
        return input;
    }
}

