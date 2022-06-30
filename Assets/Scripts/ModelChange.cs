using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;
using UnityEngine.UI;
public class ModelChange : MonoBehaviour
{
    public GameObject Target;//Ҫ������Ŀ������
    public GameObject Brush;//Ҫ�����ĸ�����
    GameObject newModle;//�������ģ��
    CSG csgModle;
    void Start()
    {
    }
    //�󲢼�
    public void getUnion()
    {
        //getTarget();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Union);//�󽻼��Ľ��
        CreateNewObj();
    }
    //��
    public void getSubtract()
    {
        //getTarget();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Subtract);//���Ľ��
        CreateNewObj();
    }
    //�󽻼�
    public void getIntersection()
    {
        //getTarget();
        CSG.EPSILON = 1e-5f;
        csgModle = getCsg(CSG.Operation.Intersection);//�󽻼��Ľ��
        CreateNewObj();
    }
    //�����ť����ѡȡĿ��
    void getTarget()
    {

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
    }
    void CreateNewObj()
    {
        if (newModle != null)
        {
            Destroy(newModle);
        }
        //���ɲ������ģ��
        newModle = csgModle.PerformCSG();
        newModle.name = "newModle";
        newModle.transform.SetParent(transform.parent);
        csgModle.Target = newModle;
        //Brush.SetActive(true);
    }    
}
