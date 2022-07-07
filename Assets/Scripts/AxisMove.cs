using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMove : SceneSingleton<AxisMove>
{
    public bool check = false;//�л����

    public Transform axis;  //������ģ��
    private Material selectMat;
    [SerializeField]
    private Transform target;  //Ҫ�ƶ�������
    public Camera axisCamera; //ֻ��Ⱦ������������

    public float MOVE_SPEED = 0.03F;

    private int currentAxis = 0;//��ǰҪ�ƶ����� �� 1 2 3��ʶ x y z�� 0��ʾû��ѡ��������
    private bool choosedAxis = false;//�ж��Ƿ�ѡ����������
    private Vector3 oldPos; //��һ֡���λ��
    private Vector3 offset;
    private Vector3 screenPosition;

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
            Drag.instance.enabled = true;
            this.enabled = false;
        }
        if (target == null)
        {
            axis.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCast.instance.CastRay(axisCamera);//������Ļ��������
            if (hit.collider != null && target == null)
            {
                target = hit.collider.gameObject.transform;
                axis.gameObject.SetActive(true);
                axis.position = target.position;
            }else if (hit.collider == null)
            {
                axis.gameObject.SetActive(false);
                target = null;
            }
        }
        if (target != null)
        {
            //��갴��
            if (Input.GetMouseButtonDown(0))
            {
                AxisCheck();
            }
            //��곤�������ڰ������ʱѡ��������
            if (Input.GetMouseButton(0) && choosedAxis)
            {
                //�ƶ�����
                UpdateCubePosition();
            }

            //���̧����ʼ����ֵ 
            if (Input.GetMouseButtonUp(0))
            {
                choosedAxis = false;
                currentAxis = 0;
                if (selectMat != null)
                {
                    selectMat.SetFloat("_IsSelected", 0);
                }
                
            }
        }
        
    }

    private void AxisCheck()
    {
        //�������Ƿ���ײ��������
        Ray ray = axisCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, axisCamera.farClipPlane, 1 << LayerMask.NameToLayer("Axis"))) //ֻ���Axis��һ��
        {
            
            choosedAxis = true;
            oldPos = target.position;
            screenPosition = axisCamera.WorldToScreenPoint(oldPos);
            offset = axis.position - axisCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            //��ײ�ɹ��� ��¼���λ��
            //oldPos = Input.mousePosition;
            selectMat = hit.collider.gameObject.GetComponent<Renderer>().materials[0];
            selectMat.SetFloat("_IsSelected", 1);
            //�жϵ�ǰѡ���������
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
    //��ʼ�ƶ�����
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
