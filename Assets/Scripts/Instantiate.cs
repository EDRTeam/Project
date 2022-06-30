using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Instantiate : MonoBehaviour
{
    public GameObject Target;
    public Vector3 Pos;
    public Quaternion Rot;
    //Ҫ���ɵ�����
    public GameObject prefab;
    //��ק������
    public GameObject dragObj;
    //�Ƿ�������ק
    public bool isDrag;
    void Start()
    {
        Pos = Target.gameObject.transform.position;
        Rot = Target.gameObject.transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (isDrag)
        {
            //ˢ��λ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                dragObj.transform.position = hit.point;
                dragObj.SetActive(true);
            }
            else
            {
                dragObj.SetActive(false);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (!dragObj.activeSelf)
                {
                    Destroy(dragObj);
                }
                isDrag = false;
                dragObj = null;
            }
        }*/
    }     
    /*
    //�������ʱ��ʼ����ʵ��Ԥ�Ƽ�
    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
        dragObj = Instantiate(prefab);
    }*/
    public void InstantiateL()
    {
        GameObject.Instantiate(prefab, Pos, Rot);
    }
    public GameObject Cube;
    public GameObject Sphere;
    public void Cube1()
    {
        Cube.gameObject.SetActive(true);
    }
    public void Sphere1(){
        Sphere.gameObject.SetActive(true);
    }
}
