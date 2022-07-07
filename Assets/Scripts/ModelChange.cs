using MeshMakerNamespace;
using System.Collections.Generic;
using UnityEngine;
public class ModelChange : SceneSingleton<ModelChange>
{
    public GameObject freeExpPos;//自由拼搭父对象

    [SerializeField]
    private GameObject Target;//要操作的目标物体
    [SerializeField]
    private GameObject Brush;//要操作的副物体

    private bool getB;

    public Camera expCamera;

    private List<Command> m_RedoList;
    [HideInInspector]
    public List<Command> M_RedoList
    {
        set
        {
            m_RedoList.Add(value[0]);
        }
        get
        {
            return m_RedoList;
        }
    }

    private List<Command> m_UndoList;
    [HideInInspector]
    public List<Command> M_UndoList
    {
        set
        {
            m_UndoList.Add(value[0]);
        }
        get
        {
            return m_UndoList;
        }
    }

    void Start()
    {
        getB = false;
        m_UndoList = new List<Command>();
        m_RedoList = new List<Command>();
    }


    public void GetTarget()
    {
        CleanList();
        DisOutline();
        Target = null;
        Brush = null;
        getB = !getB;

        //让其他按钮不可操作
        if (getB)
        {
            Drag.instance.enabled = false;
        }
        else
        {
            Drag.instance.enabled = true;
        }

        
    }

    //点击按钮后点击选取目标
    void GetTarget(bool f)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCast.instance.CastRay(expCamera);//创建屏幕发射射线
            if (Target == null)
            {
                if (hit.collider != null)
                {
                    Target = hit.collider.gameObject;
                    Target.GetComponent<Outline>().enabled = true;
                }
            }
            else if (Brush == null)
            {
                if (hit.collider != null && hit.collider.gameObject != Target)
                {
                    Brush = hit.collider.gameObject;
                    Brush.GetComponent<Outline>().enabled = true;
                }
            }
            if (Target != null && Brush != null)
            {
                getB = f;
                Drag.instance.enabled = true;
            }
        }
    }

    void Update()
    {
        if (getB)
        {
            GetTarget(false);
        }


    }

    //实例化
    public void Instantiate(GameObject prefab)
    {
        CleanList();
        Command command = new InstantiateCommand(prefab, freeExpPos);
        command.Execute();
        //m_UndoStack.Push(command);
        m_UndoList.Add(command);
        Debug.Log("实例化：" + command.ToString());
    }

    //求差
    public void Subtract()
    {
        DisOutline();
        Command command = new SubtractCommand(Target, Brush);
        m_UndoList.Add(command);
        //m_UndoStack.Push(command);
        command.Execute();
        Debug.Log("求差：" + command.ToString());
    }

    //求并
    public void Union()
    {
        DisOutline();
        Command command = new UnionCommand(Target, Brush);
        m_UndoList.Add(command);
        //m_UndoStack.Push(command);
        command.Execute();
        Debug.Log("求并：" + command.ToString());
    }

    //求交
    public void Intersection()
    {
        DisOutline();
        Command command = new IntersectionCommand(Target, Brush);
        m_UndoList.Add(command);
        //m_UndoStack.Push(command);
        command.Execute();
        Debug.Log("求交：" + command.ToString());
    }

    //取消所有描边
    void DisOutline()
    {
        if (Target != null)
        {
            Target.GetComponent<Outline>().enabled = false;
        }
        if (Brush != null)
        {
            Brush.GetComponent<Outline>().enabled = false;
        }
    }

    //撤销
    public void Undo()
    {
        if(m_UndoList.Count == 0)
        {
            return;
        }
        /*
        foreach (var c in m_UndoStack)
        {
            Debug.Log("撤销命令列表：" + c.ToString());
        }*/
        //Debug.Log("---------");

        Command command = m_UndoList[m_UndoList.Count - 1];
        m_UndoList.RemoveAt(m_UndoList.Count - 1);
        if (command.CheckCommand())
        {
            command.Undo();
            m_RedoList.Add(command);
        }

        //Debug.Log("此次撤销：" + command.ToString());
    }

    //重做
    public void Redo()
    {

        if (m_RedoList.Count == 0)
        {
            return;
        }

        Command command = m_RedoList[m_RedoList.Count - 1];
        m_RedoList.RemoveAt(m_RedoList.Count - 1);
        if (command.CheckCommand())
        {
            command.Redo();
            m_UndoList.Add(command);
        }
    }

    //当重做列表中还有指令时  进行新的操作  删除重做列表指令
    public void CleanList()
    {
        //Debug.Log(m_RedoStack.Count);
        if (m_RedoList.Count != 0)
        {
            foreach (var c in m_RedoList)
            {
                c.DestroyModel();
            }
            m_RedoList.Clear();
        }

        /*
        foreach (var c in m_RedoStack)
        {
            Debug.Log("重做：" + c.ToString());
        }*/
    }
}
