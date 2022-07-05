using MeshMakerNamespace;
using System.Collections.Generic;
using UnityEngine;
public class ModelChange : SceneSingleton<ModelChange>
{
    public GameObject freeExpPos;//自由拼搭父对象
    public GameObject Target;//要操作的目标物体
    public GameObject Brush;//要操作的副物体
    private bool getB;

    private bool mcI;
    private bool mcU;
    private bool mcS;

    GameObject newModle;//操作后的模型
    CSG csgModle;

    public Camera expCamera;

    private Stack<Command> m_RedoStack;
    private Stack<Command> m_UndoStack;

    void Start()
    {
        getB = false;

        m_UndoStack = new Stack<Command>();
        m_RedoStack = new Stack<Command>();
    }
    //求并集
    public void getUnion()
    {
        Target = null;
        Brush = null;
        getB = true;
        mcU = true;
    }
    //求差集
    public void getSubtract()
    {
        Target = null;
        Brush = null;
        getB = true;
        mcS = true;
    }
    //求交集
    public void getIntersection()
    {
        Target = null;
        Brush = null;
        getB = true;
        mcI = true;
        
    }




    //点击按钮后点击选取目标
    void getTarget()
    {
        
        if (getB)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = CastRay();//创建屏幕发射射线
                if (Target == null)
                {
                    if (hit.collider != null)
                    {
                        Target = hit.collider.gameObject;
                    }
                }
                else if (Brush == null)
                {
                    if (hit.collider != null && hit.collider.gameObject != Target)
                    {
                        Brush = hit.collider.gameObject;
                    }
                }
                if (Target != null && Brush != null)
                {
                    getB = false;
                    mcI = false;
                    mcU = false;
                    mcS = false;
                }
            }
        }
    }

    private RaycastHit CastRay()
    {
        //将射线长度限制在摄像机内
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, expCamera.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, expCamera.nearClipPlane);

        Vector3 far = expCamera.ScreenToWorldPoint(screenFar);
        Vector3 near = expCamera.ScreenToWorldPoint(screenNear);

        RaycastHit hit;
        Physics.Raycast(near, far - near, out hit);
        return hit;
    }

    //几何布尔运算
    CSG getCsg(CSG.Operation OperationType)
    {
        CSG csg;
        csg = new CSG();
        csg.Brush = Brush;
        csg.Target = Target;
        csg.OperationType = OperationType;
        csg.customMaterial = new Material(Shader.Find("Standard")); // 材质
        csg.useCustomMaterial = false; // 使用上面的材质来填充切口
        csg.hideGameObjects = true; // 操作后隐藏目标和画笔对象
        csg.keepSubmeshes = true; // 保持原始的网格和材质
        return csg;
    }

    void Update()
    {
        if (mcS)
        {
            getTarget();
            /* CSG.EPSILON = 1e-5f;
             csgModle = getCsg(CSG.Operation.Subtract);//求差集的结果
             CreateNewObj();*/
        }
        else if (mcI)
        {
            getTarget();

            CSG.EPSILON = 1e-5f;
            csgModle = getCsg(CSG.Operation.Intersection);//求交集的结果
            CreateNewObj();
        }
        else if (mcU)
        {
            getTarget();
            CSG.EPSILON = 1e-5f;
            csgModle = getCsg(CSG.Operation.Union);//求交集的结果
            CreateNewObj();
        }
        
    }

    void CreateNewObj()
    {
        if (Target != null && Brush != null)
        {
            if (newModle != null)
            {
                Destroy(newModle);
            }
            //生成操作后的模型
            newModle = csgModle.PerformCSG();
            newModle.name = "newModle";
            newModle.transform.SetParent(Target.transform.parent);
            newModle.AddComponent<BoxCollider>();
            csgModle.Target = newModle;
            Destroy(Target);
            Destroy(Brush);
            //Target = newModle;
        }
        //Brush.SetActive(true);
    }

    public void Instantiate(GameObject prefab)
    {
        Command command = new InstantiateCommand(prefab, freeExpPos);
        command.Execute();
        m_UndoStack.Push(command);
        Debug.Log("实例化：" + command.ToString());
    }
    public void subtract()
    {
        Command command = new SubtractCommand(Target, Brush);
        m_UndoStack.Push(command);
        command.Execute();
        Debug.Log("求差：" + command.ToString());
    }

    public void undo()
    {
        if (m_UndoStack.Count == 0)
        {
            return;
        }
     
        foreach (var c in m_UndoStack)
        {
            Debug.Log("撤销命令：" + c.ToString());
        }
        Debug.Log("---------");
        Command command = m_UndoStack.Pop();
        m_RedoStack.Push(command);
        command.Undo();
    }
}
