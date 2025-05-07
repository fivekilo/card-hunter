using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardNum { get; set; }
    //public int cardRarity { get; set; }
    public string cardText;
    public string cardName;
    public string cardType;
    private bool isDragging = false;
    private Vector3 originalPosition;
    //Card(int cardNUm) { this.cardNum = cardNum; }
    private SpriteRenderer cardFramework;
    private SpriteRenderer cardImage;
    private TextMeshProUGUI cardTextBox;
    private TextMeshProUGUI cardNameBox;
    private TextMeshProUGUI cardTypeBox;
    void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = "测试中卡名";
        cardText = "测试中卡牌描述";
        cardType = "类1";
    }
     void cardTextsInit (int cardNum){
        FindText(cardNum, ref cardName,ref cardText,ref cardType);
        GameObject Target = GameObject.Find("cardText");
        cardTextBox = Target.GetComponent<TextMeshProUGUI>();
        Target = GameObject.Find("cardName");
        cardNameBox= Target.GetComponent<TextMeshProUGUI>();
        Target = GameObject.Find("cardType");
        cardTypeBox= Target.GetComponent<TextMeshProUGUI>();
        cardTypeBox.text = cardType;
        cardTextBox.text = cardText;
        cardNameBox.text = cardName;
    }
    int GetcardRarity(int cardNum)
    {
        return cardNum;
    }
    void cardFrameworkInit(int cardNum)
    {
        Sprite newSprite;
        int cardRarity = GetcardRarity(cardNum);
        GameObject Target = GameObject.Find("cardFramework");
        cardFramework = Target.GetComponent<SpriteRenderer>();
        Target = GameObject.Find("cardImage");
        cardImage= Target.GetComponent<SpriteRenderer>();
        newSprite = Resources.Load<Sprite>("3");
        cardImage.sprite = newSprite;
        if (cardRarity == 0)
        {
            newSprite = Resources.Load<Sprite>("cardbottom");
            cardFramework.sprite = newSprite;
        }
        else if (cardRarity ==1)
        {
            newSprite = Resources.Load<Sprite>("cardbottom_blue");
            cardFramework.sprite = newSprite;
        }
        else if(cardRarity == 2)
        {
            newSprite = Resources.Load<Sprite>("cardbottom_god");
            cardFramework.sprite = newSprite;
        }
    }
    public void cardInit()
    {
        cardTextsInit(cardNum);
        cardFrameworkInit(cardNum);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card clicked: " + cardNum);
        // 卡牌被点击时的逻辑
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        // 提高卡牌层级，确保它在其他卡牌上方
        GetComponent<SpriteRenderer>().sortingOrder += 10;
    }

    // 拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        // 将屏幕坐标转换为世界坐标
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0; // 确保z坐标为0
        transform.position = mousePos;
    }

    // 结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder -= 10;

        // 检查是否放置在有效区域
        if (!IsValidDropZone())
        {
            // 如果不是有效区域，返回原位
            transform.position = originalPosition;
        }
    }

    private bool IsValidDropZone()
    {
        // 这里实现检查是否在有效放置区域的逻辑
        // 可以使用射线检测或其他方法
        return true; // 暂时返回true
    }
    //Start is called before the first frame update
    void Start()
    {
        //cardInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
