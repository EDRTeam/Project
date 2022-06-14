using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHighLight : MonoBehaviour
{
    public GameObject cur_Obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();//������Ļ��������
            if (hit.collider != null)
            {
                //��⵽�����岻������ģ��
                if (!hit.collider.CompareTag("HighLight"))
                {   
                    //��ǰ��ѡ������ ������ʾ��
                    if (cur_Obj != null)
                    {
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
                        cur_Obj = null;
                    }   
                    return;
                }else if (hit.collider.CompareTag("HighLight"))
                {
                    //��ǰû��ѡ������ ����⵽�����������ʾ
                    if (cur_Obj == null)
                    {
                        cur_Obj = hit.collider.gameObject;
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 1);
                    }
                    //��ѡ�����岢�Ҳ����¼�⵽����
                    if (hit.collider.gameObject != cur_Obj)
                    {
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
                        cur_Obj = hit.collider.gameObject;
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 1);
                    }
                    else return;
                }
            }
            else
            {
                //û�м�⵽���� 
                if (cur_Obj != null && cur_Obj.CompareTag("HighLight"))
                {
                    cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
                    cur_Obj = null;
                }
                return;
            }
        }*/
    }

    private RaycastHit CastRay()
    {
        //�����߳����������������
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 far = Camera.main.ScreenToWorldPoint(screenFar);
        Vector3 near = Camera.main.ScreenToWorldPoint(screenNear);

        RaycastHit hit;
        Physics.Raycast(near, far - near, out hit);
        return hit;
    }

    public void HighLightOn()
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 1);
    }

    public void HighLightOff()
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
    }
}