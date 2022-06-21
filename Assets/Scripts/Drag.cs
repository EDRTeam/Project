using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Drag : MonoBehaviour
{
    public IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//三维物体坐标转屏幕坐标
        //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }
    }
}