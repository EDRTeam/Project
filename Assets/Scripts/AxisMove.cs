using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMove : SceneSingleton<AxisMove>
{
    public bool check = false;//切换检测
    public RotationController rc;
    public ScaleController sc;

    public Transform axis;  //坐标轴模型
    private Material selectMat;
    [SerializeField]
    private Transform target;  //要移动的物体
    public Transform Target
    {
        get
        {
            return target;
        }
    }
    public Camera axisCamera; //只渲染坐标轴的摄像机
    public Camera fepCamera;

    public float MOVE_SPEED = 0.03F;

    private int currentAxis = 0;//当前要移动的轴 用 1 2 3标识 x y z轴 0表示没有选择坐标轴
    private bool choosedAxis = false;//判断是否选择了坐标轴
    private Vector3 oldPos; //上一帧鼠标位置
    private Vector3 offset;
    private Vector3 screenPosition;

    public GameObject axisPanel;

    private Command command;
    void Start()
    {
        axis.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            check = !check;
            axis.gameObject.SetActive(false);
            axisPanel.SetActive(false);

            Drag.instance.enabled = true;
            target = null;

            rc.Init(null);
            sc.Init(null);

            this.enabled = false;
        }
        if (target == null)
        {
            axis.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCast.instance.CastRay(fepCamera);//创建屏幕发射射线

            if (hit.collider != null)
            {
                if (target == null||
                    (target.gameObject != hit.collider.gameObject && hit.collider.gameObject.layer!=LayerMask.NameToLayer("Axis")))
                {
                    target = hit.collider.gameObject.transform;
                    axis.gameObject.SetActive(true);
                    axis.position = target.position;
                }
            }
            else
            {
                axis.gameObject.SetActive(false);
                target = null;
            }
            rc.Init(target);
            sc.Init(target);


            //if (hit.collider != null && target == null)
            //{
            //    if (target == null)
            //    {
            //        target = hit.collider.gameObject.transform;
            //        axis.gameObject.SetActive(true);
            //        axis.position = target.position;
            //    }
            //    else if(target.gameObject != hit.collider.gameObject)
            //    {
            //        target = hit.collider.gameObject.transform;
            //        axis.position = target.position;
            //    }
            //}
            //else if (hit.collider == null)
            //{
            //    axis.gameObject.SetActive(false);
            //    target = null;
            //}
            //rc.Init(target);
            //sc.Init(target);
        }
        if (target != null)
        {
            //鼠标按下
            if (Input.GetMouseButtonDown(0))
            {
                AxisCheck();
            }
            //鼠标长按并且在按下鼠标时选中坐标轴
            if (Input.GetMouseButton(0) && choosedAxis)
            {
                //移动物体
                UpdateCubePosition();
            }

            //鼠标抬起后初始化数值 
            if (Input.GetMouseButtonUp(0))
            {
                if (choosedAxis)
                {
                    command.Execute();
                    List<Command> commandList = new List<Command>(1);
                    if (command.CheckCommand())
                    {
                        commandList.Add(command);
                        ModelChange.instance.M_UndoList = commandList;
                    }
                    commandList.Clear();
                }
                choosedAxis = false;
                currentAxis = 0;
                if (selectMat != null)
                {
                    selectMat.SetFloat("_IsSelected", 0);
                }
                selectMat = null;

            }
        }
        
    }

    private void AxisCheck()
    {
        //检测相机是否碰撞了坐标轴
        RaycastHit hit = RayCast.instance.CastRay(axisCamera, 1 << LayerMask.NameToLayer("Axis"));
        if (hit.collider!=null) //只检测Axis这一层
        {
            //Debug.Log("坐标轴:" + hit.collider.name);
            command = new TransformCommand(target.gameObject);
            choosedAxis = true;
            oldPos = target.position;
            screenPosition = axisCamera.WorldToScreenPoint(oldPos);
            offset = axis.position - axisCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            //碰撞成功后 记录鼠标位置
            //oldPos = Input.mousePosition;
            selectMat = hit.collider.gameObject.GetComponent<Renderer>().materials[0];
            selectMat.SetFloat("_IsSelected", 1);
            //判断当前选择的坐标轴
            switch (hit.collider.name)
            {
                case "x":
                    currentAxis = 1;
                    break;
                case "y":
                    currentAxis = 2;
                    break;
                case "z":
                    currentAxis = 3;
                    break;
                default:
                    break;
            }
        }
    }
    //开始移动物体
    private void UpdateCubePosition()
    {
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
        Vector3 currentPosition = axisCamera.ScreenToWorldPoint(currentScreenSpace) + offset;
        float tempLength = Vector3.Distance(currentPosition, axis.transform.position);
        Vector3 tempPos = target.transform.position;
        switch (currentAxis)
        {
            case 1:
                tempPos = Vector3.Project(currentPosition, axis.transform.right) + Vector3.Project(oldPos, axis.transform.up) + Vector3.Project(oldPos, axis.transform.forward);
                break;
            case 2:
                tempPos = Vector3.Project(currentPosition, axis.transform.up) + Vector3.Project(oldPos, axis.transform.right) + Vector3.Project(oldPos, axis.transform.forward);
                break;
            case 3:
                tempPos = Vector3.Project(currentPosition, axis.transform.forward) + Vector3.Project(oldPos, axis.transform.up) + Vector3.Project(oldPos, axis.transform.right);
                break;
        }
        target.transform.position = tempPos;
        axis.position = tempPos;
    }
}
