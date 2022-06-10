using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GraphMoveControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IScrollHandler
{

    [SerializeField]
    private Vector2 scaleLimit;
    [SerializeField]
    private float scaleSpeed = 20f;

    private Vector3 mouseDownPosition;
    private Vector2 lastPosition;

    private RectTransform rectTransform;

    //--------面板呼出-------
    /*private bool isCallOut;
    [SerializeField]
    private float callOutTime = 1f;
    private Vector3 originScale;
    */
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //originScale = rectTransform.localScale;
        //isCallOut = true;
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scrollWheel = eventData.scrollDelta.y;
        if (Mathf.Abs(scrollWheel) > 0.01f)
        {
            Vector3 currentScale = rectTransform.localScale;
            currentScale += Vector3.one * scrollWheel * scaleSpeed * Time.deltaTime;
            if (currentScale.x > scaleLimit.x && currentScale.x < scaleLimit.y)
            {
                float scaleOffset = currentScale.x - rectTransform.localScale.x;
                rectTransform.localScale = currentScale;
                //修正偏移量
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, Camera.main, out Vector2 mousePos);
                Vector2 offset = mousePos * scaleOffset;
                rectTransform.anchoredPosition -= offset;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseDownPosition = Input.mousePosition;
        lastPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mouseOffset = Input.mousePosition - mouseDownPosition;
        Vector2 currentPos = lastPosition + mouseOffset;
        rectTransform.anchoredPosition = currentPos;
    }


    /// <summary>
    /// 呼出或者隐藏图纸面板
    /// </summary>
   /* public void CallOut()
    {
        if (isCallOut)
        {
            //隐藏面板
            isCallOut = false;
            Invoke("HideSelf", callOutTime);
            Vector3 scale = Vector3.zero * 0.01f;
            Vector2 position = new Vector3(0, -410);
            rectTransform.DOScale(scale, callOutTime);
            rectTransform.DOAnchorPos(position, callOutTime);
        }
        else
        {
            //呼出面板
            isCallOut = true;
            gameObject.SetActive(true);
            Vector2 position = new Vector3(0, 0);
            rectTransform.DOScale(originScale, callOutTime);
            rectTransform.DOAnchorPos(position, callOutTime);
        }
    }


    private void HideSelf()
    {
        gameObject.SetActive(isCallOut);
    }*/
}
