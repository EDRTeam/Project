using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Drag : SceneSingleton<Drag>
{
    public Camera fxpCamera;

    private GameObject currentDrag;
    [HideInInspector]
    public GameObject CurrentDrag
    {
        get
        {
            return currentDrag;
        }
    }

    private Vector3 screenSpace;
    private Vector3 offset;

    private bool inDesFlag;
    public GameObject recycleUiLight;
    //private List<Command> commandList;
    private Command command;
    public void Start()
    {
        inDesFlag = false;
        AxisMove.instance.enabled = false;
        //commandStack = new Stack<Command>();
        //StopAllCoroutines();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AxisMove.instance.check = !AxisMove.instance.check;
            AxisMove.instance.enabled = true;
            AxisMove.instance.axisPanel.SetActive(true);
            this.enabled = false;
        }
        if (!AxisMove.instance.check)
        {
            AxisMove.instance.axisPanel.SetActive(false);
            AxisMove.instance.enabled = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCast.instance.CastRay(fxpCamera);//创建屏幕发射射线
            if (hit.collider != null && currentDrag == null)
            {
                currentDrag = hit.collider.gameObject;
                screenSpace = fxpCamera.WorldToScreenPoint(currentDrag.transform.position);//三维物体坐标转屏幕坐标
                //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离
                offset = currentDrag.transform.position - fxpCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                command = new TransformCommand(currentDrag);
                //commandStack.Push(command);
                ModelChange.instance.CleanList();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (currentDrag != null)
            {
                Move();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            List<Command> commandList = new List<Command>(1);
            if (currentDrag != null)
            {
                //记录物体最后的transform信息
                command.Execute();
                //拖动物体到回收站直接删除对象
                if (inDesFlag)
                {
                    Destroy(currentDrag);
                    inDesFlag = false;
                    recycleUiLight.SetActive(false);
                }
                else
                {
                    if (command.CheckCommand())
                    {
                        commandList.Add(command);
                        ModelChange.instance.M_UndoList = commandList;
                    }
                    
                }
            }
            currentDrag = null;
            commandList.Clear();
            //commandStack.Clear();
        }
    }

    public void Move()
    {
        //Debug.Log("move:" + currentDrag.name);
        Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        var curPosition = fxpCamera.ScreenToWorldPoint(curScreenSpace) + offset;
        currentDrag.transform.position = curPosition;
        //Debug.Log(curPosition);
    }

    public void InUIDestroy()
    {
        //Debug.Log("in");
        if (currentDrag != null)
        {
            recycleUiLight.SetActive(true);
        }
        inDesFlag = true;
    }
    public void OutUIDestroy()
    {
        inDesFlag = false;
        recycleUiLight.SetActive(false);
    }


}