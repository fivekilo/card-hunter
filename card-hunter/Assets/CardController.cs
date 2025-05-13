using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 hoverScale = new Vector3(1.65f, 1.98f, 1f);
    private Vector3 normalScale = new Vector3(1.5f, 1.8f, 1f);
    private RectTransform rectTransform;
    private Canvas canvas;
    public Canvas canvas2;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private bool isDragging=false;
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
        isDragging = true;
        // �ö���ʾ
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

        // ���û����Ч���ã�����ԭλ
        if (!IsDroppedInValidZone(eventData))
        {
            ReturnToOriginalPosition();
        }
        isDragging = false;
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            transform.localScale = hoverScale;
            // ��߲㼶ȷ����ͣ���������Ϸ�
            canvas2.sortingOrder += 5;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            transform.localScale = normalScale;
            // �ָ��㼶
            canvas2.sortingOrder -= 5;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
