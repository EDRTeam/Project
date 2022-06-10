using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.UI;

//Ԥ������ͼ�Ĵ����ƶ�
public class PreviewerController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public RectTransform[] viewer;    //Ԥ������ͼrectTransform
    //public RectTransform viewer_in; //С��rectTransform
    //public RectTransform[] viewer_z;
    
    Vector3 startPos;               //�������ͼ��ʼ��
    Vector3 viewerCenter;           
    Vector3 offset = Vector3.zero;  //λ��ƫ����  

    bool canMove = true;            //���λ�� ����ܷ��ƶ�

    float minx, maxx, miny, maxy;   //�ƶ�����

    public float camMaxPosX;         //������ܿ���������߽�ʱ��xλ��
    public float camCenPosX;         //���������������ʱ��xλ��
    public float camMaxPosY;
    public float camCenPosY;
    private Vector2 moveFactor;        //���ֵӦ�ø���������������߽�ʱ��λ��-���������������ʱ��λ��/С��������ͼ�����ң�ʱ���ĵ�λ��-С��������ͼ����ʱ������λ��
    // Start is called before the first frame update
    void Start()
    {
        
        //viewer = GetComponent<RectTransform>();
        //viewer_in = gameObject.GetComponentInChildren<RectTransform>();
        viewer = gameObject.GetComponentsInChildren<RectTransform>();

        viewer[1].position = viewer[0].position;   //�����������غ�
        viewerCenter = viewer[1].position;         //��¼��ʼλ��

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
            //����������ƶ����� �ƶ������
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

    //���Ʒ�Χ
    void SetDragRange()
    {
        // ��Сx���� = ������ǰx���� - �������ľ�����߽�ľ��� + UI���ľ�����߽�ľ���
        minx = viewer[0].position.x - viewer[0].pivot.x * viewer[0].rect.width + viewer[1].rect.width * viewer[0].pivot.x;
        // ���x���� = ������ǰx���� + �������ľ����ұ߽�ľ��� - UI���ľ����ұ߽�ľ���
        maxx = viewer[0].position.x +(1- viewer[0].pivot.x) * viewer[0].rect.width - viewer[1].rect.width * (1-viewer[0].pivot.x);
        // ��Сy���� = ������ǰy���� - �������ľ���ױߵľ��� + UI���ľ���ױߵľ���
        miny = viewer[0].position.y - viewer[0].pivot.y * viewer[0].rect.height + viewer[1].rect.height * viewer[0].pivot.y;
        // ���y���� = ������ǰx���� + �������ľ��붥�ߵľ��� - UI���ľ��붥�ߵľ���
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
