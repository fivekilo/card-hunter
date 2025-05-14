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
    public Card card;

    public delegate void CardUsedHandler(Card card);
    public event CardUsedHandler OnCardUsed; //卡牌被使用
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
        if (!card.CBuse)
        {
            Debug.Log(card.CBuse);
            eventData.pointerDrag = null;
            return;
        }
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 1.0f;//虚化程度
        canvasGroup.blocksRaycasts = false;
        isDragging = true;
        //transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if (rectTransform.anchoredPosition[1] > 100)
        {
            canvasGroup.alpha = 0.8f;
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // 如果没有有效放置，返回原位
        if (!IsDroppedInValidZone(eventData))
        {
            ReturnToOriginalPosition();
        }
        else
        {
            OnCardUsed?.Invoke(card);
            Debug.Log("卡牌被使用了");
            card.transform.position += new Vector3(10000, 0, 0);//后用对象池实现卡牌消失
        }
        
    }
    private bool IsDroppedInValidZone(PointerEventData eventData)
    {
        return eventData.pointerEnter != null &&
               rectTransform.anchoredPosition[1] > 100;
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
            // 提高层级确保悬停卡牌在最上方
            canvas2.sortingOrder += 5;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            transform.localScale = normalScale;
            // 恢复层级
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
