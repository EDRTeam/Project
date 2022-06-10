using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotateController : SceneSingleton<ObjRotateController>
{
    public ObjRotate[] objRotates;

    public float rotateSpeed = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        objRotates = FindObjectsOfType<ObjRotate>(true);

        foreach (var modelRotates in objRotates)
        {
            modelRotates.Initialize();
        }

    }

    public void RecoverTransform()
    {
        foreach (var modelRotates in objRotates)
        {
            modelRotates.RecoverRotation();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
