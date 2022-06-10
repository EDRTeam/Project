using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ObjRotate : MonoBehaviour
{
    //private GameObject cur_Obj;
    private Quaternion originQuaternion;    //记录初始位置信息，用于复原

    public float rotateSpeed;
    public bool canRotate;     //判断物体是否可以旋转

    public void Initialize() 
    {
        originQuaternion = transform.localRotation;
        rotateSpeed = ObjRotateController.instance.rotateSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {

    }
    private void OnMouseDrag()
    {
        if (!canRotate) return;

        Vector3 mouseOffset = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
        mouseOffset *= rotateSpeed * 10;
        transform.Rotate(mouseOffset, Space.World);
    }
    private void OnMouseUp()
    {

    }

    public void RecoverRotation()
    {
        transform.DOLocalRotateQuaternion(originQuaternion, 1.5f);
    }
}
