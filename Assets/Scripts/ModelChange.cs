using MeshMakerNamespace;
using System.Collections.Generic;
using UnityEngine;
public class ModelChange : SceneSingleton<ModelChange>
{
    public GameObject freeExpPos;//����ƴ�����
    public GameObject Target;//Ҫ������Ŀ������
    public GameObject Brush;//Ҫ�����ĸ�����
    private bool getB;

    private bool mcI;
    private bool mcU;
    private bool mcS;

    GameObject newModle;//�������ģ��
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
    //�󲢼�
    public void getUnion()
    {
        Target = null;
        Brush = null;
        getB = true;
        mcU = true;
    }
    //��
    public void getSubtract()
    {
        Target = null;
        Brush = null;
        getB = true;
        mcS = true;
    }
    //�󽻼�
    public void getIntersection()
    {
        Target = null;
        Brush = null;
        getB = true;
        mcI = true;
        
    }




    //�����ť����ѡȡĿ��
    void getTarget()
    {
        
        if (getB)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = CastRay();//������Ļ��������
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
        //�����߳����������������
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, expCamera.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, expCamera.nearClipPlane);

        Vector3 far = expCamera.ScreenToWorldPoint(screenFar);
        Vector3 near = expCamera.ScreenToWorldPoint(screenNear);

        RaycastHit hit;
        Physics.Raycast(near, far - near, out hit);
        return hit;
    }

    //���β�������
    CSG getCsg(CSG.Operation OperationType)
    {
        CSG csg;
        csg = new CSG();
        csg.Brush = Brush;
        csg.Target = Target;
        csg.OperationType = OperationType;
        csg.customMaterial = new Material(Shader.Find("Standard")); // ����
        csg.useCustomMaterial = false; // ʹ������Ĳ���������п�
        csg.hideGameObjects = true; // ����������Ŀ��ͻ��ʶ���
        csg.keepSubmeshes = true; // ����ԭʼ������Ͳ���
        return csg;
    }

    void Update()
    {
        if (mcS)
        {
            getTarget();
            /* CSG.EPSILON = 1e-5f;
             csgModle = getCsg(CSG.Operation.Subtract);//���Ľ��
             CreateNewObj();*/
        }
        else if (mcI)
        {
            getTarget();

            CSG.EPSILON = 1e-5f;
            csgModle = getCsg(CSG.Operation.Intersection);//�󽻼��Ľ��
            CreateNewObj();
        }
        else if (mcU)
        {
            getTarget();
            CSG.EPSILON = 1e-5f;
            csgModle = getCsg(CSG.Operation.Union);//�󽻼��Ľ��
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
            //���ɲ������ģ��
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
        Debug.Log("ʵ������" + command.ToString());
    }
    public void subtract()
    {
        Command command = new SubtractCommand(Target, Brush);
        m_UndoStack.Push(command);
        command.Execute();
        Debug.Log("��" + command.ToString());
    }

    public void undo()
    {
        if (m_UndoStack.Count == 0)
        {
            return;
        }
     
        foreach (var c in m_UndoStack)
        {
            Debug.Log("�������" + c.ToString());
        }
        Debug.Log("---------");
        Command command = m_UndoStack.Pop();
        m_RedoStack.Push(command);
        command.Undo();
    }
}
