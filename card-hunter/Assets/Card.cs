using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    //private Vector2 hoverScale = new Vector2(1.2f, 1.2f);
    //private Vector2 normalScale = Vector2.one;
    public Canvas parentCanvas;
    //public int cardRarity { get; set; }
    public string cardText;
    public string cardName;
    public string cardType;

    //private bool isDragging = false;
    //private Vector2 originalPosition;
    public int cardNum { get; set; }
    public List<int> Move; //移动方向
    public Vector2Int MoveLength;//移动长度 下上限
    public int Derivation;//派生卡牌
    public bool MoveTurn;//移动是否导致转向
    public bool Consumption;//消耗
    public int DrawCard;//抽牌
    public bool Nothingness;//虚无
    public int OnlyLState;//0:不限 1:限自由态 2:限连携态
    public int EnterState;//1:进入自由态 2:进入连携态
    public List<int> Buff;//Buff
    public List<int> DeBuff;//DeBuff
    public List<int> AttackDirection; //攻击方向
    public int AttackLength;//攻击长度
    public int Cost;//费用
    public List<int> Attack;//攻击
    public int Defence;//格挡
    public int DeltaWound;
    public int DeltaCost;//回费
    public int DeltaBladeNum;//回气刃槽
    public int DeltaBladeLevel; //提升气刃等级
    public int DeltaHealth; //生命值变化

    //private TextMeshProUGUI cardTextBox;
    //private TextMeshProUGUI cardNameBox;
    //private TextMeshProUGUI cardTypeBox;
    void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = GameConfig.CardName[cardNum];
        cardText = GameConfig.CardText[cardNum];
        switch (GameConfig.CardType[cardNum])
        {
            case 0:
                cardType = "攻击";
                break;
            case 1:
                cardType = "防御";
                break;
            case 2:
                cardType = "移动";
                break;
            case 3:
                cardType = "能力";
                break;
        }
    }
     void cardTextsInit (int cardNum){
        TextMeshProUGUI cardTypeBox = GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI cardTextBox = GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI cardNameBox = GetComponentsInChildren<TextMeshProUGUI>()[2];
        FindText(cardNum, ref cardName,ref cardText,ref cardType);
        cardTypeBox.text = cardType;
        cardTextBox.text = cardText;
        cardNameBox.text = cardName;
    }
    void cardFrameworkInit(int cardNum)
    {
        Sprite newSprite;
        //SpriteRenderer cardFramework= GetComponentsInChildren<SpriteRenderer>()[0];
        Image cardFramework = GetComponentsInChildren<Image>()[0];
        newSprite = Resources.Load<Sprite>(GameConfig.CardImageName[cardNum]);
        cardFramework.sprite = newSprite;
    }
    public void cardInit()
    {
        cardTextsInit(cardNum);
        cardFrameworkInit(cardNum);
    }
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log("Card clicked: " + cardNum);
    //}
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    isDragging = true;
    //    // 提高卡牌层级，确保它在其他卡牌上方
    //    GetComponentInChildren<Canvas>().sortingOrder += 10;
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    // 将屏幕坐标转换为世界坐标
    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
    //    Debug.Log(mousePos.x);
    //    Debug.Log(mousePos.y);
    //    //mousePos.z = 0; // 确保z坐标为0
    //    transform.position = mousePos;
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    isDragging = false;
    //    GetComponentInChildren<Canvas>().sortingOrder -= 10;

    //    // 检查是否放置在有效区域
    //    if (!IsValidDropZone())
    //    {
    //        Debug.Log("被移回");
    //        // 如果不是有效区域，返回原位
    //        transform.position = originalPosition;
    //    }
    //}

    //private bool IsValidDropZone()
    //{
    //    // 这里实现检查是否在有效放置区域的逻辑
    //    // 可以使用射线检测或其他方法
    //    return true; // 暂时返回true
    //}
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (!isDragging)
    //    {
    //        //transform.localScale = hoverScale;
    //        // 提高层级确保悬停卡牌在最上方
    //        GetComponentInChildren<Canvas>().sortingOrder += 5;
    //    }
    //}
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!isDragging)
    //    {
    //        //transform.localScale = normalScale;
    //        // 恢复层级
    //        GetComponentInChildren<Canvas>().sortingOrder -= 5;
    //    }
    //}
        //Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
}
