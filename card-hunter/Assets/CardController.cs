using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // 置顶显示
        transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 如果没有有效放置，返回原位
        if (!IsDroppedInValidZone(eventData))
        {
            ReturnToOriginalPosition();
        }

    }
    private bool IsDroppedInValidZone(PointerEventData eventData)
    {
        return eventData.pointerEnter != null &&
               eventData.pointerEnter.CompareTag("DropZone");
    }

    private void ReturnToOriginalPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
