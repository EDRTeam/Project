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
            //Ë¢ÐÂÎ»ÖÃ
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

    public override void Execute()
    {
        base.Execute();
        instance = GameObject.Instantiate(prefab, fep.transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.Translate(Vector3.up * 0.5f * instance.transform.localScale.y, Space.Self);
    }

    public override void Undo()
    {
        base.Undo();
        instance.SetActive(false);
    }

    public override void Redo()
    {
        base.Redo();
        
        instance.SetActive(true);
    }

    public override bool CheckCommand()
    {
        if (instance == null)
        {
            //ModelChange.instance.M_RedoList.Remove(this);
            Destroy(this);
            return false;
        }
        return base.CheckCommand();
    }
    public override void DestroyModel()
    {
        DestroyImmediate(instance);
    }

    public override string ToString()
    {
        string s = prefab.ToString();
        return s;
    }
}
