using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InstantiateCommand : Command
{
    private GameObject prefab;
    private GameObject fep;
    private GameObject instance;

    public InstantiateCommand(GameObject prefab,GameObject fep)
    {
        this.prefab = prefab;
        this.fep = fep;
    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (isDrag)
        {
            //刷新位置
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
    //按下鼠标时开始生成实体预制件
    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
        dragObj = Instantiate(prefab);
    }*/

    public override void Execute()
    {
        base.Execute();
        instance = GameObject.Instantiate(prefab, fep.transform);
        instance.transform.localPosition = Vector3.zero;
    }

    public override void Undo()
    {
        base.Undo();
        DestroyImmediate(instance);
    }
    public override string ToString()
    {
        string s = prefab.ToString();
        return s;
    }
}
