using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//大场景操作摄像机
public class CameraController : SceneSingleton<CameraController>
{
    public Camera camera;
    //public GameObject center;      //旋转中心
    public Vector3 center;

    public float rotateSpeed;       //旋转速度
    public float translateSpeed;    //摄像机平移速度
    public float scaleSpeed;        //缩放视角速度

    public Vector2 scaleLimit;      //缩放视角限制  x min  y max
    //public Vector2 minLimit;  //平移限制   对于大场景应与缩略图预览范围一致
    //public Vector2 maxLimit;

    //public bool canRotate;

    public Vector3 originPosition;  //初始位置
    public float originFov;     //初始缩放
    public Quaternion originQuaternion;   //初始位置-角度

    public bool check;      //切换操作方式
    // Start is called before the first frame update
    void Start()
    {
        originPosition = camera.transform.position;
        originFov = camera.fieldOfView;
        originQuaternion = camera.transform.localRotation;
        check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            CameraMove();
            CameraTransform();
        }

    }

    public void SetCenter()
    {
        /*if (center == null)
        {
            center = new GameObject();
        }*/
        center = camera.transform.position + camera.transform.forward * 10;
        //center.transform.position = camera.transform.position + camera.transform.forward * 10;
    }

    public void OnMouseDrag()
    {
        /*Vector3 mouseOffset = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
        mouseOffset *= rotateSpeed;
        camera.transform.Rotate(mouseOffset, Space.World);*/
        /*camera.transform.RotateAround(center.transform.position, Vector3.up, Input.GetAxis("Mouse X") * translateSpeed * Time.deltaTime*1000);
        camera.transform.RotateAround(center.transform.position, camera.transform.right, Input.GetAxis("Mouse Y") * translateSpeed * Time.deltaTime);
        Quaternion temp = camera.transform.rotation;
        temp.z = 0;
        camera.transform.rotation = temp;
        Debug.Log(camera.transform.rotation);*/
        var mouse_x = Input.GetAxis("Mouse X");
        var mouse_y = Input.GetAxis("Mouse Y");
        if (Input.GetKey(KeyCode.Mouse1))   //鼠标右键平移  
        {
            camera.transform.Translate(Vector3.left * mouse_x * translateSpeed * Time.deltaTime * 1000);
            camera.transform.Translate(-Vector3.up * mouse_y * translateSpeed * Time.deltaTime * 1000);
        }

        if (Input.GetKey(KeyCode.Mouse0))    //鼠标左键旋转   绕场景中心
        {
            //camera.transform.RotateAround(center.transform.position, Vector3.up, mouse_x * rotateSpeed * Time.deltaTime * 1000);
            //camera.transform.RotateAround(center.transform.position, -camera.transform.right, mouse_y * rotateSpeed * Time.deltaTime * 1000);
            camera.transform.RotateAround(center, Vector3.up, mouse_x * rotateSpeed * Time.deltaTime * 1000);
            camera.transform.RotateAround(center, -camera.transform.right, mouse_y * rotateSpeed * Time.deltaTime * 1000);
            Quaternion temp = camera.transform.rotation;
            Debug.Log(temp+"1");
            temp.z = 0;
            Debug.Log(temp);
            camera.transform.rotation = temp;
        }
    }

    public void CameraTransform()
    {
        var mouse_x = Input.GetAxis("Mouse X");
        var mouse_y = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(1))   //鼠标右键平移  
        {
            camera.transform.Translate(Vector3.left * mouse_x * translateSpeed * Time.deltaTime * 1000);
            camera.transform.Translate(-Vector3.up * mouse_y * translateSpeed * Time.deltaTime * 1000);
        }

        if (Input.GetMouseButton(0))    //鼠标左键旋转   绕场景中心
        {
            if (Input.GetMouseButtonDown(0))
            {
                center = camera.transform.position + camera.transform.forward * 10;
            }
            //camera.transform.RotateAround(center.transform.position, Vector3.up, mouse_x * rotateSpeed * Time.deltaTime * 1000);
            //camera.transform.RotateAround(center.transform.position, -camera.transform.right, mouse_y * rotateSpeed * Time.deltaTime * 1000);
            camera.transform.RotateAround(center, Vector3.up, mouse_x * rotateSpeed * Time.deltaTime * 1000);
            camera.transform.RotateAround(center, -camera.transform.right, mouse_y * rotateSpeed * Time.deltaTime * 1000);
            Quaternion temp = camera.transform.rotation;
            Debug.Log(temp + "1");
            temp.z = 0;
            Debug.Log(temp);
            camera.transform.rotation = temp;
        }
    }

    //滚轮控制相机FOV实现缩放效果
    public void CameraMove()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheel) > 0.01)
        {
            float fov = camera.fieldOfView;
            fov -= scrollWheel * Time.deltaTime * scaleSpeed *1000;
            if(fov > scaleLimit.x && fov < scaleLimit.y)
            {
                camera.fieldOfView = fov;
            }
        }
    }

    //重置摄像机状态
    public void RecoverTransform(Vector3 offset)
    {
        camera.transform.DOLocalMove(originPosition + offset, 1f);
        camera.transform.DOLocalRotateQuaternion(originQuaternion, 1f);
        camera.DOFieldOfView(originFov,1f);
    }
}
