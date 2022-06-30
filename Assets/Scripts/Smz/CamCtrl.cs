using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    public Transform CenObj;//围绕的物体
    private Vector3 Rotion_Transform;
    public Camera camera;
    void Start()
    {
        Rotion_Transform = CenObj.position;
    }
    void Update()
    {
        Ctrl_Cam_Move();
        Cam_Ctrl_Rotation();
        Cam_Move();
    }
    //摄像机的移动
    public void Cam_Move()
    {

    }
    //镜头的远离和接近
    public void Ctrl_Cam_Move()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(Vector3.forward * 1f);//速度可调  自行调整
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(Vector3.forward * -1f);//速度可调  自行调整
        }
    }
    //摄像机的旋转
    public void Cam_Ctrl_Rotation()
    {
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.RotateAround(Rotion_Transform, Vector3.up, mouse_x * 5);
            transform.RotateAround(Rotion_Transform, transform.right, mouse_y * 5);
        }
    }
}
