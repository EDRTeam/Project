using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.UI;

//预览缩略图的窗口移动
public class PreviewerController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public RectTransform[] viewer;    //预览缩略图rectTransform
    //public RectTransform viewer_in; //小框rectTransform
    //public RectTransform[] viewer_z;
    
    Vector3 startPos;               //点击缩略图起始点
    Vector3 viewerCenter;           
    Vector3 offset = Vector3.zero;  //位置偏移量  

    bool canMove = true;            //鼠标位置 相机能否移动

    float minx, maxx, miny, maxy;   //移动限制

    public float camMaxPosX;         //相机在能看到场景最边界时的x位置
    public float camCenPosX;         //相机看到场景中心时的x位置
    public float camMaxPosY;
    public float camCenPosY;
    private Vector2 moveFactor;        //这个值应该根据相机看到场景边界时的位置-相机看到场景中心时的位置/小框在缩略图最左（右）时中心的位置-小框在缩略图中心时的中心位置
    // Start is called before the first frame update
    void Start()
    {
        
        //viewer = GetComponent<RectTransform>();
        //viewer_in = gameObject.GetComponentInChildren<RectTransform>();
        viewer = gameObject.GetComponentsInChildren<RectTransform>();

        viewer[1].position = viewer[0].position;   //让两个中心重合
        viewerCenter = viewer[1].position;         //记录起始位置

        SetDragRange();

        viewer[1].gameObject.GetComponent<RawImage>().DOFade(0f, 0.5f);

        moveFactor.x = (camMaxPosX - camCenPosX) / (maxx - viewerCenter.x);
        moveFactor.y = (camMaxPosY - camCenPosY) / (maxy - viewerCenter.y);
        Debug.Log(moveFactor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(viewer[1], eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos)){
            
            viewer[1].gameObject.GetComponent<RawImage>().DOFade(0.3f, 0.5f);
            //offset = viewer[1].position - globalMousePos;
            startPos = globalMousePos;
            offset = (DragRangeLimit(globalMousePos) - viewerCenter)*moveFactor;
            //Debug.Log("gl:" + globalMousePos);
            //Debug.Log("gld:" + DragRangeLimit(globalMousePos));
            //Debug.Log("pos:" + viewer[1].position);
            
            CameraController.instance.RecoverTransform(offset);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(viewer[1], eventData.position,eventData.enterEventCamera,out Vector3 globalMousePos))
        {
            viewer[1].position = DragRangeLimit(globalMousePos);
            //Debug.Log("gl2:" + globalMousePos);
            //计算摄像机移动距离 移动摄像机
            if (canMove)
            {
                var camOffset = viewer[1].position - viewerCenter;
                Debug.Log(camOffset);
                camOffset.x *= moveFactor.x;
                camOffset.y *= moveFactor.y;
                Debug.Log(camOffset);
                CameraController.instance.camera.transform.position = camOffset;
                //CameraController.instance.camera.transform.position = new Vector3(camOffset.x,camOffset.y, CameraController.instance.camera.transform.position.z);
                //CameraController.instance.camera.transform.position += (viewer[1].position - startPos) * Time.deltaTime * moveFactor;
            }
            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        viewer[1].gameObject.GetComponent<RawImage>().DOFade(0f, 0.5f);
    }

    //限制范围
    void SetDragRange()
    {
        // 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
        minx = viewer[0].position.x - viewer[0].pivot.x * viewer[0].rect.width + viewer[1].rect.width * viewer[0].pivot.x;
        // 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
        maxx = viewer[0].position.x +(1- viewer[0].pivot.x) * viewer[0].rect.width - viewer[1].rect.width * (1-viewer[0].pivot.x);
        // 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
        miny = viewer[0].position.y - viewer[0].pivot.y * viewer[0].rect.height + viewer[1].rect.height * viewer[0].pivot.y;
        // 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
        maxy = viewer[0].position.y + (1 - viewer[0].pivot.y) * viewer[0].rect.height - viewer[1].rect.height * (1 - viewer[0].pivot.y);
        //Debug.Log("minx:" + minx);
        //Debug.Log("masx:" + maxx);
        //Debug.Log("miny:" + miny);
        //Debug.Log("maxy:" + maxy);
    }

    private Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minx, maxx);
        pos.y = Mathf.Clamp(pos.y, miny, maxy);
        if (pos == new Vector3(minx, miny, 0) || pos == new Vector3(minx, maxy, 0) || pos == new Vector3(maxx, miny) || pos == new Vector3(maxx, maxy))
        {
            canMove = false;
        }
        else canMove = true;
        
        return pos;
    }
}
